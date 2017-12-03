using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BakeryHub.Models;
using BakeryHub.Domain;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using BakeryHub.Services;

namespace BakeryHub.Controllers
{
    
    public class SellerController : Controller
    {
        BakeryHubContext db;
        IConfiguration config;
        IHostingEnvironment env;
        IEmailService email;
        public SellerController(BakeryHubContext context, IConfiguration configuration, IHostingEnvironment envi, IEmailService emails) => 
            (db, config, env, email) = (context, configuration, envi, emails);

        [Authorize(Policy = "Seller", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue("Id"));
            var products = 
                await 
                    (from p in db.Products
                     where p.SupplierId == userId
                     select p).ToListAsync();
            var productImages =
                await
                    (from i in db.ProductImages
                     join p in db.Products on i.ProductId equals p.ProductId
                     where p.SupplierId == userId
                     select i).ToListAsync();
            var groupedImages =
                productImages.GroupBy(i => i.ProductId).ToDictionary(kv => kv.Key, kv => kv.ToList());
            foreach (var product in products)
            {
                var key = product.ProductId;
                if (groupedImages.ContainsKey(key))
                {
                    product.Images = groupedImages[key];
                }
                else
                    product.Images = new List<ProductImage>(); 
            }
            return View(new SellerDashboard { Products = products });
        }

