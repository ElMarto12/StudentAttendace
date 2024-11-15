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
}