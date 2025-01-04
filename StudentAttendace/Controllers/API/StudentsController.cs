using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAttendace.Config;
using StudentAttendace.Models.DbModels;

namespace StudentAttendace.Controllers.API;

[ApiController]
[Route("api/[controller]")]
public class StudentsController(ApplicationDbContext context): ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
    {
        var students = await context.Students.ToListAsync();

        return students;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<Student>> GetStudentById(int id)
    {
        var student = await context.Students.FindAsync(id);

        if (student == null)
        {
            return NotFound();
        }
        
        return RedirectToAction("Attendances", "Home", student);
    }

    [HttpGet]
    [Route("ById/{id}")]
    public async Task<ActionResult<Student>> GetOneStudent(int id)
    {
        var student = await context.Students.FindAsync(id);
        return student;
    }
    
    
    [HttpGet]
    [Route("ByGroup/{groupId}")]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudentsByGroupId(int groupId)
    {
        var students = await context.Students
            .Where(gs => gs.GroupId == groupId)
            .ToListAsync();

        if (students.Count == 0)
        {
            return NotFound();
        }

        return students;
    }

    [HttpGet]
    [Route("StudentLectures/{lectureId}")]
    public async Task<ActionResult<IEnumerable<StudentsLecture>>> GetStudentsLecturesByLectureId(int lectureId)
    {
        var slectures = await context.StudentsLectures.Where(sl => sl.LectureId == lectureId).ToListAsync();

        if (slectures.Count == 0)
        {
            return NotFound();
        }

        return slectures;
    }

    [HttpGet]
    [Route("StudentLectures")]
    public async Task<ActionResult<IEnumerable<StudentsLecture>>> GetStudentsLecture()
    {
        var sLectures = await context.StudentsLectures.ToListAsync();

        return sLectures;
    }

    [HttpPost]
    [Route("/Create")]
    public async Task<ActionResult<Student>> AddNewStudent([FromForm] Student? student)
    {
        if (student != null)
        {
            var newStudent = new Student
            {
                Name = student.Name,
                LastName = student.LastName,
                GroupId = student.GroupId,
                Email = student.Email,
                Password = student.Password
            };

            await context.Students.AddAsync(newStudent);

            await context.SaveChangesAsync();
        }

        return RedirectToAction("AdminStudent", "Admin");
    }
    
    [HttpPost]
    [HttpDelete]
    [Route("/Delete")]
    public async Task<ActionResult<Student>> DeleteStudent(int studentId)
    {
        var student = await context.Students
            .Where(st => st.StudentID == studentId).FirstOrDefaultAsync();

        if (student != null)
        {
            context.Students.Remove(student);
            await context.SaveChangesAsync();
        }
        
        return RedirectToAction("AdminStudent", "Admin");
    }

    [HttpPost]
    [HttpPut]
    [Route("/Update")]
    public async Task<ActionResult<Student>> UpdateStudent([FromForm] Student student)
    {
        var studentUpdate = await context.Students.Where(st => st.StudentID == student.StudentID).FirstOrDefaultAsync();

        if (studentUpdate == null)
        {
            return NotFound();
        }

        studentUpdate.Name = student.Name;
        studentUpdate.LastName = student.LastName;
        studentUpdate.Password = student.Password;
        studentUpdate.Email = student.Email;
        studentUpdate.GroupId = student.GroupId;

        context.Entry(studentUpdate).State = EntityState.Modified;
        await context.SaveChangesAsync();

        return RedirectToAction("AdminStudent", "Admin");
    }
}