        [HttpGet]
        [Authorize(Policy = "Seller", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Product(Guid? id)
        {
            var userId = int.Parse(User.FindFirstValue("Id"));
            var categories =
                await (from c in db.ProductCategories select c).ToListAsync();
            if (id.HasValue)
            {
                var product =
                    await
                        (from p in db.Products
                         where p.SupplierId == userId && p.ProductId == id.Value
                         select p).FirstOrDefaultAsync();
                if (product == null)
                {
                    //TODO: general error message
                    return RedirectToAction("Index");
                } else
                {
                    var images =
                        await (from i in db.ProductImages where i.ProductId == id.Value select i).ToListAsync();
                    return View(new ProductViewModel {
                        ProductId = product.ProductId,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        AvailableInStore = product.AvailableNow,
                        ImagePathes =
                            images.Select(i => i.LogicalPath).ToList(),
                        CategoryId = product.ProductCategoryId,
                        Categories = categories
                    });
                }
            }
            return View(new ProductViewModel { Categories = categories });
        }

        private async Task<IList<ProductImage>> SaveImages(int userId, IList<IFormFile> files, Guid? productId = null)
        {
            var productImages = new List<ProductImage>();
            for (var i = 0; i < files.Count; i++)
            {
                var ext = Path.GetExtension(files[i].FileName);
                var path = Path.Combine(env.WebRootPath, "products", $"{userId}_{productId}_{i}{ext}");
                var logicalPath = $"/products/{userId}_{productId}_{i}{ext}";
                using (var file = System.IO.File.OpenWrite(path))
                {
                    await files[i].CopyToAsync(file);
                }
                var img = new ProductImage
                {
                    Path = path,
                    LogicalPath = logicalPath,
                    Mime = files[i].ContentType,
                };
                if (productId.HasValue) img.ProductId = productId.Value;
                productImages.Add(img);
            }
            return productImages;
        }

        [HttpPost]
        [Authorize(Policy = "Seller", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Product(ProductViewModel productModel)
        {
            var userId = int.Parse(User.FindFirstValue("Id"));
            if (ModelState.IsValid)
            {
                if (productModel.ProductId.HasValue)
                {
                    //update
                    var product = await (from p in db.Products
                                         where p.SupplierId == userId && p.ProductId == productModel.ProductId.Value
                                         select p).FirstOrDefaultAsync();
                    if (product == null)
                    {
                        //TODO: error message in cookies
                        return RedirectToAction("Index");
                    } else
                    {
                        var productImages =
                            await (from p in db.ProductImages
                                   join pr in db.Products on p.ProductId equals pr.ProductId
                                   where pr.SupplierId == userId && p.ProductId == productModel.ProductId.Value
                                   select p).ToListAsync();
                        product.Name = productModel.Name;
                        product.Description = productModel.Description;
                        product.AvailableNow = productModel.AvailableInStore;
                        product.Price = productModel.Price;
                        product.ProductCategoryId = productModel.CategoryId;
                        product.Images = await SaveImages(userId, productModel.Images, product.ProductId);
                        db.ProductImages.RemoveRange(productImages);
                        db.Products.Update(product);
                        await db.SaveChangesAsync();
                    }
                } else
                {
                    //insert

                    var product =
                        new Product
                        {
                            SupplierId = userId,
                            AvailableNow = productModel.AvailableInStore,
                            Name = productModel.Name,
                            Description = productModel.Description,
                            Price = productModel.Price,
                            Images = await SaveImages(userId, productModel.Images),
                            ProductCategoryId = productModel.CategoryId
                        };

                    await db.Products.AddAsync(product);
                    await db.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }
            productModel.Categories = 
                await (from c in db.ProductCategories select c).ToListAsync();
            return View(productModel);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Register() {
            if (User.FindFirstValue("Seller") == "1")
                return RedirectToAction("Index");
            return View(new SellerRegistration {
                States = await (from s in db.States select s).ToListAsync(),
                Contacts = new List<SellerContact>
                {
                    new SellerContact { Type = Contact.ContactType.Email.ToString() }
                }
            });
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Register(SellerRegistration sellerRegistration)
        {
            if (User.FindFirstValue("Seller") == "1")
                return RedirectToAction("Index");
            var userId = int.Parse(User.FindFirstValue("Id"));
            var states = await (from s in db.States select s).ToListAsync();
            if (!states.Any(state => state.Code == sellerRegistration.StateId))
                ModelState.AddModelError("StateId", "Specified unknown state");
            if (sellerRegistration.Contacts == null || sellerRegistration.Contacts.Count == 0)
            {
                ModelState.AddModelError("Contacts[0]", "Should specify at least one contact");
            }
            if (ModelState.IsValid)
            {
                var logoPath = "";
                if (sellerRegistration.Logo != null) {
                    var logoLogicalPath = Path.Combine($"logos", $"{userId}{Path.GetExtension(sellerRegistration.Logo.FileName)}");
                    logoPath = Path.Combine(env.WebRootPath, logoLogicalPath);
                }
                var user = await db.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
                user.Name = sellerRegistration.Name;
                var seller =
                    new Supplier
                    {
                        Id = userId,
                        Description = sellerRegistration.Description,
                        HasLogo = logoPath != "",
                        IsCompany = sellerRegistration.IsCompany,
                        //Addresses = new List<SupplierAddress>
                        //{
                        //    new SupplierAddress
                        //    {
                        //        SupplierId = userId,
                        //        AddressId = 0,
                        //        City = sellerRegistration.City,
                        //        StateId = sellerRegistration.StateId,
                        //        isUIVisible = true,
                        //        Street = sellerRegistration.Street,
                        //        Zip = sellerRegistration.Zip
                        //    }
                        //},
                        //Contacts =
                        //    sellerRegistration.Contacts.Select((c, i) =>
                        //        new SupplierContact
                        //        {
                        //            SupplierId = userId,
                        //            ContactId = i,
                        //            Address = c.Address,
                        //            Name = c.Name,
                        //            IsConfirmed = false,
                        //            IsUIVisible = true,
                        //            Type = Enum.Parse<ContactType>(c.Type)
                        //        }
                        //    ).ToList()
                    };
                using (var tran = await db.Database.BeginTransactionAsync())
                {
                    db.Users.Update(user);
                    await db.Suppliers.AddAsync(seller);
                    await db.SaveChangesAsync();
                    if (logoPath != "")
                        using (var file = System.IO.File.OpenWrite(logoPath))
                        {
                            await sellerRegistration.Logo.CopyToAsync(file);
                        }
                    var claims = new List<Claim>(User.Claims);
                    claims.Add(new Claim("Seller", "1"));
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(new ClaimsIdentity(claims)),
                            new AuthenticationProperties { IsPersistent = User.FindFirstValue("Persistent") == "1" });
                    tran.Commit();
                }
                return RedirectToAction("Index");
            }
            else
            {
                sellerRegistration.States = states;
                return View(sellerRegistration);
            }
        }
    }
}