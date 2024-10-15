using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAttendace.Config;
using StudentAttendace.Models.DbModels;

namespace StudentAttendace.Controllers.API;

[ApiController]
[Route("api/[controller]")]
public class GroupsController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
    {
        return await context.Groups.ToListAsync();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<Group>> GetGroupById(int id)
    {
        var group = await context.Groups.FindAsync(id);

        if (group == null)
        {
            return NotFound();
        }

        return group;
    }

    [HttpGet]
    [Route("ByTeacher/{teacherId}")]
    public async Task<ActionResult<IEnumerable<Group>>> GetGroupsByTeacherId(int teacherId)
    {
        var groups = await context.TeachersGroups
            .Where(tg => tg.TeacherId == teacherId)
            .Select(tg => tg.Group)
            .ToListAsync();

        if (groups.Count == 0)
        {
            return NotFound();
        }

        return groups;
    }
    
}