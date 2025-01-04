using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAttendace.Config;
using StudentAttendace.Models.DbModels;

namespace StudentAttendace.Controllers.API;

[ApiController]
[Route("api/[controller]")]
public class SubjectsController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Subject>>> GetSubjects()
    {
        return await context.Subjects.ToListAsync();
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<Subject>> GetSubjectById(int id)
    {
        var subject = await context.Subjects.FindAsync(id);

        if (subject == null)
        {
            return NotFound();
        }

        return subject;
    }

    [HttpGet]
    [Route("ByTeacher/{teacherId}")]
    public async Task<ActionResult<IEnumerable<Subject>>> GetSubjectsByTeacherId(int teacherId)
    {
        var subjects = await context.Subjects
            .Where(ts => ts.TeacherId == teacherId)
            .ToListAsync();

        if (subjects.Count == 0)
        {
            return NotFound();
        }

        return Ok(subjects);
    }
    
    [HttpPost]
    [Route("/CreateSubject")]
    public async Task<ActionResult<Subject>> AddNewSubject([FromForm] Subject subject, [FromForm] int teacherId)
    {
        var newSubject = new Subject
        {
            SubjectName = subject.SubjectName,
            LectureNumber = subject.LectureNumber,
            TeacherId = teacherId
        };

        context.Subjects.Add(newSubject);
        await context.SaveChangesAsync();

        return RedirectToAction("AdminSubject", "Admin");

    }

    [HttpPost]
    [HttpDelete]
    [Route("/DeleteSubject")]
    public async Task<ActionResult<Subject>> DeleteSubject(int subjectId)
    {
        var subject = await context.Subjects
            .Where(sub => sub.SubjectId == subjectId).FirstOrDefaultAsync();

        if (subject == null)
        {
            return NotFound();
        }

        context.Subjects.Remove(subject);
        await context.SaveChangesAsync();

        return RedirectToAction("AdminSubject", "Admin");
    }

    [HttpPost]
    [HttpPut]
    [Route("/UpdateSubject")]
    public async Task<ActionResult<Subject>> UpdateSubject([FromForm] Subject? subject)
    {
        if (subject != null)
        {
            var subjectToUpdate = await context.Subjects
                .Where(sub => sub.SubjectId == subject.SubjectId).FirstOrDefaultAsync();

            subjectToUpdate.SubjectName = subject.SubjectName;
            subjectToUpdate.LectureNumber = subject.LectureNumber;
            subjectToUpdate.TeacherId = subject.TeacherId;

            context.Entry(subjectToUpdate).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        return RedirectToAction("AdminSubject", "Admin");
    }

    [HttpPost]
    [Route("/CreateSubjectGroups")]
    public async Task<ActionResult<GroupsSubject>> AddSubjectGroups([FromForm] int subjectId, [FromForm] List<int> groupIds)
    {
        if (subjectId != null && groupIds.Count != 0)
        {
            foreach (var gId in groupIds)
            {
                var groupsSubject = new GroupsSubject
                {
                    SubjectId = subjectId,
                    GroupId = gId
                };

                context.GroupsSubjects.Add(groupsSubject);
            }

            await context.SaveChangesAsync();
        }

        await AddStudentSubjects(subjectId, groupIds);
        
        return RedirectToAction("AdminSubject", "Admin");
    }

    public async Task<ActionResult<StudentsSubject>> AddStudentSubjects(int subjectId, List<int> groupIds)
    {
        if (subjectId != null && groupIds.Count != 0)
        {
            foreach (var gId in groupIds)
            {
                var groupStudents = await context.Students
                    .Where(st => st.GroupId == gId).ToListAsync();

                foreach (var gStudents in groupStudents)
                {
                    var studentSubject = new StudentsSubject
                    {
                        StudentId = gStudents.StudentID,
                        SubjectId = subjectId
                    };

                    context.StudentsSubjects.Add(studentSubject);
                }
                
            }

            await context.SaveChangesAsync();
        }

        return Ok();
    }

    
}