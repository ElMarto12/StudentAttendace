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
    
}