using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebNews.Models;

namespace WebNews.Views.Components
{
    public class MenuComponent : ViewComponent
    {
        private readonly Data_NewsContext _context;

        public MenuComponent(Data_NewsContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<DanhMuc> menu = await _context.DanhMucs.Include(n => n.ChuDes).Where(n => n.HienThi == true).OrderBy(n => n.ThuTuHienThi).ToListAsync();

            return View("/Views/Components/Menu/Default.cshtml", menu);
        }
    }
}
