using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BlogPlatform.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using NLog.Extensions.Logging;
using NLog.Web;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using BlogPlatform.Domain.Services;
using BlogPlatform.Domain.Services.Abstract;
using BlogPlatform.Mappings;
using BlogPlatform.Infrastructure.Cryptography;
using BlogPlatform.Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;
using BlogPlatform.Domain.Authentication.Requirements;
using System.Collections.Generic;

namespace BlogPlatform
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AutoMapperConfiguration.Configure();

            services.AddMemoryCache();

            services.AddDbContext<BlogPlatformContext>(options => options.UseSqlServer(Configuration.GetConnectionString("BlogPlatformConnection")));

            //services.AddScoped<AngularAntiForgeryTokenAttribute>();
            //services.AddAntiforgery(options =>
            //{
            //    options.HeaderName = "X-XSRF-TOKEN";
            //});

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IArticlesFilteringService, ArticlesFilteringService>();
            services.AddScoped<IArticleManagingService, ArticleManagingService>();
            services.AddScoped<ICommentsService, CommentsService>();
            services.AddScoped<IArticleRatingService, ArticleRatingService>();
            services.AddScoped<PasswordHasher, PasswordHasher>();

            services.AddSingleton<IAuthorizationHandler, ArticleOwnerHandler>();

            services.AddAuthentication(o =>
            {
                o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = new PathString("/login");
                options.LogoutPath = new PathString("/logout");
                options.SlidingExpiration = true;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Claims.ClaimsAuthorizedUserPolicyName, policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, Claims.ClaimsAutorizedRole);
                });

                options.AddPolicy(Claims.ClaimsArticleOwnerPolicyName, policy =>
                {
                    policy.Requirements.Add(new ArticleOwnerRequirement());
                });
            });

            // Add framework services.
            services.AddNodeServices();
            services.AddMvc();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //add NLog to .NET Core
            loggerFactory.AddNLog();

            //Enable ASP.NET Core features (NLog.web) - only needed for ASP.NET Core users
            app.AddNLogWeb();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    EnvironmentVariables = new Dictionary<string, string>()
                    {
                        {"NODE_ENV", "production"}
                    }
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseStaticFiles();


            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            DbInitializer.Initialize(app.ApplicationServices);
        }
    }
}
