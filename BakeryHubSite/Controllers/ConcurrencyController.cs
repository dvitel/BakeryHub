using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BakeryHub.Domain;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace BakeryHubSite.Models
{
    public class ConcurrencyController : Controller
    {
        BakeryHubContext db;
        public ConcurrencyController(BakeryHubContext context) => db = context;

        public async Task<string> ChangeProduct(Product product, int cnt)
        {
            try
            {
                product.AvailableNow = product.AvailableNow - cnt;
                product.LastUpdated = DateTime.UtcNow;
                db.Products.Update(product);
                await db.SaveChangesAsync();
                return $"OK, new available is {product.AvailableNow}";
            }
            catch (DbUpdateConcurrencyException)
            {
                var dbEntity =
                    await
                        db.Products.AsNoTracking()
                            .Where(p => p.SupplierId == product.SupplierId
                                && p.ProductId == product.ProductId)
                            .FirstOrDefaultAsync();
                if (dbEntity.AvailableNow > cnt)
                {
                    product.AvailableNow = dbEntity.AvailableNow;
                    return await ChangeProduct(product, cnt);
                } else
                {
                    return "Not enogh product now." +
                            "Do you wish to proceed with hanshake?";
                }
            }
        }
        public async Task<IActionResult> Index2(Guid? id, int? cnt)
        {
            Response.ContentType = "text/plain";
            if (id == null)
                return Content("No product was specified for search");
            var product = await db.Products.FindAsync(id.Value);
            //here we have long running code
            return Content(await ChangeProduct(product, cnt ?? 1));
        }

        public async Task<IActionResult> Index(Guid? id, int? cnt)
        {
            try
            {
                Response.ContentType = "text/plain";
                if (id == null)
                    return Content("No product was specified for search");
                var product = await db.Products.FindAsync(id.Value);
                //here we have long running code
                product.AvailableNow = product.AvailableNow - (cnt ?? 1);
                product.LastUpdated = DateTime.UtcNow;
                db.Products.Update(product);
                await db.SaveChangesAsync();
                return Content($"OK, new available is {product.AvailableNow}");
            }
            catch (Exception e)
            {
                return Content(e.ToString());
            }
        }

    }
}