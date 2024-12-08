using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using StudentAttendace.Models.DbModels;
using StudentAttendace.Services;

namespace StudentAttendace.Controllers;

public class ReportController(PdfReportService pdfService, StudentService studentService, IWebHostEnvironment environment, TeacherService teacherService, SubjectService subjectService, GroupService groupService) : Controller
{
    public async Task<IActionResult> GenerateReportBySubject(int subjectId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var teacher = await teacherService.GetTeacherByUserIdAsync(userId);
        var subject = await subjectService.GetSubjectByIdAsync(subjectId.ToString());
        var groups = await groupService.GetGroupsBySubjectIdAsync(subjectId.ToString());

        List<Student> students = new List<Student>();

        foreach (var group in groups)
        {
            students.AddRange(await studentService.GetStudentsByGroupIdAsync(group.GroupID.ToString()));
        }
        
        string filePath = Path.Combine(environment.WebRootPath, "Reports", "report.pdf");
        string fileTitle = "Lankomumas";
        
        pdfService.GenerateAttendanceReport(filePath, students, teacher, subject, groups, fileTitle);

        var stream = System.IO.File.OpenRead(filePath);
        return File(stream, "application/pdf", "report.pdf");
    }
}