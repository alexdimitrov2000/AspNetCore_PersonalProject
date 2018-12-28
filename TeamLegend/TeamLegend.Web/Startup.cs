﻿namespace TeamLegend.Web
{
    using Data;
    using Services;
    using TeamLegend.Models;
    using Services.Contracts;
    using Infrastructure.Extensions;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        private const string DefaultConnection = "DefaultConnection";

        private const string FacebookAppId = "Authentication:Facebook:AppId";
        private const string FacebookAppSecret = "Authentication:Facebook:AppSecret";

        private const string MicrosoftApplicationId = "Authentication:Microsoft:ApplicationId";
        private const string MicrosoftPassword = "Authentication:Microsoft:Password";

        private const string GoogleClientId = "Authentication:Google:ClientId";
        private const string GoogleClientSecret = "Authentication:Google:ClientSecret";

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

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString(DefaultConnection))
                    .UseLazyLoadingProxies());

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 2;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAutoMapper();

            services.AddAuthentication()
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = Configuration[FacebookAppId];
                    facebookOptions.AppSecret = Configuration[FacebookAppSecret];
                })
                .AddMicrosoftAccount(microsoftOptions =>
                {
                    microsoftOptions.ClientId = Configuration[MicrosoftApplicationId];
                    microsoftOptions.ClientSecret = Configuration[MicrosoftPassword];
                })
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = Configuration[GoogleClientId];
                    googleOptions.ClientSecret = Configuration[GoogleClientSecret];
                });

            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<ILeaguesService, LeaguesService>();
            services.AddScoped<IPlayersService, PlayersService>();
            services.AddScoped<IStadiumsService, StadiumsService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<ITeamsService, TeamsService>();
            services.AddScoped<IManagersService, ManagersService>();
            services.AddScoped<IFixturesService, FixturesService>();
            services.AddScoped<IMatchesService, MatchesService>();

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

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.SeedRoles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller}/{action}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
