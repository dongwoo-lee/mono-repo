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
using M30.AudioFile.DAL;
using MAMBrowser.BLL;
using MAMBrowser.Middleware;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using MAMBrowser.Foundation;
using MAMBrowser.Helper;
using M30.AudioFile.DAL.Dao;
using M30.AudioFile.Common.Foundation;
using M30_CueSheetDAO.DAO;
using M30_CueSheetDAO.Interfaces;
using M30_CueSheetDAO;
using M30.AudioFile.DAL.Expand.Factories.Web;
using M30.AudioFile.DAL.WebService;

namespace MAMBrowser
{
    public delegate StorageManager ServiceResolver(string key);
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
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
            QueryHelper.ConnectionString = AppSetting.ConnectionString;
            TokenGenerator.UseToken = true;
            TokenGenerator.ExpireHour = AppSetting.ExpireMusicTokenHour;
            TokenGenerator.TokenIssuer = AppSetting.TokenIssuer;
            TokenGenerator.TokenSignature = AppSetting.TokenSignature;

            //큐시트 관련 DI 등록
            services.AddTransient<ICueSheetDAO, CueSheetDAO>();
            services.AddTransient<ICommonDAO, CommonDAO>();
            services.AddTransient<ITemplateDAO, TemplateDAO>();
            services.AddTransient<IFavoritesDAO, FavoritesDAO>();
            services.AddCueSheetDAOConnectionString(AppSetting.ConnectionString);
            MAMWebFactory.Instance.Setting(AppSetting.ConnectionString);

            services.AddTransient<WebServerFileHelper>();
            //services.AddTransient<TransactionRepository>();
            services.AddTransient<QueryHelper>();
            services.AddTransient<HttpContextDBLogger>();


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
            services.AddTransient<CueUserInfoBll>();
            services.AddTransient<DayCueSheetBll>();
            services.AddTransient<DefCueSheetBll>();
            services.AddTransient<TemplateBll>();
            services.AddTransient<FavoriteBll>();
            services.AddTransient<ArchiveCueSheetBll>();

            //서비스 등록
            services.AddScoped<IUserService, UserService>();
        }
        private void StorageFactorySetting(IServiceCollection services)
        {
            //스토리지 연결정보 팩토리구성
            services.AddTransient<MusicWebService>(serviceProvider =>
            {
                var storagesSection = Configuration.GetSection("StorageConnections:External:MusicConnection");
                var storage = storagesSection.Get<ExternalStorage>();
                var logger = serviceProvider.GetRequiredService<ILogger<MusicWebService>>();
                return new MusicWebService(storage, logger,  Startup.AppSetting.ExpireMusicTokenHour);
            });
            services.AddTransient(serviceProvider =>
            {
                var storagesSection = Configuration.GetSection("StorageConnections:Internal:PrivateWorkConnection");
                var storage = storagesSection.Get<StorageManager>();
                storage.FileSystem = GetProtocol(storage);
                return storage;
            });
            services.AddTransient(serviceProvider =>
            {
                var storagesSection = Configuration.GetSection("StorageConnections:Internal:PrivateWorkConnection");
                var storage = storagesSection.Get<StorageManager>();
                storage.FileSystem = GetProtocol(storage);
                return storage;
            });

            services.AddTransient(serviceProvider =>
            {
                var storagesSection = Configuration.GetSection("StorageConnections:Internal:PublicWorkConnection");
                var storage = storagesSection.Get<StorageManager>();
                storage.FileSystem = GetProtocol(storage);
                return storage;
            });
            services.AddTransient(serviceProvider =>
            {
                var storagesSection = Configuration.GetSection("StorageConnections:Internal:MirosConnection");
                var storage = storagesSection.Get<StorageManager>();
                storage.FileSystem = GetProtocol(storage);
                return storage;
            });
            services.AddTransient(serviceProvider =>
            {
                var storagesSection = Configuration.GetSection("StorageConnections:Internal:DLArchiveConnection");
                var storage = storagesSection.Get<StorageManager>();
                storage.FileSystem = GetProtocol(storage);
                return storage;
            });
            services.AddTransient<ServiceResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case MAMDefine.PrivateWorkConnection:
                        return serviceProvider.GetServices<StorageManager>().First(impl => impl.Name == key);
                    case MAMDefine.PublicWorkConnection:
                        return serviceProvider.GetServices<StorageManager>().First(impl => impl.Name == key);
                    case MAMDefine.MirosConnection:
                        return serviceProvider.GetServices<StorageManager>().First(impl => impl.Name == key);
                    case MAMDefine.DLArchiveConnection:
                        return serviceProvider.GetServices<StorageManager>().First(impl => impl.Name == key);
                    default:
                        throw new Exception("KeyNotFoundException"); // or maybe return null, up to you
                }
            });
        }
        private IFileProtocol GetProtocol(StorageManager sm)
        {
            switch (sm.Protocol)
            {
                case MAMDefine.FTP:
                    return new FTPProtocol(sm.UploadHost, sm.UserId, sm.UserPass, sm.TmpUploadFolder, sm.UploadFolder, sm.EncodingType);
                case MAMDefine.SMB:
                    return new NetDriveProtocol(sm.UploadHost, sm.UserId, sm.UserPass, sm.TmpUploadFolder, sm.UploadFolder);
                default:
                    return null;
            }
        }
    }
}
