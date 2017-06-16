using Microsoft.AspNet.Identity.Owin;
using OnlineLibrary.BLL.DTO;
using OnlineLibrary.BLL.Infrastructure;
using OnlineLibrary.BLL.Interfaces;
using OnlineLibrary.Models;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OnlineLibrary.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminController : Controller
    {
        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }
        public ActionResult AllUsers()
        {
            return View(UserService.AllUsers());
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
                ResetPassModel ViewModel = new ResetPassModel() { Id = id };
                return View(ViewModel);
        }
        [HttpPost]
        public ActionResult Edit(ResetPassModel model)
        {
            if (ModelState.IsValid)
            {
                UserService.ResetPass(model.Id, model.Password);
                return RedirectToAction("AllUsers", "Admin");
            }
            else
            {
                ModelState.AddModelError("", "Заполните поля");
            }
            return View();
        }
        public async Task<ActionResult> Delete(string id)
        {
            var result = await UserService.DeleteUser(id);
            if (result)
            {
                return RedirectToAction("AllUsers", "Admin");
            }
            else
            {
                ModelState.AddModelError("", "Удаление отменено");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                    Name = model.Name,
                    Role = model.Role,
                    Address = model.Address
                };
                OperationDetails operationDetails = await UserService.Create(userDto);
                if (operationDetails.Succedeed)
                    return RedirectToAction("AllUsers", "Admin");
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }
    }
}