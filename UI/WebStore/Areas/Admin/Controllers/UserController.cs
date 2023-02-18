using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities.Identity;
using WebStore.Services.Interfaces;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Administrators)]
    public class UserController : Controller
    {
        private IUsersData _UsersData { get; set; }
        public UserController(IUsersData UsersData)
        {
            _UsersData = UsersData;
        }
        public IActionResult Index()
        {
            var users = _UsersData.GetUsers();
            return View(users);
        }
    }
}
