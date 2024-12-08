using Microsoft.AspNetCore.Mvc;

namespace StudentAttendace.Controllers;

public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}