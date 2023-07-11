using Microsoft.AspNetCore.Mvc;

namespace SmartdustApp.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class LeaveController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
