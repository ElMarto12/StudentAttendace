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

    [HttpGet]
    [Route("ByStudent/{studentId}")]
    public async Task<ActionResult<Group>> GetGroupByStudentId(int studentId)
    {
        var group = await context.Groups.FindAsync(studentId);

        if (group == null)
        {
            return NotFound();
        }
        
        return group;
    }
    
    [HttpGet]
    [Route("BySubject/{subjectId}")]
    public async Task<ActionResult<IEnumerable<Group>>> GetGroupsBySubjectId(int subjectId)
    {
        var groups = await context.GroupsSubjects
            .Where(gs => gs.SubjectId == subjectId)
            .Select(gs => gs.Group)
            .ToListAsync();
        
        if(groups.Count == 0)
        {
            return NotFound();
        }

        return groups;
        
    }

    [HttpPost]
    [Route("/CreateGroup")]
    public async Task<ActionResult<Group>> AddNewGroup([FromForm] Group? group)
    {
        if (group != null)
        {
            var newGroup = new Group
            {
                GroupName = group.GroupName
            };
            
            await context.Groups.AddAsync(newGroup);
            await context.SaveChangesAsync();
        }

        return RedirectToAction("AdminGroup", "Admin");
    }

    [HttpPost]
    [HttpDelete]
    [Route("/DeleteGroup")]
    public async Task<ActionResult<Group>> DeleteGroup(int groupId)
    {
        var group = await context.Groups.Where(gr => gr.GroupID == groupId).FirstOrDefaultAsync();

        if (group != null)
        {
            context.Groups.Remove(group);
            await context.SaveChangesAsync();
        }

        return RedirectToAction("AdminGroup", "Admin");
    }
}