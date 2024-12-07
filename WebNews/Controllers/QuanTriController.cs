using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WebNews.Models;

namespace WebNews.Controllers
{
    public class QuanTriController : Controller
    {
        private readonly Data_NewsContext _context;

        public QuanTriController(Data_NewsContext context)
        {
            _context = context;
        }

        #region Quản lý thành viên
        // Quản lý danh sách thành viên
        public async Task<ActionResult> DanhSachThanhVien()
        {
            List<NguoiDung> nguoiDung = await _context.NguoiDungs
                                    .Include(n => n.BaiViets)
                                    .Include(n => n.BinhLuans)
                                    .Include(n => n.LuotThiches)
                                    .Where(n => n.MaQuyen == 2)
                                    .ToListAsync();

            return View(nguoiDung);
        }

        // Khóa tài khoản thành viên
        public async Task<ActionResult> KhoaThanhVien(string sTaiKhoan)
        {
            var taiKhoan = await _context.NguoiDungs.SingleOrDefaultAsync(n => n.TaiKhoan == sTaiKhoan);
            if (taiKhoan != null)
            {
                if (taiKhoan.TrangThai == 0)
                {
                    taiKhoan.TrangThai = 1;
                    TempData["SweetMessage"] = "Success|Đã khóa tài khoản thành công";
                }
                else
                {
                    taiKhoan.TrangThai = 0;
                    TempData["SweetMessage"] = "Success|Đã mở khóa tài khoản thành công";
                }
                _context.SaveChanges();

                return RedirectToAction("DanhSachThanhVien", "QuanTri");
            }

            TempData["ToastMessage"] = "Error|Khóa tài khoản thất bại";
            return View();
        }

        // Cập nhật thành viên
        [HttpGet]
        public async Task<ActionResult> CapNhatThanhVien(string sTaiKhoan)
        {
            var taiKhoan = await _context.NguoiDungs.SingleOrDefaultAsync(n => n.TaiKhoan == sTaiKhoan);
            if (taiKhoan == null)
            {
                TempData["SweetMessage"] = "Error|Không tìm thấy tài khoản";
                return RedirectToAction("DanhSachThanhVien", "QuanTri");
            }

            return View(taiKhoan);
        }

        // Cập nhật thành viên
        [HttpPost]
        public async Task<ActionResult> CapNhatThanhVien(NguoiDung model)
        {
            var nd = await _context.NguoiDungs.SingleOrDefaultAsync(n => n.TaiKhoan == model.TaiKhoan);
            nd.MatKhau = model.MatKhau;
            nd.Ho = model.Ho;
            nd.Ten = model.Ten;
            nd.NgaySinh = model.NgaySinh;
            nd.GioiTinh = model.GioiTinh;
            _context.SaveChanges();

            TempData["SweetMessage"] = "Success|Cập nhật thành công";
            return RedirectToAction("DanhSachThanhVien", "QuanTri");
        }
        #endregion

        #region Quản lý chủ để
        // Quản lý chủ đề
        public async Task<ActionResult> DanhSachChuDe()
        {
            List<ChuDe> chuDe = await _context.ChuDes
                                .Include(n => n.DanhMuc)
                                .ToListAsync();

            return View(chuDe);
        }

        // Ẩn/ hiện chủ đề
        public async Task<ActionResult> AnChuDe(int iMaChuDe)
        {
            var chuDe = await _context.ChuDes.SingleOrDefaultAsync(n => n.MaChuDe == iMaChuDe);
            if (chuDe != null)
            {
                if (chuDe.HienThi == true)
                {
                    chuDe.HienThi = false;
                    TempData["SweetMessage"] = "Success|Đã ẩn chủ đề thành công";
                }
                else
                {
                    chuDe.HienThi = true;
                    TempData["SweetMessage"] = "Success|Đã hiển thị chủ đề thành công";
                }
                _context.SaveChanges();

                return RedirectToAction("DanhSachChuDe", "QuanTri");
            }

            TempData["ToastMessage"] = "Error|Cập nhật trạng thái chủ đề thất bại";
            return View();
        }

