using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAttendace.Config;
using StudentAttendace.Models.DbModels;

namespace StudentAttendace.Controllers.API;

[ApiController]
[Route("api/[controller]")]
public class StudentsLecturesController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentsLecture>>> GetStudentsLectures()
    {
        var sLectures = await context.StudentsLectures.ToListAsync();

        return sLectures;
    }

    [HttpGet]
    [Route("ByLecture/{lectureId}")]
    public async Task<IEnumerable<StudentsLecture>> GetStudentsLecturesByLectureId(int lectureId)
    {
        var sLectures = await context.StudentsLectures
            .Where(sl => sl.LectureId == lectureId).ToListAsync();

        if (sLectures.Count == 0)
        {
            return null;
        }

        return sLectures;
    }
}