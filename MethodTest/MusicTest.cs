using MAMBrowser;
using MAMBrowser.BLL;
using MAMBrowser.Controllers;
using MAMBrowser.DAL;
using MAMBrowser.DTO;
using MAMBrowser.Helpers;
using MAMBrowser.Processor;
using MAMBrowser.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Xml.Serialization;
using Xunit;

namespace MethodTest
{
    public class Startup
    {
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        //public IConfiguration Configuration { get; }

        IConfiguration Configuration;
        public void ConfigureServices(IServiceCollection services)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();
            Configuration = (IConfiguration)config;

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
                    MAMUtility.LocalIpAddress = ip.ToString();
                    break;
                }
            }
        }
        private void DISetting(IServiceCollection services)
        {
            //DAL 등록
            services.AddTransient<APIDAL>();
            services.AddTransient<CategoriesDAL>();
            services.AddTransient<PrivateFileDAL>();
            services.AddTransient<ProductsDAL>();
            services.AddTransient<PublicFileDAL>();

            //서비스 등록
            services.AddScoped<IUserService, UserService>();
            services.AddTransient<LogService>();

            //기타 등록
            services.Configure<StorageMaps>(Configuration.GetSection("StorageMaps"));
        }
        private void StorageFactorySetting(IServiceCollection services)
        {
            //스토리지 연결정보 팩토리구성
            var storagesSection = Configuration.GetSection("StorageConnections");
            services.Configure<StorageConnections>(storagesSection);
            var storage = storagesSection.Get<StorageConnections>();
            services.AddTransient<IFileService, NetDriveService>(serviceProvider =>
            {
                return new NetDriveService
                {
                    Name = "MirosConnection",
                    UploadHost = storage.PrivateWorkConnection["Host"].ToString(),
                };
            });
            services.AddTransient<MusicService>();
            services.AddTransient<IFileService, FtpService>(serviceProvider =>
            {
                return new FtpService
                {
                    Name = "PrivateWorkConnection",
                    UploadHost = storage.PrivateWorkConnection["Host"].ToString(),
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
                    UploadHost = storage.PublicWorkConnection["Host"].ToString(),
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
                    UploadHost = storage.DLArchiveConnection["Host"].ToString(),
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
                    case "DLArchiveConnection":
                        return serviceProvider.GetServices<IFileService>().First(impl => impl.Name == key);
                    default:
                        throw new Exception("KeyNotFoundException"); // or maybe return null, up to you
                }
            });
        }
    }

    public class MusicTest
    {
        private readonly MusicService _fileService;
        public MusicTest(MusicService fileService) => _fileService = fileService;

        //[Fact]
        //public void TempDownload()
        //{
        //    string imagefilePath = @"\\192.168.1.201\detail-small-2.jpg";
        //    string wavfilePath = @"\\mibis-wave1\midas_0302\20170224\99\DDG00205.wav";

        //    var wavToken = MAMUtility.GenerateMusicToken(wavfilePath);
        //    var albumToken = MAMUtility.GenerateMusicToken(wavfilePath);


        //    _fileService.TempDownloadWavAndEgy("raduieng", "192.168.1.231", wavToken);
        //}

        [Fact]
        public void GetImagePathList()
        {
            //string imagefilePath = @"\\192.168.1.201:90\imagedata\detail-small-2.jpg";
            //string wavfilePath = @"\\mibis-wave1\midas_0302\20170224\99\DDG00205.wav";
            //var wavToken = MAMUtility.GenerateMusicToken(wavfilePath);
            //var albumToken = MAMUtility.GenerateMusicToken(imagefilePath);

            //var fileNameList = _fileService.TempImageDownload("radioeng","localhost",wavToken, albumToken);
        }

        [Fact]
        public void GetImage()
        {
            //string filePath = @"mibis_011\midas_0302\20170224\99\DDG00205.jpg";
            //var alumbImageFilePath = MAMUtility.GenerateMusicToken(filePath);
            //string contentType;
            //_fileService.GetAlbumImage(alumbImageFilePath, out contentType);
        }
    }
}
