using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Helpers
{
    public class DataContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
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
