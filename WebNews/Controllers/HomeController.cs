using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebNews.Models;

namespace WebNews.Controllers
{
    public class HomeController : Controller
    {
        private readonly Data_NewsContext _context;

        public HomeController(Data_NewsContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<DanhMuc> danhMuc = _context.DanhMucs.Include(n => n.ChuDes).Where(n => n.HienThi == true).OrderBy(n => n.ThuTuHienThi).ToList();
            ViewBag.DanhMuc = danhMuc;

            // Hiển thị danh sách bài viết
            var listBaiViet = _context.BaiViets.Include(tk => tk.NguoiDung).Include(bv => bv.ChuDe).Where(n => n.TrangThai == 0).Take(6).ToList();

            // Hiển thị chủ đề hot
            ViewBag.ChuDe =  _context.ChuDes.Where(n => n.HienThi == true).ToList();

            // Hiển thị bài viết gói cước
            //var listBaiViet = _context.BaiViets.Include(tk => tk.NguoiDung).Include(bv => bv.ChuDe).Where(n => n.TrangThai == 0).Take(3).ToList();

            return View(listBaiViet);
        }

        public ActionResult Menu()
        {
            var listDanhMuc = _context.DanhMucs.ToList();
            return PartialView(listDanhMuc);
        }


        // Thêm mới email
        // Xem danh sách bài viết theo chủ đề
        //[HttpPost]
        public async Task<ActionResult> ThemMoiEmail(string Email)
        {
            try
            {
                // Kiểm tra email đã tồn tại trong csdl hay chưa
                var email = await _context.EmailThongBaos.SingleOrDefaultAsync(n => n.Email == Email);
                if (email != null)
                {
                    // thực hiện thêm mới email
                    EmailThongBao em = new EmailThongBao();
                    em.Email = Email;
                    em.TrangThai = true;
                    _context.EmailThongBaos.Add(em);
                    _context.SaveChanges();

                    TempData["SweetMessage"] = "Success|Thêm email thành công";
                }
                else
                {
                    TempData["SweetMessage"] = "Error|Email đã tồn tại trong hệ thống";
                }

                List<DanhMuc> danhMuc = _context.DanhMucs.Include(n => n.ChuDes).Where(n => n.HienThi == true).OrderBy(n => n.ThuTuHienThi).ToList();
                ViewBag.DanhMuc = danhMuc;

                // Hiển thị danh sách bài viết
                var listBaiViet = _context.BaiViets.Include(tk => tk.NguoiDung).Include(bv => bv.ChuDe).Where(n => n.TrangThai == 0).Take(6).ToList();
                
                return View(listBaiViet);
            }
            catch
            {
                // Rollback
                _context.Database.RollbackTransaction();
                // Return erro 500
                return StatusCode(500, "Đã xảy ra lỗi. vui lòng thử lại");
            }
        }
    }
}
