using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Expire.io.Helpers;
using Expire.io.Models.Data;
using Expire.io.Models.Entities;
using Expire.io.Services;
using Expire.io.Services.Contracts;

namespace Expire.io
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
            services.AddDbContext<ExpireContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, Role>((opts) => {
                opts.Password.RequireDigit = false;
                opts.Password.RequiredLength = 8;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireNonAlphanumeric = false;
            })
                 .AddEntityFrameworkStores<ExpireContext>();
            services.AddTransient<Seed>();

            services.AddScoped<IAdminService, AdminService>();

            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IDocumentService, DocumentService>();

            services.AddScoped<IHomeService, HomeService>();

            services.AddScoped<IManagerService, ManagerService>();

            services.AddHttpContextAccessor();

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
            });
            services.AddMvc(opts=> 
            {
                opts.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Seed seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            seeder.SeedUsers();
           // seeder.CheckAndSend();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
