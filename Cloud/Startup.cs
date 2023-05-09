using MediaToolkit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MOROPO.SAPRESSC.Core.Persistence.EF.Extensions;
using MOROPO.SAPRESSC.Core.Persistence.Repositories;
using MOROPO.SAPRESSC.Core.Security.Common.Web.Frontend.Extensions;
using MOROPO.SAPRESSC.Middleware.Identity.Web.Common.Extensions;
using MOROPO.SAPRESSC.Middleware.Media.Web.Common.Extensions;
using MOROPO.SAPRESSC.Middleware.Security.Essential.Persistence.EF.Extensions;
using MOROPO.SAPRESSC.Middleware.Security.Essential.Web.Backend.Extensions;
using MOROPO.SAPRESSC.Middleware.Security.Essential.Web.Common.Extensions;
using Serilog;
using SmartSolucionesCuba.Viderhe.Cloud.Options;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Cloud
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

            this.configuration = configuration;
            this.env = env;

            #region Ensure Required Assemblies load

            var extensionRootDirectoryPath = configuration["Viderhe:ExtensionRootDirectoryPath"];

            Log.Logger.Information(string.Format(">>>>>>>>>>>>>>> extensionRootDirectoryPath: {0}", extensionRootDirectoryPath));

            var extensionRootDirectoryInfo = new System.IO.DirectoryInfo(extensionRootDirectoryPath);

            var extensionCustomRuntimeLibraries = Microsoft.Extensions.DependencyModel.DependencyContext.Default.RuntimeLibraries.Where(l => l.Type == "project" && l.Name.Contains(".Viderhe.Extend.") && !l.Name.Contains(".Viderhe.Support.Extend") && !l.Name.Contains(".Viderhe.Extend.Common")).ToList();

            if (extensionRootDirectoryInfo.Exists)
            {
                var extensionDirectoryInfos = extensionRootDirectoryInfo.GetDirectories();

                foreach (var extensionDirectoryInfo in extensionDirectoryInfos)
                {
                    Log.Logger.Information(string.Format(">>>>>>>>>>>>>>> extensionDirectoryInfo: {0}", extensionDirectoryInfo));

                    var extensionLibraryFileInfos = Providers.ViderheExtensionLibraryFileInfosProvider.GetRelevantToCloud(extensionDirectoryInfo);

                    foreach (var extensionLibraryFileInfo in extensionLibraryFileInfos)
                    {
                        Log.Logger.Information(string.Format(">>>>>>>>>>>>>>> extensionLibraryFileInfo: {0}", extensionLibraryFileInfo));

                        var assembly = System.Reflection.Assembly.LoadFrom(extensionLibraryFileInfo.FullName);

                        var assemblyName = assembly.GetName();

                        extensionCustomRuntimeLibraries.Add(new Microsoft.Extensions.DependencyModel.RuntimeLibrary(
                            "project",
                            assemblyName.Name,
                            assemblyName.Version.ToString(),
                            null,
                            new Microsoft.Extensions.DependencyModel.RuntimeAssetGroup[] { },
                            new Microsoft.Extensions.DependencyModel.RuntimeAssetGroup[] { },
                            Enumerable.Empty<Microsoft.Extensions.DependencyModel.ResourceAssembly>(),
                            Enumerable.Empty<Microsoft.Extensions.DependencyModel.Dependency>(),
                            false));
                    }
                }
            }

            foreach (var runtimeLibrary in extensionCustomRuntimeLibraries)
            {
                System.AppDomain.CurrentDomain.Load(runtimeLibrary.Name);

                Log.Logger.Information(string.Format(">>>>>>>>>>>>>>> runtimeLibrary Loaded: {0}", runtimeLibrary.Name));
            }

            #endregion
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MediaThumbnailResizeOptions>(configuration.GetSection("MediaThumbnailResize"));
            services.Configure<MediaPathOptions>(configuration.GetSection("MediaPath"));
            services.Configure<JwtOptions>(configuration.GetSection("JWT"));
            //services.Configure<FfmpegOptions>(configuration.GetSection("FFmpeg"));

            //services.AddDbContext<Persistence.EF.ApplicationDbContext>(options => options.UseMySql(configuration.GetConnectionString("DefaultConnection"), mySqlOptions =>
            //{
            //    mySqlOptions.ServerVersion(new System.Version(configuration.GetSection("MySQLSeverVersion").Value), Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql);
            //}));
            services.AddDbContext<Persistence.EF.ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<DbContext, Persistence.EF.ApplicationDbContext>();

            services.PerformCoreWebSecurityCommonFrontendServiceRegistration();
            services.PerformMiddlewareSecurityEssentialWebCommonServiceRegistration();
            services.PerformMiddlewareSecurityEssentialWebBackendServiceRegistration();
            services.PerformMiddlewareMediaWebCommonServiceRegistration();
            services.PerformMiddlewareIdentityWebCommonServiceRegistration();

            services.AddScoped<IEntityRepository<Persistence.Entities.Account, System.Guid>, Persistence.EF.Repositories.AccountRepository>();
            services.AddScoped<IEntityRepository<Persistence.Entities.Frame, long>, Persistence.EF.Repositories.FrameRepository>();
            services.AddScoped<IEntityRepository<Persistence.Entities.Placard, System.Guid>, Persistence.EF.Repositories.PlacardRepository>();
            services.AddScoped<IEntityRepository<Persistence.Entities.Player, string>, Persistence.EF.Repositories.PlayerRepository>();
            services.AddScoped<IEntityRepository<Persistence.Entities.Screenplay, System.Guid>, Persistence.EF.Repositories.ScreenplayRepository>();
            services.AddScoped<IEntityRepository<Persistence.Entities.Moderator, System.Guid>, Persistence.EF.Repositories.ModeratorRepository>();
            services.AddScoped<IEntityRepository<Persistence.Entities.Extension, System.Guid>, Persistence.EF.Repositories.ExtensionRespository>();
            services.AddScoped<IEntityRepository<Persistence.Entities.Template, System.Guid>, Persistence.EF.Repositories.TemplateRepository>();
            services.AddScoped<IEntityRepository<Persistence.Entities.ScheduledRelaunch, System.Guid>, Persistence.EF.Repositories.ScheduledRelaunchRepository>();

            services.AddSingleton<MOROPO.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade.IImageFileManagerFacade, Managers.ImageFileManager>();
            services.AddSingleton<MOROPO.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade.IThumbnailFileManagerFacade, Managers.ThumbnailFileManager>();
            services.AddSingleton<MOROPO.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade.IVideoFileManagerFacade, Managers.VideoFileManager>();

            services.AddMediaToolkit(configuration["FFmpeg:MainBinaryPath"]);

            services.AddSingleton<MOROPO.SAPRESSC.Core.Persistence.EF.Providers.Facade.IDbSchemaConfigurationProviderFacade, Persistence.EF.Providers.BusinessDbSchemaConfigurationProvider>();

            services.AddSingleton<MOROPO.SAPRESSC.Core.Persistence.EF.Helpers.Facade.IDataInitializationHelper, Helpers.BusinessDataInitializationHelper>();

            services.AddScoped<Microsoft.AspNetCore.Authorization.IAuthorizationHandler, Security.Authorization.Requirement.Handler.AccountModerationRequirementHandler>();

            services.AddAuthentication(config =>
            {
                config.DefaultScheme = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(config =>
            {
                config.AccessDeniedPath = new PathString("/security/error/forbidden");
                config.LoginPath = new PathString("/security/account/login");
            })
            .AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Issuer"],
                    ValidateLifetime = false,
                    RequireExpirationTime = false,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
                    ClockSkew = System.TimeSpan.Zero
                };
                config.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/dashboard"))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }

                        return System.Threading.Tasks.Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, policy =>
                {
                    policy.AddAuthenticationSchemes(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim(System.Security.Claims.ClaimTypes.NameIdentifier);
                });
                options.AddPolicy(Security.Authorization.Policies.ACCOUNT_MODERATION, policy =>
                {
                    policy.AddAuthenticationSchemes(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme);
                    policy.AddRequirements(new Security.Authorization.Requirement.AccountModerationRequirement());
                });
                options.AddPolicy(Security.Authorization.Policies.ACCOUNT_SANDBOXED, policy =>
                {
                    policy.AddAuthenticationSchemes(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme);
                    policy.AddRequirements(new Security.Authorization.Requirement.AccountSandboxedRequirement());
                });
            });

            #region SAPRESSC.Core.Security concreter dependencies configuration
            services.AddSingleton<MOROPO.SAPRESSC.Core.Web.Security.Helpers.Facade.ILicenseValidationHelperFacade, Security.Licensing.WilcardLicenseValidator>();

            services.AddTransient<MOROPO.SAPRESSC.Core.Web.Security.Models.View.Contrat.IUserProfileViewModel, Models.View.UserProfileViewModel>();

            services.AddSingleton<MOROPO.SAPRESSC.Core.Web.Common.ModelBinding.IConcretModelBindHelper, Helpers.Binding.ConcretModelBindHelper>();

            services.AddScoped<MOROPO.SAPRESSC.Core.Web.Security.Providers.Facade.IIdentityProviderFacade, Managers.IdentityManager>();
            services.AddScoped<MOROPO.SAPRESSC.Core.Web.Security.Managers.Facade.IUserProfileManagerFacade, Managers.UserProfileManager>();
            #endregion

            #region Globalization configuration
            var defaultCulture = new System.Globalization.CultureInfo("es");

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(defaultCulture);
                options.SupportedCultures = new[] { defaultCulture };
                options.SupportedUICultures = new[] { defaultCulture };

                options.RequestCultureProviders = new[] { new MOROPO.SAPRESSC.Core.Web.Common.Localization.UrlRequestCultureProvider() };
            });

            services.AddSingleton<Helpers.Localization.GlobalViewLocalizationHelper>();
            #endregion

            services.Configure<Microsoft.AspNetCore.Routing.RouteOptions>(options => options.LowercaseUrls = true);

            services.AddResponseCaching();
            services.AddMemoryCache();
            services.AddSession();

            services.AddSignalR();

            services.AddMvc(mvc =>
                mvc.ModelBinderProviders.Insert(0, new MOROPO.SAPRESSC.Core.Web.Common.ModelBinding.AbstractsModelBinderProvider())
            )
            .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization(options =>
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    return factory.Create(typeof(SharedResources));
                }
            )
            .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_0);

            services.AddDataProtection()
                .PersistKeysToFileSystem(new System.IO.DirectoryInfo(System.IO.Path.Combine(env.ContentRootPath, "dpkeys")));

            services.AddSingleton<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider, Microsoft.AspNetCore.Mvc.ViewFeatures.CookieTempDataProvider>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Microsoft.Extensions.Options.IOptions<RequestLocalizationOptions> requestLocalizationOptions, Microsoft.Extensions.Logging.ILoggerFactory loggerFactory, DbContext dbContext)
        {
            //FFMediaToolkit.FFmpegLoader.FFmpegPath = ffmpegOptions.Value.BinariesPath;

            loggerFactory.AddSerilog(Log.Logger);

            app.UseRequestLocalization(requestLocalizationOptions.Value);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else if (env.IsStaging())
            {
                dbContext.Database.Migrate();

                app.PerformDatabaseInitialization();

                if (!(dbContext as Persistence.EF.ApplicationDbContext).Users.Any())
                {
                    var defaultUserId = configuration["DefaultUser:Id"];

                    var k3y = new System.Guid(0x6abae711, 0xcd91, 0x47e7, 0xad, 0x8a, 0xae, 0x88, 0xe4, 0x50, 0x25, 0x4d);

                    var protectedPassword = (dbContext as Persistence.EF.ApplicationDbContext).CreateSecureUser(k3y, defaultUserId, "system");

                    Log.Logger.Information(string.Format(">>>>>> Protected Password : \"{0}\" => \"{1}\"", defaultUserId, protectedPassword));
                }
            }
            else
            {
                app.UseExceptionHandler("/error");

                app.UseHsts();
            }

            var contentTypePovider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();

            contentTypePovider.Mappings.Add(".apk", "application/vnd.android.package-archive");

            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = contentTypePovider
            });

            app.UseAuthentication();
            app.UseSession();

            app.UseSignalR(endpoints =>
            {
                endpoints.MapHub<Hubs.DashboardHub>("/hubs/dashboard");
                endpoints.MapHub<Hubs.PlayerHub>("/hubs/player");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute("defaultLocalizedAreaRoutes",
                    template: "{culture:regex(^[[a-z]]{{2}}(?:-[[A-Z]]{{2}})?$)}/{area:exists}/{controller=start}/{action=index}/{id?}");

                routes.MapRoute(
                    name: "defaultLocalizedRoutes",
                    template: "{culture:regex(^[[a-z]]{{2}}(?:-[[A-Z]]{{2}})?$)}/{controller=start}/{action=index}/{id?}");

                routes.MapRoute("defaultAreaRoutes",
                    template: "{area:exists}/{controller=start}/{action=index}/{id?}");

                routes.MapRoute(
                    name: "defaultRoutes",
                    template: "{controller=start}/{action=index}/{id?}");
            });
        }
    }
}
