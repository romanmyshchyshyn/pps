using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DataAccess;
using DataAccess.Models;
using TaskManagment.Middlewares;
using DataAccess.Interfaces;
using DataAccess.Implementation;
using Services.Interfaces;
using Services.Implementation;
using Microsoft.Extensions.FileProviders;
using System.IO;
using TaskManagment.NewFolder;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace TaskManagment
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<TmDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<User>(options => {
                options.Tokens.ProviderMap.Add("Default", new TokenProviderDescriptor(typeof(IUserTwoFactorTokenProvider<User>)));
            })
                .AddRoles<IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<TmDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICustomTaskService, CustomTaskService>();
            services.AddScoped<ICustomTaskStatusService, CustomTaskStatusService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITeamService, TeamService>();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddTransient<IEmailSender, EmailSender>(i =>
                new EmailSender(
                    Configuration["EmailSender:Host"],
                    Configuration.GetValue<int>("EmailSender:Port"),
                    Configuration.GetValue<bool>("EmailSender:EnableSSL"),
                    Configuration["EmailSender:UserName"],
                    Configuration["EmailSender:Password"]
                )
            );

            services.AddScoped<IUserClaimsPrincipalFactory<User>, ApplicationClaimsIdentityFactory>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseCors("MyPolicy");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            
            // app.ConfigureCustomExceptionMiddleware();

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
