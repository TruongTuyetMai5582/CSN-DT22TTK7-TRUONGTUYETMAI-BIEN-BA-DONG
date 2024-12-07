using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebNews.Models;

namespace WebNews.Controllers
{
    public class DanhMucController : Controller
    {
        private readonly Data_NewsContext _context;

        public DanhMucController(Data_NewsContext context)
        {
            _context = context;
        }


    }
}
