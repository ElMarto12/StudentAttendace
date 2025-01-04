using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using StudentAttendace.Models.DbModels;
using StudentAttendace.Services;

namespace StudentAttendace.Controllers;

public class ReportController(PdfReportService pdfService, StudentService studentService, IWebHostEnvironment environment, TeacherService teacherService, SubjectService subjectService, GroupService groupService, LectureService lectureService) : Controller
{
    public async Task<IActionResult> GenerateReportBySubject(int subjectId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var teacher = await teacherService.GetTeacherByUserIdAsync(userId);
        var subject = await subjectService.GetSubjectByIdAsync(subjectId.ToString());
        var groups = await groupService.GetGroupsBySubjectIdAsync(subjectId.ToString());
        var sLectures = await studentService.GetStudentsLecturesAsync();
        var sAttendances = await subjectService.GetSubjectAttendanceBySubjectIdAsync(subjectId);
        
        List<Student> students = new List<Student>();

        foreach (var group in groups)
        {
            students.AddRange(await studentService.GetStudentsByGroupIdAsync(group.GroupID.ToString()));
        }
        
        string filePath = Path.Combine(environment.WebRootPath, "Reports", "report.pdf");
        string fileTitle = "Lankomumas";
        
        pdfService.GenerateAttendanceReport(filePath, students, teacher, subject, groups, fileTitle, sLectures, sAttendances);

        var stream = System.IO.File.OpenRead(filePath);
        return File(stream, "application/pdf", "report.pdf");
    }

    public async Task<IActionResult> GenerateStudentAttendanceReportByTeacher(int studentId) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var teacher = await teacherService.GetTeacherByUserIdAsync(userId);
        var student = await studentService.GetOneStudentAsync(studentId.ToString());
        var subjects = await subjectService.GetSubjectsByTeacherIdAsync(teacher.TeacherID.ToString());
        var group = await groupService.GetGroupByStudentIdAsync(studentId);
        var studentLectures = await studentService.GetStudentsLecturesAsync();
        var subjectAttendances = await subjectService.GetSubjectAttendanceByStudentIdAsync(studentId);
        var lectures = await lectureService.GetLecturesAsync(); 

        string filePath = Path.Combine(environment.WebRootPath, "Reports", $"report_{studentId}.pdf");
        string fileTitle = "Lankomumo Ataskaita";

        pdfService.GenerateStudentAttendanceReportByTeacher(filePath, teacher, student, subjects, group, fileTitle, studentLectures, subjectAttendances, lectures);

        var stream = System.IO.File.OpenRead(filePath);
        return File(stream, "application/pdf", $"report_{studentId}.pdf");

    }
}