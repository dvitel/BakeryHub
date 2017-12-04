using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BakeryHub.Models;
using BakeryHub.Domain;
using Microsoft.EntityFrameworkCore;

namespace BakeryHub.Controllers
{
    public class HomeController : Controller
    {
        BakeryHubContext db;
        public HomeController(BakeryHubContext context) => db = context;
        public async Task<IActionResult> Index()
        {
            var products =
                await (
                    from p in db.Products.Include(p => p.MainImage) select p
                ).ToListAsync();
            return View(products);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

    }
}
