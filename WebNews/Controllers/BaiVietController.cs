using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Drawing;
using System.Reflection.PortableExecutable;
using WebNews.Models;

namespace WebNews.Controllers
{
    public class BaiVietController : Controller
    {
        private readonly Data_NewsContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BaiVietController(Data_NewsContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment; ;
        }


        public async Task<ActionResult> DanhSachBaiViet()
        {
            List<BaiViet> baiViet = await _context.BaiViets.Include(n => n.ChuDe).Include(n => n.NguoiDung).ToListAsync();
            return View(baiViet);
        }

        // Xem danh sách bài viết theo chủ đề
        public async Task<ActionResult> BaiVietChuDe(int iMaChuDe)
        {
            try
            {
                var chuDe = await _context.ChuDes.FirstOrDefaultAsync(n => n.MaChuDe == iMaChuDe);
                if (chuDe != null)
                {
                    var baiViet = await _context.BaiViets
                                        .Include(n => n.NguoiDung)
                                        .Include(n => n.ChuDe)
                                        .Where(n => n.MaChuDe == iMaChuDe && n.TrangThai == 0)
                                        .OrderByDescending(n => n.MaBaiViet)
                                        .ToListAsync();

                    chuDe.LuotXem++;
                    _context.SaveChanges();

                    ViewBag.ChuDe = chuDe.TenChuDe;

                    return View(baiViet);
                }
                else
                {
                    return NotFound(); // return 404
                }
            }
            catch
            {
                // Rollback
                _context.Database.RollbackTransaction();
                // Return erro 500
                return StatusCode(500, "Đã xảy ra lỗi. vui lòng thử lại");
            }
        }


        // Xem chi tiết bài viết
        [HttpGet]
        public async Task<ActionResult> XemChiTietBaiViet(int iMaBaiViet)
        {
            try
            {
                var baiViet = await _context.BaiViets
                                    .Include(n => n.NguoiDung)
                                    .Include(n => n.ChuDe)
                                    .FirstOrDefaultAsync(n => n.MaBaiViet == iMaBaiViet && n.TrangThai == 0);

                if (baiViet != null)
                {
                    baiViet.LuotXem++;
                    _context.SaveChanges();

                    // Lấy bài viết liền kề trước
                    ViewBag.baiVietTruoc = await _context.BaiViets
                                            .Where(n => n.MaBaiViet < iMaBaiViet && n.MaChuDe == baiViet.MaChuDe && n.TrangThai == 0)
                                            .OrderByDescending(n => n.MaBaiViet)
                                            .FirstOrDefaultAsync();

                    // Lấy bài viết liền kề sau
                    ViewBag.baiVietSau = await _context.BaiViets
                                            .Where(n => n.MaBaiViet > iMaBaiViet && n.MaChuDe == baiViet.MaChuDe && n.TrangThai == 0)
                                            .OrderBy(n => n.MaBaiViet)
                                            .FirstOrDefaultAsync();

                    // Hiển thị bình luận
                    ViewBag.BinhLuan = await _context.BinhLuans
                                            .Include(n => n.NguoiDung)
                                            .Where(n => n.MaBaiViet == iMaBaiViet)
                                            .OrderBy(n => n.MaBinhLuan)
                                            .ToListAsync();

                    // hiển thị danh sách người like
                    ViewBag.Like = await _context.LuotThiches
                                          .Include(n => n.NguoiDung)
                                         .Where(n => n.MaBaiViet == iMaBaiViet)
                                         .OrderByDescending(n => n.NgayThich)
                                         .ToListAsync();

                    // Hiển thị chủ đề hot
                    ViewBag.ChuDe = await _context.ChuDes.Where(n => n.HienThi == true).ToListAsync();

                    // Hiển thị bài viết liên quan
                    ViewBag.BaiVietLienQuan = await _context.BaiViets
                                                    .Where(n => n.MaChuDe == baiViet.MaChuDe && n.TrangThai == 0 && n.MaBaiViet != baiViet.MaBaiViet)
                                                    .ToListAsync();


                    return View(baiViet);
                }
                else
                {
                    // return 404
                    return NotFound();
                }
            }
            catch
            {
                // Return erro 500
                return StatusCode(500, "Đã xảy ra lỗi. vui lòng thử lại");
            }
        }

