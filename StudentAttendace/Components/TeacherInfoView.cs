using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using StudentAttendace.Services;

namespace StudentAttendace.Components;

public class TeacherInfoView(TeacherService service) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Content("User not authenticated");
        }

        var teacher = await service.GetTeacherByUserIdAsync(userId);
        return View(teacher);
    }
}