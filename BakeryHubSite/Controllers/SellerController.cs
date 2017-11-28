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

namespace BakeryHub.Controllers
{
    public class SellerController : Controller
    {
        BakeryHubContext db;
        IConfiguration config;
        public SellerController(BakeryHubContext context, IConfiguration configuration) => 
            (db, config) = (context, configuration);

        [Authorize(Policy = "Seller", AuthenticationSchemes = "S-Cookie")]
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult Login(string r) => View(new LoginData { Redirect = r });

        private string GenerateSalt()
        {
            return Guid.NewGuid().ToString();
        }
        private string MD5Password(string password, string salt)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password + " " + salt));
                return BitConverter.ToString(bytes);
            }
        }
        private string DESPassword(string password, string salt)
        {
            using (var des = TripleDES.Create())
            {
                des.Key = Convert.FromBase64String(config["DESPassword:Key"]);
                des.IV = Convert.FromBase64String(config["DESPassword:Vect"]);
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.None;
                using (var enc = des.CreateEncryptor())
                {
                    using (var memory = new MemoryStream()) {
                        using (var cstream = new CryptoStream(memory, enc, CryptoStreamMode.Write))
                        {
                            var bytes = System.Text.Encoding.UTF8.GetBytes(password + " " + salt);
                            cstream.Write(bytes, 0, bytes.Length);
                        }
                        return Convert.ToBase64String(memory.ToArray());
                    }
                }
            }
        }
        private bool ValidatePassword(User dbUser, LoginData login)
        {
            if (dbUser.PasswordEncryptionAlgorithm == Domain.User.PasswordEncryption.Plain
                && dbUser.Password == login.Password)
                return true;
            if (dbUser.PasswordEncryptionAlgorithm == Domain.User.PasswordEncryption.MD5
                && dbUser.Password == MD5Password(login.Password, dbUser.Salt))
                return true;
            if (dbUser.PasswordEncryptionAlgorithm == Domain.User.PasswordEncryption.DES
                && dbUser.Password == DESPassword(login.Password, dbUser.Salt))
                return true;
            return false;
        }

        private (Domain.User.PasswordEncryption, string, string) ComputePassword(RegisterData reg)
        {
            if (config["PasswordEncMethod"] == "DES")
            {
                var salt = GenerateSalt();
                var pwd = DESPassword(reg.Password, salt);
                return (Domain.User.PasswordEncryption.DES, pwd, salt);
            }
            if (config["PasswordEncMethod"] == "MD5")
            {
                var salt = GenerateSalt();
                var pwd = MD5Password(reg.Password, salt);
                return (Domain.User.PasswordEncryption.DES, pwd, salt);
            }
            return (Domain.User.PasswordEncryption.Plain, reg.Password, "");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginData loginForm)
        {
            //TODO: login process
            if (ModelState.IsValid)
            {
                //fetch user from db 
                var user =
                    await 
                        (from u in db.Users
                         where u.Login == loginForm.Login 
                         select u).FirstOrDefaultAsync();

                //TODO: in separate service - IPasswordValidator
                if (ValidatePassword(user, loginForm))
                {
                    //TODO: in separate cookie provider
                    //creating auth cookie
                    var claims =
                        new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Login),
                            new Claim("Id", user.Id.ToString()),
                            new Claim("Seller", "1"),
                        };
                    var identity = new ClaimsIdentity(claims, "S-Cookie");
                    await HttpContext.SignInAsync("S-Cookie", new ClaimsPrincipal(identity), 
                        new AuthenticationProperties { IsPersistent = loginForm.RememberMe });

                    if (String.IsNullOrEmpty(loginForm.Redirect))
                        return RedirectToAction("Index");
                    else
                        return Redirect(loginForm.Redirect);
                } else
                {
                    ModelState.AddModelError("Login", "Login/password pair is incorrect");
                }
            }
            return View();
        }

        public async Task<IActionResult> Logout(string r)
        {
            await HttpContext.SignOutAsync("S-Cookie");
            if (String.IsNullOrEmpty(r))
                return RedirectToAction("Index");
            else
                return Redirect(r);
        }

        [HttpGet]
        public IActionResult Register() => View(new RegisterData());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterData register)
        {
            if (ModelState.IsValid)
            {
                //check duplicates in db 
                var similar =
                    await 
                        (from u in db.Users
                         where u.Login == register.Login
                         select u).CountAsync();
                if (similar > 0)
                    ModelState.AddModelError("Login", "Login is being used by other user");
                var (alg, pwd, salt) = ComputePassword(register);
                await db.Users.AddAsync(new Domain.User { Login = register.Login, Password = pwd, Salt = salt, PasswordEncryptionAlgorithm = alg });
                await db.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            else
                return View();
        }
    }
}