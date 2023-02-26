using Microsoft.AspNetCore.Mvc;

namespace WebStore.WebAPI.Controllers
{
    [ApiController]
    [Route("api/employees")] // http://localhost:5001/api/employees
    public class EmployeesApiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
