using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BakeryHub.Models;
using System.Security.Claims;
using BakeryHub.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.IO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BakeryHubSite.Controllers
{
    public class AccountController : Controller
    {
        BakeryHubContext db;
        IConfiguration config;
        public AccountController(BakeryHubContext context, IConfiguration configuration) =>
            (db, config) = (context, configuration);

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
                    using (var memory = new MemoryStream())
                    {
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
            if (dbUser.PasswordEncryptionAlgorithm == BakeryHub.Domain.User.PasswordEncryption.Plain
                && dbUser.Password == login.Password)
                return true;
            if (dbUser.PasswordEncryptionAlgorithm == BakeryHub.Domain.User.PasswordEncryption.MD5
                && dbUser.Password == MD5Password(login.Password, dbUser.Salt))
                return true;
            if (dbUser.PasswordEncryptionAlgorithm == BakeryHub.Domain.User.PasswordEncryption.DES
                && dbUser.Password == DESPassword(login.Password, dbUser.Salt))
                return true;
            return false;
        }

        private string GenerateSalt()
        {
            return Guid.NewGuid().ToString();
        }

        private (BakeryHub.Domain.User.PasswordEncryption, string, string) ComputePassword(RegisterData reg)
        {
            if (config["PasswordEncMethod"] == "DES")
            {
                var salt = GenerateSalt();
                var pwd = DESPassword(reg.Password, salt);
                return (BakeryHub.Domain.User.PasswordEncryption.DES, pwd, salt);
            }
            if (config["PasswordEncMethod"] == "MD5")
            {
                var salt = GenerateSalt();
                var pwd = MD5Password(reg.Password, salt);
                return (BakeryHub.Domain.User.PasswordEncryption.DES, pwd, salt);
            }
            return (BakeryHub.Domain.User.PasswordEncryption.Plain, reg.Password, "");
        }

        [HttpGet]
        public IActionResult Login(string r) => View(new LoginData { Redirect = r });

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
                        (from u in 
                            db.Users
                                .Include(u => u.DeliverySite)
                                .Include(u => u.Supplier)
                         where u.Login == loginForm.Login
                         select u).FirstOrDefaultAsync();

                //TODO: in separate service - IPasswordValidator
                if ((user != null) && ValidatePassword(user, loginForm))
                {
                    //TODO: in separate cookie provider
                    //creating auth cookie
                    var claims =
                        new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Login),
                            new Claim("Id", user.Id.ToString()),
                            new Claim("Persistent", loginForm.RememberMe ? "1" : "0"),
                        };
                    if (user.Supplier != null)
                        claims.Add(new Claim("Seller",  "1"));
                    if (user.DeliverySite != null)
                        claims.Add(new Claim("DeliverySite", "1"));
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity),
                        new AuthenticationProperties { IsPersistent = loginForm.RememberMe });

                    if (String.IsNullOrEmpty(loginForm.Redirect))
                    {
                        if (user.Supplier != null)
                            return RedirectToAction("Index", "Seller");
                        else
                            return RedirectToAction("Register", "Seller");
                    }
                    else
                        return Redirect(loginForm.Redirect);
                }
                else
                {
                    ModelState.AddModelError("Login", "Login/password pair is incorrect");
                }
            }
            return View();
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
                var user = new User
                {
                    Login = register.Login,
                    Password = pwd,
                    Salt = salt,
                    PasswordEncryptionAlgorithm = alg,
                    Session = new Session
                    {
                        FirstVisit = DateTime.UtcNow,
                        LastVisit = DateTime.UtcNow,
                        IP = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        UserAgent = Request.Headers["User-Agent"]
                    }
                };
                await db.Users.AddAsync(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            else
                return View();
        }

        public async Task<IActionResult> Logout(string r)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (String.IsNullOrEmpty(r))
                return RedirectToAction("Index", "Home");
            else
                return Redirect(r);
        }

    }
}