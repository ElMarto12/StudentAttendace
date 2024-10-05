using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAttendace.Config;
using StudentAttendace.Models.DbModels;

namespace StudentAttendace.Controllers.API;

[ApiController]
[Route("api/[controller]")]
public class TeachersController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
    {
        return await context.Teachers.ToListAsync();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<Teacher>> GetTeacherById(int id)
    {
        var teacher = await context.Teachers.FindAsync(id);

        if (teacher == null)
        {
            return NotFound();
        }

        return teacher;
    }

    [HttpGet]
    [Route("ById/{userid}")]
    public async Task<ActionResult<Teacher>> GetTeacherByUserId(int userid)
    {
        var teacher = await context.Teachers.Where(user => user.UserId == userid).FirstOrDefaultAsync();

        if (teacher == null)
        {
            return NotFound();
        }

        return teacher;
    }
}