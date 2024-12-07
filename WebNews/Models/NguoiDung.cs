using System;
using System.Collections.Generic;

namespace WebNews.Models
{
    public partial class NguoiDung
    {
        public NguoiDung()
        {
            BaiViets = new HashSet<BaiViet>();
            BinhLuans = new HashSet<BinhLuan>();
            LuotThiches = new HashSet<LuotThich>();
            ViPhams = new HashSet<ViPham>();
        }

        public string TaiKhoan { get; set; } = null!;
        public string MatKhau { get; set; } = null!;
        public string Ho { get; set; } = null!;
        public string Ten { get; set; } = null!;
        public int? GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }
        public int? TrangThai { get; set; }
        public string? HinhAnh { get; set; }
        public int? DiemThanhTich { get; set; }
        public int? SoBaiViet { get; set; }
        public int MaQuyen { get; set; }

        public virtual Quyen? Quyen { get; set; }
        public virtual ICollection<BaiViet> BaiViets { get; set; }
        public virtual ICollection<BinhLuan> BinhLuans { get; set; }
        public virtual ICollection<LuotThich> LuotThiches { get; set; }
        public virtual ICollection<ViPham> ViPhams { get; set; }
    }
}
