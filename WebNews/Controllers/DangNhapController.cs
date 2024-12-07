using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebNews.Models;
namespace WebNews.Controllers
{
    public class DangNhapController : Controller
    {
        private readonly Data_NewsContext _context;

        public DangNhapController(Data_NewsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> DangNhap(string TaiKhoan, string MatKhau)
        {
            try
            {
                var nguoiDung = await _context.NguoiDungs.SingleOrDefaultAsync(n => n.TaiKhoan == TaiKhoan && n.MatKhau == MatKhau);

                if (nguoiDung != null)
                {
                    if (nguoiDung.TrangThai != 0)
                    {
                        TempData["SweetMessage"] = "Success|Tài khoản của bạn đã bị khóa";

                        return View();
                    }

                    // Serialize object thành chuỗi JSON
                    var json = JsonConvert.SerializeObject(nguoiDung);
                    HttpContext.Session.SetString("NguoiDung", json);

                    TempData["ToastMessage"] = "Success|Đăng nhập thành công";

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["SweetMessage"] = "Error|Tài khoản không hợp lệ";
                    return View();
                }
            }
            catch
            {
                // Return erro 500
                return StatusCode(500, "Đã xảy ra lỗi. vui lòng thử lại");
            }
        }

        [HttpGet]
        public ActionResult DangKi()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> DangKi(NguoiDung model)
        {
            try
            {
                var nguoiDung = await _context.NguoiDungs.SingleOrDefaultAsync(n => n.TaiKhoan == model.TaiKhoan);

                if (nguoiDung == null)
                {
                    NguoiDung nd = new NguoiDung();
                    nd.TaiKhoan = model.TaiKhoan;
                    nd.MatKhau = model.MatKhau;
                    nd.Ho = model.Ho;
                    nd.Ten = model.Ten;
                    nd.GioiTinh = model.GioiTinh;
                    nd.NgaySinh = model.NgaySinh;
                    nd.TrangThai = 0;
                    nd.HinhAnh = "AnhMacDinh.png";
                    nd.DiemThanhTich = 0;
                    nd.SoBaiViet = 0;
                    nd.MaQuyen = 2;

                    _context.NguoiDungs.Add(nd);
                    _context.SaveChanges();

                    TempData["SweetMessage"] = "Success|Tạo mới tài khoản thành công";

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["SweetMessage"] = "Error|Tài khoản đã tồn tại";
                    return View();
                }
            }
            catch
            {
                // Return erro 500
                return StatusCode(500, "Đã xảy ra lỗi. vui lòng thử lại");
            }
        }

        public ActionResult DangXuat()
        {
            // Xóa thông tin người dùng khỏi session
            HttpContext.Session.Remove("NguoiDung");

            // Đánh dấu rằng người dùng đã đăng xuất
            HttpContext.Session.SetString("DaDangXuat", "true");

            TempData["ToastMessage"] = "Success|Đăng xuất thành công";

            return RedirectToAction("Index", "Home");
        }
    }
}
