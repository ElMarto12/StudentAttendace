using Microsoft.AspNetCore.Mvc;
using StudentAttendace.Services;

namespace StudentAttendace.Controllers;

public class AdminController(StudentService studentService, GroupService groupService, TeacherService teacherService, UserService userService, SubjectService subjectService, LectureService lectureService) : Controller
{
    public async Task<IActionResult> AdminStudent()
    {
        var students = await studentService.GetStudentsAsync();
        var groups = await groupService.GetGroupsAsync();

        ViewBag.Group = groups;
        
        return View(students);
    }

    public async Task<IActionResult> CreateStudent()
    {
        var groups = await groupService.GetGroupsAsync();

        ViewBag.Group = groups;
        
        return View();
    }

    public async Task<IActionResult> UpdateStudent(int studentId)
    {
        var student = await studentService.GetOneStudentAsync(studentId.ToString());
        var groups = await groupService.GetGroupsAsync();

        ViewBag.Group = groups;

        return View(student);
    }

    public async Task<IActionResult> AdminGroup()
    {
        var groups = await groupService.GetGroupsAsync();

        return View(groups);
    }

    public IActionResult CreateGroup()
    {
        return View();
    }

    public async Task<IActionResult> AdminTeacher()
    {
        var teachers = await teacherService.GetTeachersAsync();

        return View(teachers);
    }

    public IActionResult CreateTeacher()
    {
        return View();
    }

    public async Task<IActionResult> UpdateTeacher(int teacherId)
    {
        var teacher = await teacherService.GetTeacherIdAsync(teacherId);
        var user = await userService.GetUserById(teacher.UserId);

        ViewBag.User = user;
        return View(teacher);
    }

    public async Task<IActionResult> AdminSubject()
    {
        var subjects = await subjectService.GetSubjectsAsync();

        return View(subjects);
    }

    public async Task<IActionResult> CreateSubject()
    {
        var teachers = await teacherService.GetTeachersAsync();

        return View(teachers);
    }

    public async Task<IActionResult> UpdateSubject(int subjectId)
    {
        var subject = await subjectService.GetSubjectByIdAsync(subjectId.ToString());
        var teachers = await teacherService.GetTeachersAsync();

        ViewBag.Teacher = teachers;

        return View(subject);
    }

    public async Task<IActionResult> SubjectGroups(int subjectId)
    {
        var subject = await subjectService.GetSubjectByIdAsync(subjectId.ToString());

        var groups = await groupService.GetGroupsAsync();

        ViewBag.Group = groups;

        return View(subject);
    }

    public async Task<IActionResult> TeacherGroups(int teacherId)
    {
        var teacher = await teacherService.GetTeacherIdAsync(teacherId);
        var groups = await groupService.GetGroupsAsync();

        ViewBag.Group = groups;

        return View(teacher);
    }

    public async Task<IActionResult> AdminLecture()
    {
        var lectures = await lectureService.GetLecturesAsync();
        var lectureTimes = await lectureService.GetLectureTimesAsync();
        var groups = await groupService.GetGroupsAsync();
        var subjects = await subjectService.GetSubjectsAsync();
        
        ViewBag.LectureTime = lectureTimes;
        ViewBag.Group = groups;
        ViewBag.Subject = subjects;
        
        return View(lectures);
    }

    public async Task<IActionResult> CreateLecture()
    {
        var groups = await groupService.GetGroupsAsync();
        var subjects = await subjectService.GetSubjectsAsync();
        var lectureTimes = await lectureService.GetLectureTimesAsync();

        ViewBag.Group = groups; 
        ViewBag.Subject = subjects;
        ViewBag.LectureTime = lectureTimes;
        
        return View();
    }

    public async Task<IActionResult> UpdateLecture(int lectureId)
    {
        var lectures = await lectureService.GetLecturesAsync();
        lectures = lectures.Where(l => l.LectureId == lectureId);

        var lecture = lectures.FirstOrDefault();

        var groups = await groupService.GetGroupsAsync();
        var subjects = await subjectService.GetSubjectsAsync();
        var lectureTimes = await lectureService.GetLectureTimesAsync();


        ViewBag.Group = groups;
        ViewBag.Subject = subjects;
        ViewBag.LectureTime = lectureTimes;

        return View(lecture);
    }
}
