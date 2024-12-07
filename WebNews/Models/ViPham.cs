using System;
using System.Collections.Generic;

namespace WebNews.Models
{
    public partial class ViPham
    {
        public int MaViPham { get; set; }
        public string NoiDungViPham { get; set; } = null!;
        public string? TaiKhoan { get; set; }

        public virtual NguoiDung? NguoiDung { get; set; }
    }
}
