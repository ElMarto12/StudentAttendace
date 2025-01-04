using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAttendace.Config;
using StudentAttendace.Models.DbModels;

namespace StudentAttendace.Controllers.API;

[ApiController]
[Route("api/[controller]")]
public class SubjectsAttendanceController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubjectAttendance>>> GetSubjectsAttendance()
    {
        var subAttendances = await context.SubjectAttendances.ToListAsync();

        return subAttendances;
    }
    
    [HttpGet]
    [Route("BySubject/{subjectId}")]
    public async Task<ActionResult<IEnumerable<SubjectAttendance>>> GetSubjectAttendanceBySubjectId(int subjectId)
    {
        var sAttendance = await context.SubjectAttendances
            .Where(st => st.SubjectId == subjectId).ToListAsync();

        return sAttendance;
    }
    
    [HttpGet]
    [Route("ByStudent/{id}")]
    public async Task<ActionResult<IEnumerable<SubjectAttendance>>> GetSubjectAttendancesByStudentId(int id)
    {
        var subjectAttendances = await context.SubjectAttendances
            .Where(sa => sa.StudentId == id).ToListAsync();

        return subjectAttendances;
    }
}