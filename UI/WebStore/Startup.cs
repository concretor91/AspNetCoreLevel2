using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Net.Http;
using WebStore.Clients.Employees;
using WebStore.Clients.Identity;
using WebStore.Clients.Orders;
using WebStore.Clients.Products;
using WebStore.Clients.Services.Values;
using WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.AutoMapperPropfiles;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;
using WebStore.Services.Products.InCookies;

namespace WebStore
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        private Action<HttpClient, string> t = (client, address) =>
        {
            client.BaseAddress = new Uri(address);
        };

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ViewModelsMapping>();
            }, typeof(Startup));

            services.AddIdentity<User, Role>()
               .AddDefaultTokenProviders();

            services
   .AddHttpClient<IUserStore<User>, UsersClient>(client => client.BaseAddress = new Uri(Configuration["ClientAdress"]));
            services.AddHttpClient<IUserPasswordStore<User>, UsersClient>(client => client.BaseAddress = new Uri(Configuration["ClientAdress"]));
            services.AddHttpClient<IUserEmailStore<User>, UsersClient>(client => client.BaseAddress = new Uri(Configuration["ClientAdress"]));
            services.AddHttpClient<IUserPhoneNumberStore<User>, UsersClient>(client => client.BaseAddress = new Uri(Configuration["ClientAdress"]));
            services.AddHttpClient<IUserTwoFactorStore<User>, UsersClient>(client => client.BaseAddress = new Uri(Configuration["ClientAdress"]));
            services.AddHttpClient<IUserClaimStore<User>, UsersClient>(client => client.BaseAddress = new Uri(Configuration["ClientAdress"]));
            services.AddHttpClient<IUserLoginStore<User>, UsersClient>(client => client.BaseAddress = new Uri(Configuration["ClientAdress"]));
            services
               .AddHttpClient<IRoleStore<Role>, RolesClient>(client => client.BaseAddress = new Uri(Configuration["ClientAdress"]));

            services.Configure<IdentityOptions>(opt =>
            {
#if DEBUG
                opt.Password.RequiredLength = 3;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredUniqueChars = 3;

                //opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCD1234567890";
                opt.User.RequireUniqueEmail = false;
#endif

                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.MaxFailedAccessAttempts = 10;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "WebStore";
                opt.Cookie.HttpOnly = true;
                opt.ExpireTimeSpan = TimeSpan.FromDays(10);

                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenied";

                opt.SlidingExpiration = true;
            });

            services.AddControllersWithViews()
               .AddRazorRuntimeCompilation();

            services.AddScoped<ICartService, CookiesCartService>();
            services.AddHttpClient<IValuesService, ValuesClient>(client => client.BaseAddress = new Uri(Configuration["ClientAdress"]));
            services.AddHttpClient<IEmployeesData, EmployeesClient>(client => client.BaseAddress = new Uri(Configuration["ClientAdress"]));
            services.AddHttpClient<IOrderService, OrdersClient>(client => client.BaseAddress = new Uri(Configuration["ClientAdress"]));
            services.AddHttpClient<IProductData, ProductsClient>(client => client.BaseAddress = new Uri(Configuration["ClientAdress"]));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseStaticFiles();
            app.UseDefaultFiles();

            app.UseWelcomePage("/MVC");
            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );

                endpoints.MapControllerRoute(
                     name: "default",
                     pattern: "{controller=Home}/{action=Index}/{id?}"
                     );
            });
        }
    }
}