        // Thêm mới bình luận bài viết
        [HttpPost]
        public async Task<ActionResult> ThemBinhLuan(BinhLuan model)
        {
            try
            {
                #region Kiểm tra đăng nhập
                var json = HttpContext.Session.GetString("NguoiDung");

                if (json == null)
                {
                    TempData["SweetMessage"] = "Error|Vui lòng đăng nhập lại";
                    return Json(new { error = "Vui lòng đăng nhập lại" });
                    ////return RedirectToAction("DangNhap", "DangNhap");
                }
                var nguoiDungSesion = JsonConvert.DeserializeObject<NguoiDung>(json);
                #endregion

                var baiViet = await _context.BaiViets.SingleOrDefaultAsync(n => n.MaBaiViet == model.MaBaiViet);
                if (baiViet != null)
                {

                    BinhLuan bl = new BinhLuan();
                    bl.NoiDungBinhLuan = model.NoiDungBinhLuan;
                    bl.NgayBinhLuan = DateTime.Now.Date;
                    bl.MaBaiViet = model.MaBaiViet;
                    bl.TaiKhoan = nguoiDungSesion.TaiKhoan;
                    _context.BinhLuans.Add(bl);

                    baiViet.BinhLuan++;
                    _context.SaveChanges();

                    TempData["SweetMessage"] = "Success|Thêm bình luận thành công";
                    return Json(new { success = true, maBaiViet = model.MaBaiViet });
                }
                else
                {
                    TempData["ToastMessage"] = "Error|Không tồn tại bài viết";
                    // return 404
                    return NotFound();
                }
            }
            catch
            {
                // Return erro 500
                return StatusCode(500, "Đã xảy ra lỗi. vui lòng thử lại");
            }
        }



        // Thêm mới bài viết
        [HttpGet]
        public async Task<ActionResult> ThemMoiBaiViet()
        {
           List<ChuDe> chuDe = await _context.ChuDes.ToListAsync();
            return View(chuDe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ThemMoiBaiViet(BaiViet model, IFormFile imageFile)
        {
            try
            {
                #region Kiểm tra đăng nhập
                var json = HttpContext.Session.GetString("NguoiDung");

                if (json == null)
                {
                    TempData["SweetMessage"] = "Error|Vui lòng đăng nhập lại";
                    return Json(new { error = "Vui lòng đăng nhập lại" });
                    ////return RedirectToAction("DangNhap", "DangNhap");
                }
                var nguoiDungSesion = JsonConvert.DeserializeObject<NguoiDung>(json);
                #endregion


                #region Thêm mới bài viết
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Xác định đường dẫn đến thư mục wwwroot/assets/images
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Assets", "HinhAnh", "AnhBaiViet");

                    // Kiểm tra xem thư mục đã tồn tại chưa
                    if (!Directory.Exists(uploadsFolder))
                    {
                        // Nếu thư mục không tồn tại, tạo mới thư mục
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Tạo tên tệp hình ảnh duy nhất
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;

                    // Đường dẫn đầy đủ đến tệp hình ảnh
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Lưu hình ảnh vào thư mục đích
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    // Nếu lưu thành công, bạn có thể thực hiện các thao tác tiếp theo ở đây

                    BaiViet bv = new BaiViet();
                    bv.HinhAnh = uniqueFileName;
                    bv.TenBaiViet = model.TenBaiViet;
                    bv.NoiDungBaiViet = model.NoiDungBaiViet;
                    bv.NgayDang = DateTime.Now.Date;
                    bv.NgayCapNhat = DateTime.Now.Date;
                    if (nguoiDungSesion.MaQuyen == 1) // admin
                    {
                        bv.TrangThai = 0;
                    }
                    else
                    {
                        bv.TrangThai = 1; // Kiểm duyệt
                    }
                    bv.LuotThich = 0;
                    bv.BinhLuan = 0;
                    bv.LuotXem = 0;
                    bv.MaChuDe = model.MaChuDe;
                    bv.TaiKhoan = nguoiDungSesion.TaiKhoan;

                    // cộng điểm thành tích
                    var diem = await _context.NguoiDungs.SingleOrDefaultAsync(n => n.TaiKhoan == nguoiDungSesion.TaiKhoan);
                    diem.DiemThanhTich = diem.DiemThanhTich + 5;
                    diem.SoBaiViet = diem.SoBaiViet + 1;

                    _context.BaiViets.Add(bv);
                    _context.SaveChanges();
                    TempData["SweetMessage"] = "Success|Thêm bài viết thành công";

                    #endregion

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["SweetMessage"] = "Error|Lỗi upload hình ảnh";
                    return View();
                }
            }
            catch
            {
                TempData["ToastMessage"] = "Error|Không thể thực hiện";
                // Return erro 500
                return StatusCode(500, "Đã xảy ra lỗi. vui lòng thử lại");
            }
        }
    }
}
