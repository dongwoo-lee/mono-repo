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
using MAMBrowser.Services;
using System.Linq;
using MAMBrowser.DAL;
using MAMBrowser.BLL;
using MAMBrowser.Middleware;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using MAMBrowser.Foundation;
using MAMBrowser.Common.Foundation;
using MAMBrowser.Helper;

namespace MAMBrowser
{
    public delegate IFileProtocol ServiceResolver(string key);
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public  IConfiguration Configuration { get; }
        public static AppSettings AppSetting { get; set; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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



            StorageFactorySetting(services);
            DISetting(services);

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    TokenGenerator.IPAddress = ip.ToString();
                    break;
                }
            }
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
            app.UseMiddleware<JwtMiddleware>();
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
       
        private void DISetting(IServiceCollection services)
        { 
            //옵션 DI
            var optionSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(optionSection);
            
            //글로벌 셋팅
            AppSetting = optionSection.Get<AppSettings>();
            Repository.ConnectionString = AppSetting.ConnectionString;
            TokenGenerator.ExpireHour = AppSetting.ExpireMusicTokenHour;
            TokenGenerator.TokenIssuer = AppSetting.TokenIssuer;
            TokenGenerator.TokenSignature = AppSetting.TokenSignature;
            //
            services.AddTransient<WebServerFileHelper>();
            services.AddTransient<TransactionRepository>();
            services.AddTransient<Repository>();

            //DAL 등록
            services.AddTransient<APIDao>();
            services.AddTransient<CategoriesDao>();
            services.AddTransient<PrivateFileDao>();
            services.AddTransient<ProductsDao>();
            services.AddTransient<PublicFileDao>();
            services.AddTransient<LogDao>();
            //BLL 등록
            services.AddTransient<APIBll>();
            services.AddTransient<CategoriesBll>();
            services.AddTransient<PrivateFileBll>();
            services.AddTransient<ProductsBll>();
            services.AddTransient<PublicFileBll>();
            services.AddTransient<LogBll>();

            //서비스 등록
            services.AddScoped<IUserService, UserService>();

            //기타 등록
            services.Configure<StorageMaps>(Configuration.GetSection("StorageMaps"));
        }
        private void StorageFactorySetting(IServiceCollection services)
        {
            //스토리지 연결정보 팩토리구성
            var storagesSection = Configuration.GetSection("StorageConnections");
            var storage = storagesSection.Get<StorageConnections>();
            services.Configure<StorageConnections>(storagesSection);

            services.AddTransient<IFileProtocol, NetDriveProtocol>(serviceProvider =>
            {
                return new NetDriveProtocol
                {
                    Name = "MirosConnection",
                    UploadHost = storage.MirosConnection["UploadHost"].ToString(),
                    UserId = storage.MirosConnection["UserId"].ToString(),
                    UserPass = storage.MirosConnection["UserPass"].ToString(),
                };
            });
            services.AddTransient<MusicService>();
            services.AddTransient<IFileProtocol, FTPProtocol>(serviceProvider =>
            {
                return new FTPProtocol
                {
                    Name = "PrivateWorkConnection",
                    UploadHost = storage.PrivateWorkConnection["UploadHost"].ToString(),
                    UserId = storage.PrivateWorkConnection["UserId"].ToString(),
                    UserPass = storage.PrivateWorkConnection["UserPass"].ToString(),
                    TmpUploadFolder = storage.PrivateWorkConnection["TmpUploadFolder"].ToString(),
                    UploadFolder = storage.PrivateWorkConnection["UploadFolder"].ToString(),
                    EncodingType = Convert.ToInt32(storage.PrivateWorkConnection["EncodingType"]),
                };
            });
            services.AddTransient<IFileProtocol, FTPProtocol>(serviceProvider =>
            {
                return new FTPProtocol
                {
                    Name = "PublicWorkConnection",
                    UploadHost = storage.PublicWorkConnection["UploadHost"].ToString(),
                    UserId = storage.PublicWorkConnection["UserId"].ToString(),
                    UserPass = storage.PublicWorkConnection["UserPass"].ToString(),
                    TmpUploadFolder = storage.PublicWorkConnection["TmpUploadFolder"].ToString(),
                    UploadFolder = storage.PublicWorkConnection["UploadFolder"].ToString(),
                    EncodingType = Convert.ToInt32(storage.PublicWorkConnection["EncodingType"]),
                };
            });
            services.AddTransient<IFileProtocol, FTPProtocol>(serviceProvider =>
            {
                return new FTPProtocol
                {
                    Name = "DLArchiveConnection",
                    UploadHost = storage.DLArchiveConnection["UploadHost"].ToString(),
                    UserId = storage.DLArchiveConnection["UserId"].ToString(),
                    UserPass = storage.DLArchiveConnection["UserPass"].ToString(),
                    EncodingType = Convert.ToInt32(storage.PublicWorkConnection["EncodingType"]),
                };
            });
            services.AddTransient<ServiceResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case "PrivateWorkConnection":
                        return serviceProvider.GetServices<IFileProtocol>().First(impl => impl.Name == key);
                    case "PublicWorkConnection":
                        return serviceProvider.GetServices<IFileProtocol>().First(impl => impl.Name == key);
                    case "MirosConnection":
                        return serviceProvider.GetServices<IFileProtocol>().First(impl => impl.Name == key);
                    case "DLArchiveConnection":
                        return serviceProvider.GetServices<IFileProtocol>().First(impl => impl.Name == key);
                    default:
                        throw new Exception("KeyNotFoundException"); // or maybe return null, up to you
                }
            });
        }
    }
}
