using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using JJNG.Web.Areas.AppIdentity.Models;
using JJNG.Data.AppIdentity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JJNG.Web.Areas.AppIdentity.Controllers
{
    [Area("AppIdentity")]
    [Authorize(Roles = "管理员,Admins")]
    public class UserAdminController : Controller
    {
        private UserManager<AppIdentityUser> userManager;
        private IUserValidator<AppIdentityUser> userValidator;
        private IPasswordValidator<AppIdentityUser> passwordValidator;
        private IPasswordHasher<AppIdentityUser> passwordHasher;
        private readonly AppIdentityDbContext _context;

        public UserAdminController(UserManager<AppIdentityUser> usrMgr,
                IUserValidator<AppIdentityUser> userValid,
                IPasswordValidator<AppIdentityUser> passValid,
                IPasswordHasher<AppIdentityUser> passwordHash,
                AppIdentityDbContext context)
        {
            userManager = usrMgr;
            userValidator = userValid;
            passwordValidator = passValid;
            passwordHasher = passwordHash;
            _context = context;
        }

        public ViewResult Index() => View(userManager.Users);

        public IActionResult Create()
        {
            var list_department = _context.UserDepartment.ToList();
            var list_position = _context.UserPosition.ToList();
            var list_belongto = _context.UserBelongTo.ToList();
            ViewData["Department"] = new SelectList(list_department, "DepartmentName", "DepartmentName");
            ViewData["Position"] = new SelectList(list_position, "PositionName", "PositionName");
            ViewData["BelongTo"] = new SelectList(list_belongto, "BelongToName", "BelongToName");
            return PartialView("~/Areas/AppIdentity/Views/UserAdmin/Create.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateModel model)
        {
            if (ModelState.IsValid)
            {
                AppIdentityUser user = new AppIdentityUser
                {
                    UserName = model.Name,
                    PhoneNumber=model.PhoneNumber,
                    Department=model.Department,
                    Position=model.Position,
                    BelongTo=model.BelongTo,
                    RegisterDate = DateTime.Now,
                    UserImage="/images/既见.jpg"
                };
                IdentityResult result
                    = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return PartialView("~/Areas/AppIdentity/Views/UserAdmin/Create.cshtml",model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/AppIdentity/Views/UserAdmin/Delete.cshtml", user.UserName);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id,IFormCollection form)
        {
            AppIdentityUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View("Index", userManager.Users);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Edit(string id)
        {
            var list_department = _context.UserDepartment.ToList();
            var list_position = _context.UserPosition.ToList();
            var list_belongto = _context.UserBelongTo.ToList();
            AppIdentityUser user = await userManager.FindByIdAsync(id);
            ViewData["Department"] = new SelectList(list_department, "DepartmentName", "DepartmentName",user.Department.ToString());
            ViewData["Position"] = new SelectList(list_position, "PositionName", "PositionName",user.Position.ToString());
            ViewData["BelongTo"] = new SelectList(list_belongto, "BelongToName", "BelongToName",user.BelongTo.ToString());
            if (user != null)
            {
                return PartialView("~/Areas/AppIdentity/Views/UserAdmin/Edit.cshtml", user);

            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Edit(string id, string PhoneNumber,string Department,string Position,string BelongTo,string password)
        {
            AppIdentityUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.PhoneNumber =PhoneNumber ;
                user.Department = Department;
                user.Position = Position;
                user.BelongTo = BelongTo;
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager,
                        user, password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user,
                            password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }
                if (( validPass == null)|| ( password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Redirect("/");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return PartialView("~/Areas/AppIdentity/Views/UserAdmin/Edit.cshtml", user);
        }

        [AllowAnonymous]
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

    }
}
