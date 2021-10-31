using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Convensions;
using WebStore.Infrastructure.Middleware;
using WebStore.Interfaces.Services;
using WebStore.Interfaces.TestAPI;
using WebStore.Logger;
using WebStore.Services;
using WebStore.Services.Data;
using WebStore.Services.InCookies;
using WebStore.Services.InMemory;
using WebStore.Services.InSQL;
using WebStore.WebAPI.Clients.Employers;
using WebStore.WebAPI.Clients.Identity;
using WebStore.WebAPI.Clients.Orders;
using WebStore.WebAPI.Clients.Products;
using WebStore.WebAPI.Clients.Values;

namespace WebStore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddScoped<ICartService, InCookiesCartService>();
            services.AddScoped<ICartStore, InCookiesCartStore>();
            services.AddScoped<ICartService, CartService>();
            
            services.AddHttpClient("WebStore.WebAPI", client => client.BaseAddress = new(Configuration["WebAPI"]))
                .AddTypedClient<IEmployersData, EmployersClient>()
                .AddTypedClient<IValueClient, ValuesClient>()
                .AddTypedClient<IProductData, ProductsClient>()
                .AddTypedClient<IOrderService, OrdersClient>();

            services.AddIdentity<User, Role>()
                .AddDefaultTokenProviders();

            services.AddHttpClient("WebStoreWebAPIIdentity", client => client.BaseAddress = new(Configuration["WebAPI"]))
               .AddTypedClient<IUserStore<User>, UsersClient>()
               .AddTypedClient<IUserRoleStore<User>, UsersClient>()
               .AddTypedClient<IUserPasswordStore<User>, UsersClient>()
               .AddTypedClient<IUserEmailStore<User>, UsersClient>()
               .AddTypedClient<IUserPhoneNumberStore<User>, UsersClient>()
               .AddTypedClient<IUserTwoFactorStore<User>, UsersClient>()
               .AddTypedClient<IUserClaimStore<User>, UsersClient>()
               .AddTypedClient<IUserLoginStore<User>, UsersClient>()
               .AddTypedClient<IRoleStore<Role>, RolesClient>()
                ;

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                options.User.RequireUniqueEmail = false;

                options.Lockout.AllowedForNewUsers = false;
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "GB.WebStore";
                options.Cookie.HttpOnly = true;

                options.ExpireTimeSpan = TimeSpan.FromDays(20);


                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;

            });
            
            services.AddControllersWithViews(opt => opt.Conventions.Add(new TestConvention()))
                .AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory log)
        {
            log.AddLog4Net();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<TestMiddleware>();
            
            var greeting = "Greeting";
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/hello", async context =>
                {
                    await context.Response.WriteAsync(greeting);
                });

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                      name: "areas",
                      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );
                });

                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}"
                    );
            });
        }
    }
}
