using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OnlineLibrary.BLL.DTO;
using OnlineLibrary.BLL.Infrastructure;
using OnlineLibrary.BLL.Interfaces;
using OnlineLibrary.jobs;
using OnlineLibrary.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OnlineLibrary.Controllers
{
    public class AccountController : Controller
    {
        IEmailService EmailService;
        public AccountController()
        { }
        public AccountController(IEmailService emailServ)
        {
            EmailService = emailServ;
        }
        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = await UserService.Authenticate(userDto);
                if (claim != null)
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
            }
            return View(model);
        }
        public ActionResult Register()
        {
            ReservSheduler.Start();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                    Name = model.Name,
                    Role = "user",
                    Address = model.Address
                };
                OperationDetails operationDetails = await UserService.Create(userDto);
                if (operationDetails.Succedeed)
                    return RedirectToAction("Confirm", "Account");
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }
        public ActionResult Confirm()
        {
            return View();
        }
        [Authorize]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            if (email.Length > 0)
            {
                var callbackUrl = Url.Action("ResetPassword", "Account", new { id = UserService.FindByEmail(email).Id },
                    protocol: Request.Url.Scheme);
                await EmailService.Send(email, "Сброс пароля", "Для завершения регистрации перейдите по ссылке: <a href=\"" + callbackUrl + "\">сменить пароль</a>");
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult ResetPassword(string id)
        {
                ResetPassModel model = new ResetPassModel { Id = id };
                return View(model);
        }
        [HttpPost]
        public ActionResult ResetPassword(ResetPassModel model)
        {
            if (ModelState.IsValid)
            {
                UserService.ResetPass(model.Id, model.Password);
            }
            return RedirectToAction("Login", "Account");
        }
        private async Task SetInitialDataAsync()
        {
            await UserService.SetInitialData(new UserDTO
            {
                Email = "some@mail.ru",
                UserName = "some@mail.ru",
                Password = "deffolt",
                Name = "Иконников Евгений Сергеевич",
                Address = "ул. Спортивная, д.30, кв.75",
                Role = "admin",
            }, new List<string> { "user", "librarian", "admin" });
        }
    }
}