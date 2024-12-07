using System;
using System.Collections.Generic;

namespace WebNews.Models
{
    public partial class LuotThich
    {
        public int MaBaiViet { get; set; }
        public string TaiKhoan { get; set; } = null!;
        public DateTime? NgayThich { get; set; }

        public virtual BaiViet BaiViet { get; set; } = null!;
        public virtual NguoiDung NguoiDung { get; set; } = null!;
    }
}
