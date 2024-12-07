using System;
using System.Collections.Generic;

namespace WebNews.Models
{
    public partial class BaiViet
    {
        public BaiViet()
        {
            BinhLuans = new HashSet<BinhLuan>();
            LuotThiches = new HashSet<LuotThich>();
        }

        public int MaBaiViet { get; set; }
        public string? HinhAnh { get; set; }
        public string TenBaiViet { get; set; } = null!;
        public string NoiDungBaiViet { get; set; } = null!;
        public DateTime NgayDang { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public int? TrangThai { get; set; }
        public int? LuotThich { get; set; }
        public int? BinhLuan { get; set; }
        public int? LuotXem { get; set; }
        public int? MaChuDe { get; set; }
        public string? TaiKhoan { get; set; }

        public virtual ChuDe? ChuDe { get; set; }
        public virtual NguoiDung? NguoiDung { get; set; }
        public virtual ICollection<BinhLuan> BinhLuans { get; set; }
        public virtual ICollection<LuotThich> LuotThiches { get; set; }
    }
}
