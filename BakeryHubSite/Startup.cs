using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using BakeryHub.Services;

namespace BakeryHub
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection()
                .SetApplicationName("BakeryHub");
                //.TODO: persist key in Azure of shared vault 
                //.PersistKeysToFileSystem(new DirectoryInfo(@"\\server\share\directory\"))
            services.AddDbContext<BakeryHub.Domain.BakeryHubContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddAuthentication()
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
                {
                    opt.Cookie.Name = "Auth";
                    opt.LoginPath = "/Account/Login";
                    opt.LogoutPath = "/Account/Logout";
                    opt.ReturnUrlParameter = "r";
                    opt.AccessDeniedPath = "/Error/AccessDenied";
                    opt.ExpireTimeSpan = TimeSpan.FromDays(7);
                    opt.Events.OnRedirectToAccessDenied =
                        rc =>
                        {
                            rc.RedirectUri = "/Seller/Register";
                            return Task.CompletedTask;
                        };
                });
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("Seller", p => p.RequireClaim("Seller"));
            });
            services.AddTransient<IEmailService, GmailEmailService>();
            services.AddTransient<ISMSService, AmazonSNSService>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            //app.Use(async (context, next) =>
            //    {

            //        await next.Invoke();
            //    }
            //);

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
