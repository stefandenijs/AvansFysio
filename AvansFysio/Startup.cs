using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Core.DomainServices;
using Core.DomainServices.Authorization;
using Core.DomainServices.Utility;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Webservice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AvansFysio
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
            services.AddDbContext<PhysioDbContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("Default")));

            services.AddDbContext<SecurityDbContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("Security")));
            services.AddIdentity<IdentityUser, IdentityRole>(config =>
                {
                    config.Password.RequiredLength = 8;
                    config.Password.RequireDigit = true;
                    config.Password.RequireUppercase = false;
                    config.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<SecurityDbContext>()
                .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Identity.Cookie";
                config.LoginPath = "/Login";
            });

            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IWorkerService, WorkerService>();
            services.AddScoped<IAvailabilityService, AvailabilityService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ITreatmentService, TreatmentService>();
            services.AddScoped<IRoomService, RoomsService>();
            services.AddScoped<IAgeHelper, AgeHelper>();
            services.AddScoped<IAppointmentPlanner, AppointmentPlanner>();

            services.AddScoped<IDiagnosisServiceHttp, DiagnosisServiceHttp>();
            services.AddScoped<ITreatmentInfoServiceHttp, TreatmentInfoServiceHttp>();

            services.AddControllersWithViews();
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireWorker",
                    policy => policy.Requirements.Add(new WorkerEntryRequirement()));
                options.AddPolicy("RequirePhysio",
                    policy => policy.RequireClaim("Claim.Physiotherapist"));
                options.AddPolicy("RequireIntern",
                    policy => policy.RequireClaim("Claim.Intern"));
            });

            services.AddScoped<IAuthorizationHandler, WorkerEntryHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
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
            
            app.UseStatusCodePages();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

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
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "slug",
                    pattern: "Patients",
                    defaults: new { controller = "Patient", action = "Patients"});
                endpoints.MapControllerRoute(
                    name: "slug",
                    pattern: "Login",
                    defaults: new { controller = "Authentication", action = "Login" });
                endpoints.MapControllerRoute(
                    name: "slug",
                    pattern: "Register",
                    defaults: new { controller = "Authentication", action = "Register" });
            });
        }
    }
}
