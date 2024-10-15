using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using StudentAttendace.Models.DbModels;
using StudentAttendace.Services;

namespace StudentAttendace.Components;

public class TeacherInfoView(TeacherService teacherService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Content("User not authenticated");
        }

        var teacher = await teacherService.GetTeacherByUserIdAsync(userId);
        return View(teacher);
    }
}