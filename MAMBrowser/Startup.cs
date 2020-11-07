using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MAMBrowser.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using VueCliMiddleware;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using MAMBrowser.Processor;
using MAMBrowser.Controllers;
using MAMBrowser.Services;
using System.Linq;
using MAMBrowser.DAL;
using MAMBrowser.BLL;
using MAMBrowser.Middleware;
using Microsoft.Extensions.Options;

namespace MAMBrowser
{
    public delegate IFileService ServiceResolver(string key);
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
            StorageDISetting(services);
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.SchemaFilter<EnumSchemaFilter>();
            });

            services.AddControllers();
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp";
            });


            // configure strongly typed settings objects.  DI등록 안함.
            var optionSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(optionSection);
            services.AddScoped<IUserService, UserService>();

            var appSettings = optionSection.Get<AppSettings>();
            Repository.ConnectionString = appSettings.ConnectionString;
            MAMUtility.TokenIssuer = appSettings.TokenIssuer;
            MAMUtility.TokenSignature = appSettings.TokenSignature;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env, ILoggerFactory logFactory, IOptions<AppSettings> appSettings)
        {
            app.UseFileServer();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MAM Browser API V1");
            });

            logFactory.AddLog4Net();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseSpaStaticFiles();
            //app.UseAuthentication();
            //app.UseAuthorization();
            app.UseMiddleware<JwtMiddleware>();
            if (appSettings.Value.RequestLog)
            {
                app.UseMiddleware<RequestResponseLoggingMiddleware>();
            }
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                if (env.IsDevelopment())
                    spa.Options.SourcePath = "ClientApp";
                else
                    spa.Options.SourcePath = "dist";

                if (env.IsDevelopment())
                {
                    spa.UseVueCli(npmScript: "serve");
                }

            });
        }

        private void StorageDISetting(IServiceCollection services)
        {
            //DAL 등록
            services.AddTransient<APIDAL>();
            services.AddTransient<CategoriesDAL>();
            services.AddTransient<PrivateFileDAL>();
            services.AddTransient<ProductsDAL>();
            services.AddTransient<PublicFileDAL>();

            //서비스 등록
            services.AddTransient<LogService>();
            var storagesSection = Configuration.GetSection("Storages");
            var storage = storagesSection.Get<Storages>();

            services.AddTransient<IFileService, NetDriveService>(serviceProvider =>
            {
                return new NetDriveService
                {
                    Name = "MirosConnection",
                    Host = storage.PrivateWorkConnection["Host"].ToString(),
                };
            });
            services.AddTransient<MusicService>(serviceProvider =>
            {
                return new MusicService
                {
                    AuthorKey = storage.MusicConnection["AuthorKey"].ToString(),
                    MbcDomain = storage.MusicConnection["MbcDomain"].ToString(),
                };
            });
            services.AddTransient<IFileService, FtpService>(serviceProvider =>
            {
                return new FtpService
                {
                    Name = "PrivateWorkConnection",
                    Host = storage.PrivateWorkConnection["Host"].ToString(),
                    UserId = storage.PrivateWorkConnection["UserId"].ToString(),
                    UserPass = storage.PrivateWorkConnection["UserPass"].ToString(),
                    TmpUploadFolder = storage.PrivateWorkConnection["TmpUploadFolder"].ToString(),
                    UploadFolder = storage.PrivateWorkConnection["UploadFolder"].ToString(),
                };
            });
            services.AddTransient<IFileService, FtpService>(serviceProvider =>
            {
                return new FtpService
                {
                    Name = "PublicWorkConnection",
                    Host = storage.PublicWorkConnection["Host"].ToString(),
                    UserId = storage.PublicWorkConnection["UserId"].ToString(),
                    UserPass = storage.PublicWorkConnection["UserPass"].ToString(),
                    TmpUploadFolder = storage.PublicWorkConnection["TmpUploadFolder"].ToString(),
                    UploadFolder = storage.PublicWorkConnection["UploadFolder"].ToString(),
                };
            });
            services.AddTransient<IFileService, FtpService>(serviceProvider =>
            {
                return new FtpService
                {
                    Name = "DLArchiveConnection",
                    Host = storage.DLArchiveConnection["Host"].ToString(),
                    UserId = storage.DLArchiveConnection["UserId"].ToString(),
                    UserPass = storage.DLArchiveConnection["UserPass"].ToString()
                };
            });
            services.AddTransient<ServiceResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case "PrivateWorkConnection":
                        return serviceProvider.GetServices<IFileService>().First(impl => impl.Name == key);
                    case "PublicWorkConnection":
                        return serviceProvider.GetServices<IFileService>().First(impl => impl.Name == key);
                    case "MirosConnection":
                        return serviceProvider.GetServices<IFileService>().First(impl => impl.Name == key);
                    case "MusicConnection":
                        return serviceProvider.GetService<MusicService>();
                    case "DLArchiveConnection":
                        return serviceProvider.GetServices<IFileService>().First(impl => impl.Name == key);
                    default:
                        throw new Exception("KeyNotFoundException"); // or maybe return null, up to you
                }
            });
        }
    }
}
