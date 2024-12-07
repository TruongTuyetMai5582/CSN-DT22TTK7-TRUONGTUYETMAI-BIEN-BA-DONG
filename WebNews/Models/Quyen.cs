using System;
using System.Collections.Generic;

namespace WebNews.Models
{
    public partial class Quyen
    {
        public Quyen()
        {
            NguoiDungs = new HashSet<NguoiDung>();
        }

        public int MaQuyen { get; set; }
        public string TenQuyen { get; set; } = null!;

        public virtual ICollection<NguoiDung> NguoiDungs { get; set; }
    }
}
