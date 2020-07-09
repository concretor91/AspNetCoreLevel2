using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels.Identity;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _SignInManager;
        private readonly ILogger<AccountController> logger;

        public AccountController(UserManager<User> UserManager, SignInManager<User> SignInManager, ILogger<AccountController> logger)
        {
            _UserManager = UserManager;
            _SignInManager = SignInManager;
            this.logger = logger;
        }

        #region Register new user

        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);

            var user = new User
            {
                UserName = Model.UserName
            };

            logger.LogInformation($"Начинается процесс регистрации пользователя {user.UserName}");
            var registration_result = await _UserManager.CreateAsync(user, Model.Password);
            if (registration_result.Succeeded)
            {
                logger.LogInformation($"Пользователь {user.UserName} успешно зарегистрирован");
                var add_user_role_result = await _UserManager.AddToRoleAsync(user, Role.User);
                if (add_user_role_result.Succeeded)
                    logger.LogInformation($"Пользователяю {user.UserName} успешно добавлена роль {Role.User}");
                else
                {
                    logger.LogInformation(
                        $"Ошибка добавления пользователю {user.UserName} роли {Role.User}:{string.Join(",", add_user_role_result.Errors.Select(x => x.Description))}");
                    throw new ApplicationException("Ошибка наделения нового пользователя ролью Пользователь");
                }

                await _SignInManager.SignInAsync(user, false);
                logger.LogInformation($"Пользователь {user.UserName} успешно зарегистрирован");
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in registration_result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(Model);
        }

        #endregion

        #region Login user
        
        public IActionResult Login(string ReturnUrl) => View(new LoginViewModel { ReturnUrl = ReturnUrl});

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);

            logger.LogInformation($"Процесс входа пользователя {Model.UserName}");
            var login_result = await _SignInManager.PasswordSignInAsync(
                Model.UserName,
                Model.Password,
                Model.RememberMe,
                false);

            if (login_result.Succeeded)
            {
                logger.LogInformation($"Пользователь {Model.UserName} вошел успешно");
                if (Url.IsLocalUrl(Model.ReturnUrl))
                    return Redirect(Model.ReturnUrl);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                logger.LogInformation($"Ошибка входа пользователя {Model.UserName}");
            }

            ModelState.AddModelError(string.Empty, "Неверное имя пользователя, или пароль!");

            return View(Model);
        }

        #endregion

        public async Task<IActionResult> Logout()
        {
            await _SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();
    }
}
