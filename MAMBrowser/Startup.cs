using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MAMBrowser.Helpers;
using System;
using VueCliMiddleware;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.IO;
using MAMBrowser.Services;
using System.Linq;
using MAMBrowser.BLL;
using MAMBrowser.Middleware;
using Microsoft.Extensions.Options;
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
using MAMBrowser.Hubs;
using MAMBrowser.Workers;
using M30_ManagementControlDAO;
using M30_ManagementControlDAO.Interfaces;
using M30_ManagementControlDAO.DAO;

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
            services.AddSignalR();
            services.AddHostedService<WebCueSheetServiceWorker>();

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
                
                endpoints.MapHub<ProgressHub>("/api/ProgressHub");
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
            //�ɼ� DI
            var optionSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(optionSection);

            //�۷ι� ����
            AppSetting = optionSection.Get<AppSettings>();
            QueryHelper.ConnectionString = AppSetting.ConnectionString;
            TokenGenerator.UseToken = true;
            TokenGenerator.ExpireHour = AppSetting.ExpireMusicTokenHour;
            TokenGenerator.TokenIssuer = AppSetting.TokenIssuer;
            TokenGenerator.TokenSignature = AppSetting.TokenSignature;

            //ť��Ʈ ���� DI ���
            services.AddTransient<ICueSheetDAO, CueSheetDAO>();
            services.AddTransient<ICommonDAO, CommonDAO>();
            services.AddTransient<ITemplateDAO, TemplateDAO>();
            services.AddTransient<IFavoritesDAO, FavoritesDAO>();
            services.AddTransient<IUserManagementDAO, UserManagementDAO>();
            services.AddTransient<IGroupManagementDAO, GroupManagementDAO>();
            services.AddTransient<ICodeManagementDAO, CodeManagementDAO>();
            services.AddTransient<IMirosManagementDAO, MirosManagementDAO>();
            services.AddTransient<IDelManagementDAO,DelManagementDAO>();
            services.AddTransient<ITransMissionListDAO,TransMissionListDAO>();
            services.AddCueSheetDAOConnectionString(AppSetting.ConnectionString);
            ManagementControlSqlSession.ConnectionString = AppSetting.ConnectionString;
            MAMWebFactory.Instance.Setting(AppSetting.ConnectionString);

            services.AddTransient<WebServerFileHelper>();
            services.AddTransient<QueryHelper>();
            services.AddTransient<HttpContextDBLogger>();

            //DAL ���
            services.AddTransient<APIDao>();
            services.AddTransient<CategoriesDao>();
            services.AddTransient<PrivateFileDao>();
            services.AddTransient<ProductsDao>();
            services.AddTransient<PublicFileDao>();
            services.AddTransient<LogDao>();
            //BLL ���
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
            services.AddTransient<CueAttachmentsBll>();

            services.AddTransient<ManagementControlSqlSession>();
            services.AddTransient<ManagementSystemBll>();
            services.AddTransient<ManagementDeleteProductsBll>();

            services.AddTransient<TransMissionListBll>();

            //���� ���
            services.AddScoped<IUserService, UserService>();
        }
        private void StorageFactorySetting(IServiceCollection services)
        {
            //���丮�� �������� ���丮����
            services.AddTransient<MusicWebService>(serviceProvider =>
            {
                var storagesSection = Configuration.GetSection("StorageConnections:External:MusicConnection");
                var storage = storagesSection.Get<ExternalStorage>();
                var logger = serviceProvider.GetRequiredService<ILogger<MusicWebService>>();
                return new MusicWebService(storage, logger, Startup.AppSetting.ExpireMusicTokenHour);
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
                    return new FTPProtocol(sm.UserId, sm.UserPass, sm.EncodingType);
                case MAMDefine.SMB:
                    return new NetDriveProtocol(sm.UserId, sm.UserPass);
                default:
                    return null;
            }
        }
    }
}
