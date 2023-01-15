using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using WebStore.Domain.Entities.Identity;
using WebStore.ViewModels;
using WebStore.ViewModels.Identity;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _SignInManager;

        public AccountController(UserManager<User> UserManager, SignInManager<User> SignInManager)
        {
            _UserManager = UserManager;
            _SignInManager = SignInManager;
        }

        public IActionResult Register() => View(new RegisterUserViewModel());// Регистрация
        
        [HttpPost, ValidateAntiForgeryToken] //ValidateAntiForgeryToken - безопасность
        public async Task<IActionResult> Register(RegisterUserViewModel Model)//EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(Model);

            var user = new User
            {
                UserName = Model.UserName,
            };
            var registration_result = await _UserManager.CreateAsync(user, Model.Password);
            if (registration_result.Succeeded)
            {
                await _SignInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in registration_result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(Model);
        }


        public IActionResult Login(string ReturnUrl) => View(new LoginViewModel { ReturnUrl = ReturnUrl });//Вход в систему

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel Model)
        {
            if (!ModelState.IsValid)
                return View(Model);

            var login_result = await _SignInManager.PasswordSignInAsync( //Принимаем данные в login-result
                Model.UserName,
                Model.Password,
                Model.RememberMe,
                true);

            if (login_result.Succeeded)
            {
                //return Redirect(Model.ReturnUrl); // Не безопасно!!!

                //if(Url.IsLocalUrl(Model.ReturnUrl))
                //    return Redirect(Model.ReturnUrl);
                //return RedirectToAction("Index", "Home");

                return LocalRedirect(Model.ReturnUrl ?? "/"); //Может быть пустой ссылкой
            }

            ModelState.AddModelError("", "Неверное имя пользователя, или пароль");

            return View(Model);
        }
        public async Task<IActionResult> Logout()
        {
            await _SignInManager.SignOutAsync(); // Выход из системы
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View(); // Отказ в доступе
    }
}
