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
                .AddCookie("S-Cookie", opt =>
                {
                    opt.Cookie.Name = "S-Cookie";
                    opt.Cookie.Path = "/Seller";
                    opt.LoginPath = "/Seller/Login";
                    opt.LogoutPath = "/Seller/Logout";
                    opt.ReturnUrlParameter = "r";
                    opt.AccessDeniedPath = "/Error/AccessDenied";
                    opt.ExpireTimeSpan = TimeSpan.FromDays(7);
                })
                .AddCookie("C-Cookie", opt =>
                {
                    opt.Cookie.Name = "C-Cookie";
                    opt.Cookie.Path = "/Client";
                    opt.LoginPath = "/Client/Login";
                    opt.LogoutPath = "/Client/Logout";
                    opt.ReturnUrlParameter = "r";
                    opt.AccessDeniedPath = "/Error/AccessDenied";
                    opt.ExpireTimeSpan = TimeSpan.FromDays(7);
                });
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("Seller", p => p.RequireClaim("Seller"));
                opts.AddPolicy("Client", p => p.RequireClaim("Client"));
            });
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
