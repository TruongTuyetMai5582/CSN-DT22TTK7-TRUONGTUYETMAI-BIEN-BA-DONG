using System;
using System.Collections.Generic;

namespace WebNews.Models
{
    public partial class BinhLuan
    {
        public int MaBinhLuan { get; set; }
        public string NoiDungBinhLuan { get; set; } = null!;
        public DateTime NgayBinhLuan { get; set; }
        public int? MaBaiViet { get; set; }
        public string? TaiKhoan { get; set; }

        public virtual BaiViet? BaiViet { get; set; }
        public virtual NguoiDung? NguoiDung { get; set; }
    }
}
