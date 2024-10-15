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
}