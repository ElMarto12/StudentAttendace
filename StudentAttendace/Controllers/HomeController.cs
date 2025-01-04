using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using StudentAttendace.Models;
using StudentAttendace.Models.DbModels;
using StudentAttendace.Services;

namespace StudentAttendace.Controllers;

public class HomeController(ILogger<HomeController> logger, TeacherService teacherService, GroupService groupService, SubjectService subjectService, LectureService lectureService, StudentService studentService) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != null)
        {
            var teacher = await teacherService.GetTeacherByUserIdAsync(userId);
            ViewData["TeacherName"] = $"{teacher.Name} {teacher.LastName}";
        }
        
        return View();
    }

    public async Task<IActionResult> Lectures()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != null)
        {
            var teacher = await teacherService.GetTeacherByUserIdAsync(userId);
            var subjects = await subjectService.GetSubjectsByTeacherIdAsync(teacher.TeacherID.ToString());
            var lectures = await lectureService.GetLecturesBySubjectIdAsync(subjects);
            var groups = await groupService.GetGroupsAsync();
            
            ViewBag.Subject = subjects;
            ViewBag.Lecture = lectures;
            ViewBag.Group = groups;
            
            return View();
        }

        return View();
    }

    public async Task<ActionResult> Groups()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != null)
        {
            var teacher = await teacherService.GetTeacherByUserIdAsync(userId);
            var groups = await groupService.GetGroupsByTeacherIdAsync(teacher.TeacherID.ToString());

            List<Student> students = new List<Student>();

            foreach (var g in groups)
            {
                students.AddRange(await studentService.GetStudentsByGroupIdAsync(g.GroupID.ToString()));
            }

            ViewBag.Student = students;
            
            return View(groups);
        }
        
        return View();
    }
    
    public async Task<IActionResult> Archives()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId != null)
        {
            var teacher = await teacherService.GetTeacherByUserIdAsync(userId);
            var subjects = await subjectService.GetSubjectsByTeacherIdAsync(teacher.TeacherID.ToString());
            var lectures = await lectureService.GetLecturesBySubjectIdAsync(subjects);

            ViewBag.Lecture = lectures;
            
            return View(subjects);
        }

        return View();
    }
    
    public async Task<IActionResult> LectureAttendance(Lecture lecture)
    {
        IEnumerable<Student> students = await studentService.GetStudentsAsync();

        if (lecture.IsAttended)
        {
            IEnumerable<StudentsLecture> sLectures =
                await studentService.GetStudentsLecturesByLectureIdAsync(lecture.LectureId.ToString());

            ViewBag.StudentLecture = sLectures;
        }
        
        ViewBag.Student = students;
        
        return View(lecture);
    }

    public async Task<IActionResult> Attendances(Student student)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var teacher = await teacherService.GetTeacherByUserIdAsync(userId);
        
        IEnumerable<Subject> subjects = await subjectService.GetSubjectsAsync();
        IEnumerable<SubjectAttendance> subjectAttendances = await subjectService.GetSubjectAttendancesAsync();
        
        ViewBag.Subject = subjects;
        ViewBag.SubjectAttendance = subjectAttendances;
        ViewBag.Teacher = teacher;
        
        return View(student);
    }
    
    public IActionResult GenerateQrCode(int lectureId)
    {
        var url = Url.Action("StudentQrLogin", "Home", new { lectureId = lectureId }, Request.Scheme);

        using (var qrGenerator = new QRCodeGenerator())
        using (var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q))
        using (var qrCode = new PngByteQRCode(qrCodeData))
        {
            var qrCodeImage = qrCode.GetGraphic(20);
            
            return File(qrCodeImage, "image/png");
        }
    }

    public IActionResult StudentQrLogin(int lectureId)
    {
        ViewBag.LectureId = lectureId;
        
        return View();
    }

    public async Task<IActionResult> GenerateReports()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var teacher = await teacherService.GetTeacherByUserIdAsync(userId);
        var subjects = await subjectService.GetSubjectsByTeacherIdAsync(teacher.TeacherID.ToString());
        var groups = await groupService.GetGroupsByTeacherIdAsync(teacher.TeacherID.ToString());
        
        List<Student> students = new List<Student>();

        foreach (var group in groups)
        {
            students.AddRange(await studentService.GetStudentsByGroupIdAsync(group.GroupID.ToString()));
        }

        ViewBag.Subject = subjects;
        ViewBag.Group = groups;
        ViewBag.Student = students;
        
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}