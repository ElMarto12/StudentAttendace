using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using StudentAttendace.Config;
using StudentAttendace.Hubs;
using StudentAttendace.Models.DbModels;

namespace StudentAttendace.Controllers.API;

[ApiController]
[Route("api/[controller]")]
public class LecturesController : ControllerBase
{
    
    private readonly IHubContext<AttendanceHub> hubContext;
    private readonly ApplicationDbContext context;

    public LecturesController(IHubContext<AttendanceHub> _hubContext, ApplicationDbContext _context)
    {
        hubContext = _hubContext;
        context = _context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Lecture>>> GetLectures()
    {
        return await context.Lectures.ToListAsync();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<Lecture>> GetLectureById(int id)
    {
        var lecture = await context.Lectures.FindAsync(id);

        if (lecture == null)
        {
            return NotFound();
        }

        return RedirectToAction("LectureAttendance", "Home", lecture);
    }

    [HttpGet]
    [Route("BySubject/{subjectId}")]
    public async Task<ActionResult<IEnumerable<Lecture>>> GetLecturesBySubjectId(int subjectId)
    {
        var lectures = await context.Lectures
            .Where(sl => sl.SubjectId == subjectId)
            .ToListAsync();

        if (lectures.Count == 0)
        {
            return NotFound();
        }

        return lectures;
    }

    [HttpPost]
    [Route("Save")]
    public async Task<IActionResult> SaveAttendance([FromForm] int lectureId, [FromForm] Dictionary<int, bool> attendance)
    {
        if (attendance == null || !attendance.Any())
        {
            ModelState.AddModelError("", "Nėra duomenų apie dalyvavimą.");
            return RedirectToAction("Lecture", new { id = lectureId });
        }

        foreach (var entry in attendance)
        {
            int studentId = entry.Key;
            bool isParticipating = entry.Value;
            
            var existingEntry = await context.StudentsLectures
                .FirstOrDefaultAsync(sl => sl.LectureId == lectureId && sl.StudentId == studentId);

            if (existingEntry != null)
            {
                existingEntry.IsParticipating = isParticipating;
                existingEntry.Time = DateTime.Now;
                context.StudentsLectures.Update(existingEntry);
            }
            else
            {
                var newEntry = new StudentsLecture
                {
                    LectureId = lectureId,
                    StudentId = studentId,
                    IsParticipating = isParticipating,
                    Time = DateTime.Now
                };
                await context.StudentsLectures.AddAsync(newEntry);
            }
        }
        
        await context.SaveChangesAsync();

        await CalculateAttendancePercentage(lectureId);
        
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> CalculateAttendancePercentage(int lectureId) 
    {
    var subjectId = await context.Lectures
        .Where(l => l.LectureId == lectureId)
        .Select(l => l.SubjectId)
        .FirstOrDefaultAsync();

    if (subjectId == 0)
    {
        return NotFound("Dalykas nerastas.");
    }

    var subject = await context.Subjects.FindAsync(subjectId);
    if (subject == null)
    {
        return NotFound("Dalykas nerastas.");
    }
    
    var subjectLectures = await context.Lectures
        .Where(sl => sl.SubjectId == subject.SubjectId)
        .ToListAsync();
    
    var lectureStudents = await context.StudentsLectures
        .Where(st => st.LectureId == lectureId)
        .Select(st => st.Student)
        .ToListAsync();

    int totalLectures = subject.LectureNumber; //10

    foreach (var student in lectureStudents)
    {
        int notAttendedLectures = 0;
        
        foreach (var lecture in subjectLectures)
        {
            var attendance = await context.StudentsLectures
                .Where(sl => sl.LectureId == lecture.LectureId && sl.StudentId == student.StudentID)
                .FirstOrDefaultAsync();

            if (attendance != null && attendance.IsParticipating == false)
            {
                notAttendedLectures++;
            }
        }


        notAttendedLectures = totalLectures - notAttendedLectures;
        decimal attendancePercentage = (decimal)(notAttendedLectures * 100) / totalLectures;
        
        var existingAttendance = await context.SubjectAttendances
            .FirstOrDefaultAsync(sa => sa.SubjectId == subject.SubjectId && sa.StudentId == student.StudentID);

        if (existingAttendance != null)
        {
            existingAttendance.AttendancePercentage = attendancePercentage;
            context.SubjectAttendances.Update(existingAttendance);
        }
        else
        {
            var newAttendance = new SubjectAttendance
            {
                AttendancePercentage = attendancePercentage,
                SubjectId = subject.SubjectId,
                StudentId = student.StudentID
            };
            await context.SubjectAttendances.AddAsync(newAttendance);
        }
    }

    await context.SaveChangesAsync();

    return Ok();
    }

    [HttpPost]
    [Route("/QrAttendance")]
    public async Task<IActionResult> QrAttendance([FromForm] int lectureId, [FromForm] string email, [FromForm] string password)
    {
        
        var student = await context.Students.FirstOrDefaultAsync(s => s.Email == email && s.Password == password);
        if (student == null) return Unauthorized();

        await hubContext.Clients.Group(lectureId.ToString()).SendAsync("ReceiveAttendanceUpdate", student.StudentID);

        return RedirectToAction("StudentQrLogin", "Home");
    }
    
}