        // Cập nhật chủ đề
        [HttpGet]
        public async Task<ActionResult> CapNhatChuDe(int iMaChuDe)
        {
            var chuDe = await _context.ChuDes.SingleOrDefaultAsync(n => n.MaChuDe == iMaChuDe);
            if (chuDe == null)
            {
                TempData["SweetMessage"] = "Error|Không tìm thấy chủ đề";
                return RedirectToAction("DanhSachChuDe", "QuanTri");
            }
            ViewBag.DanhMuc = _context.DanhMucs.ToList();

            return View(chuDe);
        }

        // Cập nhật thành viên
        [HttpPost]
        public async Task<ActionResult> CapNhatChuDe(ChuDe model)
        {
            var nd = await _context.ChuDes.SingleOrDefaultAsync(n => n.MaChuDe == model.MaChuDe);
            nd.TenChuDe = model.TenChuDe;
            nd.DanhMuc = model.DanhMuc;
            nd.DuongDan = model.DuongDan;
            _context.SaveChanges();

            TempData["SweetMessage"] = "Success|Cập nhật thành công";
            return RedirectToAction("DanhSachChuDe", "QuanTri");
        }
        #endregion

        #region Quản lý bài viết
        // Quản lý chủ đề
        public async Task<ActionResult> DanhSachBaiViet()
        {
            List<BaiViet> baiViet = await _context.BaiViets
                                         .Where(n => n.TrangThai == 1)
                                         .Include(n => n.ChuDe)
                                         .Include(n => n.NguoiDung)
                                         .ToListAsync();

            return View(baiViet);
        }

        //Ẩn/ hiện chủ đề
        public async Task<ActionResult> DuyetBaiViet(int iMaBaiViet)
        {
            var baiViet = await _context.BaiViets.SingleOrDefaultAsync(n => n.MaBaiViet == iMaBaiViet);
            if (baiViet != null)
            {
                baiViet.TrangThai = 0;
                TempData["SweetMessage"] = "Success|Đã duyệt bài viết thành công";
                _context.SaveChanges();

                return RedirectToAction("DanhSachBaiViet", "QuanTri");
            }

            TempData["ToastMessage"] = "Error|Duyệt bài viết thất bại";
            return View();
        }

        public async Task<ActionResult> XoaBaiViet(int iMaBaiViet)
        {

            try
            {
                var baiViet = await _context.BaiViets.SingleOrDefaultAsync(n => n.MaBaiViet == iMaBaiViet);
                if (baiViet != null)
                {
                    _context.BaiViets.Remove(baiViet);
                    _context.SaveChanges();
                }

                TempData["ToastMessage"] = "Success|Xóa bài viết thành công";
                return RedirectToAction("DanhSachBaiViet", "QuanTri");
            }
            catch
            {
                TempData["ToastMessage"] = "Error|Không thể thực hiện xóa bài viết";
                // Return erro 500
                return StatusCode(500, "Đã xảy ra lỗi. vui lòng thử lại");
            }

        }


        //// Cập nhật chủ đề
        //[HttpGet]
        //public async Task<ActionResult> CapNhatChuDe(int iMaChuDe)
        //{
        //    var chuDe = await _context.ChuDes.SingleOrDefaultAsync(n => n.MaChuDe == iMaChuDe);
        //    if (chuDe == null)
        //    {
        //        TempData["SweetMessage"] = "Error|Không tìm thấy chủ đề";
        //        return RedirectToAction("DanhSachChuDe", "QuanTri");
        //    }
        //    ViewBag.DanhMuc = _context.DanhMucs.ToList();

        //    return View(chuDe);
        //}

        //// Cập nhật thành viên
        //[HttpPost]
        //public async Task<ActionResult> CapNhatChuDe(ChuDe model)
        //{
        //    var nd = await _context.ChuDes.SingleOrDefaultAsync(n => n.MaChuDe == model.MaChuDe);
        //    nd.TenChuDe = model.TenChuDe;
        //    nd.DanhMuc = model.DanhMuc;
        //    nd.DuongDan = model.DuongDan;
        //    _context.SaveChanges();

        //    TempData["SweetMessage"] = "Success|Cập nhật thành công";
        //    return RedirectToAction("DanhSachChuDe", "QuanTri");
        //}
        #endregion
    }
}
