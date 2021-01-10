using System;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PinBackendSystem.Areas.Identity;
using PinBackendSystem.Data;

namespace PinBackendSystem
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
            //services.AddAuthentication(AzureADB2CDefaults.AuthenticationScheme)
            //    .AddAzureADB2C(options => Configuration.Bind("AzureAdB2C", options));


            services.AddDbContext<PinContext>(options =>
                // options.UseSqlite(
                options.UseNpgsql(
                    Configuration.GetConnectionString("PinContext")));
            //options.UseInMemoryDatabase(databaseName: "PinContext"));
            services.AddIdentity<PinrumahUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<PinContext>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRole", policy =>
                   policy.RequireRole("admin"));
                options.AddPolicy("RequireMarketingRole", policy =>
                   policy.RequireRole("admin","marketing"));
                options.AddPolicy("RequireSurveyorTeleAdsRole", policy =>
                   policy.RequireRole("admin", "surveyor", "tele", "ads"));
                options.AddPolicy("RequireSurveyorRole", policy =>
                   policy.RequireRole("admin", "surveyor"));
                options.AddPolicy("RequireTeleRole", policy =>
                   policy.RequireRole("admin", "tele"));
                options.AddPolicy("RequireAdsRole", policy =>
                   policy.RequireRole("admin", "ads"));
            });

            services.AddControllersWithViews();

            services.AddRazorPages(options =>
            {
                //options.Conventions.AuthorizeAreaPage("Admin","/Listings/Index", "RequireMarketingRole");
                //options.Conventions.AuthorizeAreaPage("Admin", "/Listings/Details", "RequireAdsRole");
                //options.Conventions.AuthorizeAreaFolder("Identity", "/Manage");
                //options.Conventions.AuthorizeAreaFolder("Admin", "/");

            });

            services.AddSingleton<IPasswordHasher<PinrumahUser>, PasswordHasherWithOldMembershipSupport>();

            services.Configure<IdentityOptions>(options =>
            {
                // Default SignIn settings.
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedAccount = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });



            //services.AddDbContext<PinContext>(options =>
            //options.UseNpgsql(Configuration.GetConnectionString("PinContext")));
            //options.UseInMemoryDatabase(databaseName: "PinContext"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var cultureInfo = new CultureInfo("id-ID");
            cultureInfo.NumberFormat.CurrencySymbol = "Rp";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                //endpoints.MapAreaControllerRoute(
                //    name: "areaRoute",
                //    areaName: "Admin",
                //    pattern: "Admin/{controller=Listings}/{action=Personal}/{id?}");

                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Listings}/{action=Index}/{id?}");
                //endpoints.MapRazorPages();
                endpoints.MapAreaControllerRoute(
                    name: "areaRoute",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Listings}/{action=Personal}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Listings}/{action=Home}/{id?}");
                endpoints.MapRazorPages();
            });

        }
    }
}
