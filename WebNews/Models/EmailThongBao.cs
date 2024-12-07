using System;
using System.Collections.Generic;

namespace WebNews.Models
{
    public partial class EmailThongBao
    {
        public string Email { get; set; } = null!;
        public bool? TrangThai { get; set; }
    }
}
