using System;
using System.Collections.Generic;

namespace WebNews.Models
{
    public partial class ChuDe
    {
        public ChuDe()
        {
            BaiViets = new HashSet<BaiViet>();
        }

        public int MaChuDe { get; set; }
        public string TenChuDe { get; set; } = null!;
        public int? LuotXem { get; set; }
        public bool? HienThi { get; set; }
        public string? DuongDan { get; set; }
        public int? ThuTuHienThi { get; set; }
        public int? MaDanhMuc { get; set; }

        public virtual DanhMuc? DanhMuc { get; set; }
        public virtual ICollection<BaiViet> BaiViets { get; set; }
    }
}
