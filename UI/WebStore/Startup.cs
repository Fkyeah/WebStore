using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Convensions;
using WebStore.Infrastructure.Middleware;
using WebStore.Interfaces.Services;
using WebStore.Interfaces.TestAPI;
using WebStore.Services.Data;
using WebStore.Services.InCookies;
using WebStore.Services.InMemory;
using WebStore.Services.InSQL;
using WebStore.WebAPI.Clients.Employers;
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
            var database_type = Configuration["Database"];

            switch (database_type)
            {
                default: throw new InvalidOperationException($"Тип БД {database_type} не поддерживается");

                case "SqlServer":
                    services.AddDbContext<WebStoreDB>(opt =>
                        opt.UseSqlServer(Configuration.GetConnectionString(database_type)));
                    break;

                case "Sqlite":
                    services.AddDbContext<WebStoreDB>(opt =>
                        opt.UseSqlite(Configuration.GetConnectionString(database_type),
                            o => o.MigrationsAssembly("WebStore.DAL.Sqlite")));
                    break;

                case "InMemory":
                    services.AddDbContext<WebStoreDB>(opt => opt.UseInMemoryDatabase("TikhonovWebStore.db"));
                    break;
            }

            services.AddTransient<WebStoreDBInitializer>();
            
            services.AddScoped<IProductData, SqlProductData>();
            services.AddScoped<ICartService, InCookiesCartService>();
            services.AddScoped<IOrderService, SqlOrderService>();

            services.AddHttpClient("WebStore.WebAPI", client => client.BaseAddress = new(Configuration["WebAPI"]))
                .AddTypedClient<IEmployersData, EmployersClient>()
                .AddTypedClient<IValueClient, ValuesClient>();

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<WebStoreDB>()
                .AddDefaultTokenProviders();

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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
