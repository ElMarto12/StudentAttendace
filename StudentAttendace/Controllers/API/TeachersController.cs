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

    [HttpPost]
    [Route("/CreateTeacher")]
    public async Task<ActionResult<Teacher>> AddNewTeacher([FromForm] Teacher? teacher, [FromForm] User? user)
    {
        if (user != null)
        {
            var newUser = new User
            {
                Email = user.Email,
                Password = user.Password,
                Role = "Teacher"
            };

            context.Users.Add(newUser);
            await context.SaveChangesAsync();
        }

        var createdUser = await context.Users
            .Where(usr => user != null && usr.Email == user.Email && usr.Password == user.Password).FirstOrDefaultAsync();

        if (createdUser != null)
        {
            var newTeacher = new Teacher
            {
                Name = teacher?.Name,
                LastName = teacher?.LastName,
                UserId = createdUser.UserID
            };

            context.Teachers.Add(newTeacher);
            await context.SaveChangesAsync();
        }

        return RedirectToAction("AdminTeacher", "Admin");
    }

    [HttpPost]
    [HttpDelete]
    [Route("/DeleteTeacher")]
    public async Task<ActionResult<Teacher>> DeleteTeacher(int teacherId)
    {
        var teacher = await context.Teachers
            .Where(t => t.TeacherID == teacherId).FirstOrDefaultAsync();

        var teacherUser = await context.Users
            .Where(usr => teacher != null && usr.UserID == teacher.UserId).FirstOrDefaultAsync();

        if (teacher != null && teacherUser != null)
        {
            context.Users.Remove(teacherUser);
            context.Teachers.Remove(teacher);
            await context.SaveChangesAsync();
        }

        return RedirectToAction("AdminTeacher", "Admin");
    }

    [HttpPost]
    [HttpPut]
    [Route("/UpdateTeacher")]
    public async Task<ActionResult<Teacher>> UpdateTeacher([FromForm] Teacher teacher, [FromForm] User user)
    {
        var teacherUpdate = await context.Teachers
            .Where(t => t.TeacherID == teacher.TeacherID).FirstOrDefaultAsync();

        var userUpdate = await context.Users
            .Where(u => u.UserID == user.UserID).FirstOrDefaultAsync();

        if (teacherUpdate == null || userUpdate == null)
        {
            return NotFound();
        }

        teacherUpdate.Name = teacher.Name;
        teacherUpdate.LastName = teacher.LastName;
        teacherUpdate.UserId = user.UserID;

        userUpdate.Email = user.Email;
        userUpdate.Password = user.Password;

        context.Entry(userUpdate).State = EntityState.Modified;
        context.Entry(teacherUpdate).State = EntityState.Modified;
        await context.SaveChangesAsync();

        return RedirectToAction("AdminTeacher", "Admin");
    }

    [HttpPost]
    [Route("/CreateTeacherGroups")]
    public async Task<ActionResult<TeachersGroup>> AddTeacherGroups([FromForm] int teacherId,
        [FromForm] List<int> groupIds)
    {
        if (teacherId != null && groupIds.Count != 0)
        {
            foreach (var gId in groupIds)
            {
                var teacherGroups = new TeachersGroup
                {
                    TeacherId = teacherId,
                    GroupId = gId
                };

                context.TeachersGroups.Add(teacherGroups);
            }

            await context.SaveChangesAsync();
        }

        return RedirectToAction("AdminTeacher", "Admin");
    }
}