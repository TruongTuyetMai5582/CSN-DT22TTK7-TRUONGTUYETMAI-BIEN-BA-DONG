using System;
using System.Collections.Generic;

namespace WebNews.Models
{
    public partial class DanhMuc
    {
        public DanhMuc()
        {
            ChuDes = new HashSet<ChuDe>();
        }

        public int MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; } = null!;
        public bool? HienThi { get; set; }
        public string? DuongDan { get; set; }
        public int? ThuTuHienThi { get; set; }

        public virtual ICollection<ChuDe> ChuDes { get; set; }
    }
}
