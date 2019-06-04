using JJNG.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JJNG.Web.ViewComponents
{
    public class Branch2ViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;
        public Branch2ViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list_branch = await _context.FncBranch.Where(x => x.BranchName != "运营中心" && x.BranchName != "町隐学院").ToListAsync();
            return View(list_branch);
        }
    }
}
