using JJNG.Data;
using JJNG.Data.AppIdentity;
using JJNG.Data.Personnel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JJNG.Web.Areas.Branch.Controllers
{
    [Area("Branch")]
    [Authorize(Roles = "Admins,前台,管家")]
    public class BrhNoteAccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

        public BrhNoteAccountController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identityContext = identitycontext;
            _userManager = usrMgr;
        }

        public async Task<IActionResult> Index()
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            ViewData["Department"] = _user.Department;
            var psnNoteAccount = await _context.PsnNoteAccount.Where(x => x.Branch == _user.Branch&&(string.IsNullOrEmpty(x.Manager)||x.Manager==_user.UserName)).ToListAsync();
            return View(psnNoteAccount);
        }
    }
}
