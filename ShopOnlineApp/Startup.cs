using System;
using System.Collections.Generic;
using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using ShopOnline.Application.Dapper.Implements;
using ShopOnline.Application.Dapper.Interfaces;
using ShopOnlineApp.Application.Implementation;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Authorization;
using ShopOnlineApp.Data.EF;
using ShopOnlineApp.Data.EF.Repositories;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Helper;
using ShopOnlineApp.Infrastructure.Interfaces;
using ShopOnlineApp.Models;
using ShopOnlineApp.Services;
using ShopOnlineApp.SignalR;

namespace ShopOnlineApp
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
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                o=>o.MigrationsAssembly("ShopOnlineApp.Data.EF")));

            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            services.Configure<CloudinaryImage>(Configuration.GetSection("CloudinarySettings"));

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true;
            });

            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.AddAuthentication()
                .AddFacebook(facebookOpts =>
                {
                    facebookOpts.AppId = Configuration["Authentication:Facebook:AppId"];
                    facebookOpts.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                })
                .AddGoogle(googleOpts => {
                    googleOpts.ClientId = Configuration["Authentication:Google:ClientId"];
                    googleOpts.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                });

            services.AddRecaptcha(new RecaptchaOptions
            {
                SiteKey = Configuration["Recaptcha:SiteKey"],
                SecretKey = Configuration["Recaptcha:SecretKey"]
            });

            services.AddAutoMapper();
            services.AddMemoryCache();
            // Add application services.
            services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>();

            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<DbInitializer>();

            services.AddMvc(options =>
                {
                    options.CacheProfiles.Add("Default",
                        new CacheProfile
                        {
                            Duration = 60
                        });
                    options.CacheProfiles.Add("Never",
                        new CacheProfile
                        {
                            Location = ResponseCacheLocation.None,
                            NoStore = true
                        });
                }).AddViewLocalization(
                    LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = "Resources"; })
                .AddDataAnnotationsLocalization()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

            services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en-US"),
                        new CultureInfo("vi-VN")
                    };

                    opts.DefaultRequestCulture = new RequestCulture("en-US");
                    // Formatting numbers, dates, etc.
                    opts.SupportedCultures = supportedCultures;
                    // UI strings that we have localized.
                    opts.SupportedUICultures = supportedCultures;
                });
            //add config system
            services.AddTransient(typeof(IUnitOfWork),typeof(EFUnitOfWork));
            services.AddTransient(typeof(IRepository<,>), typeof(EFRepository<,>));
            //end config

            //repository
            services.AddTransient<IProductCategoryRepository,ProductCategoryRepository>();
            services.AddTransient<IFunctionRepository, FunctionRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<IProductQuantityRepository, ProductQuantityRepository>();
            
            services.AddTransient<IProductTagRepository, ProductTagRepository>();
            services.AddTransient<IPermissionRepository,PermissionRepository>();
            services.AddTransient<IBusinessRepository, BusinessRepository>();
            services.AddTransient<IBusinessActionRepository, BusinessActionRepository>();
            services.AddTransient<IBillRepository, BillRepository>();
            services.AddTransient<IBillDetailRepository, BillDetailRepository>();
            services.AddTransient<IColorRepository, ColorRepository>();
            services.AddTransient<ISizeRepository, SizeRepository>();
            services.AddTransient<IProductImageRepository, ProductImageRepository>();
            services.AddTransient<IWholePriceRepository, WholePriceRepository>();
            services.AddTransient<IGrantPermissionRepository, GrantPermissionRepository>();
            services.AddTransient<IBlogRepository, BlogRepository>();
            services.AddTransient<IBlogTagRepository, BlogTagRepository>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<ISystemConfigRepository, SystemConfigRepository>();
            services.AddTransient<ISlideRepository, SlideRepository>();
            services.AddTransient<IFooterRepository, FooterRepository>();
            services.AddTransient<IFeedbackRepository, FeedbackRepository>();
            services.AddTransient<IContactRepository, ContactRepository>();
            services.AddTransient<IPageRepository, PageRepository>();
            services.AddTransient<IRatingRepository, RatingRepository>();
            services.AddTransient<IBlogCategoryRepository, BlogCategoryRepository>();
            services.AddTransient<IBlogCommentRepository, BlogCommentRepository>();
            services.AddTransient<IAnnouncementService, AnnouncementService>();

            //service
            services.AddTransient<IFunctionService, FunctionService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProductCategoryService, ProductCategoryService>();
            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, ShopOnlineClaimsPrincipalFactory>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IBusinessService, BusinessService>();
            services.AddTransient<IBusinessActionService, BusinessActionService>();
            services.AddTransient<IBillService, BillService>();
            services.AddTransient<IGrantPermissionService, GrantPermissionService>();
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<ICommonService, CommonService>();
            services.AddTransient<IViewRenderService, ViewRenderService>();
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<IFeedbackService, FeedbackService>();
            services.AddTransient<IPageService, PageService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IRatingService, RatingService>();
            services.AddTransient<IColorService, ColorService>();
            services.AddTransient<ISizeService, SizeService>();
            services.AddTransient<IBlogCategoryService, BlogCategoryService>();
            services.AddTransient<IBlogCommentService, BlogCommentService>();
            services.AddTransient<ISlideService, SlideService>();
            services.ConfigureApplicationCookie(options => options.LoginPath = "/admin-login");
            //services.Configure<IdentityOptions>(opt =>
            //{
            //    opt.Cookies.ApplicationCookie.LoginPath = new PathString("/login");
            //});

            //Config system
            services.AddMvc();

            services.AddCors(options => options.AddPolicy("CorsPolicy", builder => { builder.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:3000").AllowCredentials(); }));

            services.AddTransient<IAuthorizationHandler, BaseResourceAuthorizationHandler>();

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory logger)
        {
            logger.AddFile("Logs/shoponline-{Date}.txt");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSession();
            app.UseStaticFiles();

            app.UseAuthentication();

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "areaRoute", 
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(name: "login",
                    template: "{area:exists}/{controller=Login}/{action=Index}/{id?}");
            });
            app.UseCookiePolicy();
            app.UseCors("CorsPolicy");
            app.UseSignalR(routes => { routes.MapHub<OnlineShopHub>("/onlineShopHub"); });

        }
    }
}
