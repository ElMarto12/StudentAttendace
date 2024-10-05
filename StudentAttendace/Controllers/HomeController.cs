using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using StudentAttendace.Models;
using StudentAttendace.Services;

namespace StudentAttendace.Controllers;

public class HomeController(ILogger<HomeController> logger, TeacherService service) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != null)
        {
            var teacher = await service.GetTeacherByUserIdAsync(userId);
            ViewData["TeacherName"] = $"{teacher.Name} {teacher.LastName}";
        }
        
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}