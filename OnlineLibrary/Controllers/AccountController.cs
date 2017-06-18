using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OnlineLibrary.BLL.DTO;
using OnlineLibrary.BLL.Infrastructure;
using OnlineLibrary.BLL.Interfaces;
using OnlineLibrary.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OnlineLibrary.Controllers
{
    public class AccountController : Controller
    {
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
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
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
                {
                    try
                    {
                        using (MailMessage message = new MailMessage("onlinelibrary17@mail.ru", model.Email))
                        {
                            message.Subject = "Доступ на сайт";
                            message.Body = model.Password + "- ваш пароль для доступа на сайт!";
                            using (SmtpClient client = new SmtpClient
                            {
                                EnableSsl = true,
                                Host = "smtp.mail.ru",
                                Port = 587,
                                Credentials = new NetworkCredential("onlinelibrary17@mail.ru", "password123")
                            })
                            {
                                await client.SendMailAsync(message);
                            }
                        }
                    }
                    catch
                    { }
                    return RedirectToAction("Confirm", "Account");
                }
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }
        public ActionResult Confirm()
        {
            return View();
        }
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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            if (ModelState.IsValid)
            {
                UserDTO user = await UserService.FindByEmail(email);
                if (user != null)
                {
                    string code = await UserService.GenerateTokenAsync(user.Id);
                    string callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserService.SendEmail(user.Id, "Сброс пароля", "Для сброса пароля, перейдите по ссылке <a href=\"" + callbackUrl + "\">сбросить</a>");
                }
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
                if (UserService.FindById(model.Id) == null)
                    HttpNotFound();
                UserService.ResetPass(model.Id, model.Password);
            }
            return RedirectToAction("Login", "Account");
        }
        private async Task SetInitialDataAsync()
        {
            await UserService.SetInitialData(new UserDTO
            {
                Email = "zzzz@mail.ru",
                UserName = "zzzz@mail.ru",
                Password = "password",
                Name = "Иконников Евгений Сергеевич",
                Address = "ул. Казанская, д.30, кв.75",
                Role = "admin",
            }, new List<string> { "user", "librarian", "admin" });
        }
    }
}