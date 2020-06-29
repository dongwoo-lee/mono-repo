using MAMBrowser.Entiies;
using Microsoft.EntityFrameworkCore;

namespace MAMBrowser.Helpers
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        //protected readonly IConfiguration Configuration;

        //public DataContext(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        /**
         * EntityFramworkCore 패키지 설치가 필요함.
         */
        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //{
        //    // connect to sql server database
        //    options.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"));
        //}

        //public DbSet<User> Users { get; set; }
    }
}
