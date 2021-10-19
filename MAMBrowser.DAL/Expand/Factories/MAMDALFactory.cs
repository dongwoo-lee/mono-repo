using MAMBrowser.Common.Foundation;
using System;

namespace MAMBrowser.DAL.Expand.Factories
{
    internal class MAMDALFactory
    {
        private static readonly Lazy<MAMDALFactory> lazy = new Lazy<MAMDALFactory>(() => new MAMDALFactory());
        public static MAMDALFactory Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public APIDao APIDao { get; private set; }
        public CategoriesDao CategoriesDao { get; private set; }
        public LogDao LogDao { get; private set; }
        public PrivateFileDao PrivateFileDao { get; private set; }
        public ProductsDao ProductsDao { get; private set; }
        public PublicFileDao PublicFileDao { get; private set; }

        public void Setting(string ConnectionString, int ExpireHour, string IPAddress, string TokenIssuer, string TokenSignature)
        {
            Repository.ConnectionString = ConnectionString;

            TokenGenerator.ExpireHour = ExpireHour;
            TokenGenerator.IPAddress = IPAddress;
            TokenGenerator.TokenIssuer = TokenIssuer;
            TokenGenerator.TokenSignature = TokenSignature;

            Repository repository = new Repository();
            APIDao = new APIDao(repository);
            CategoriesDao = new CategoriesDao(repository);
            LogDao = new LogDao(repository);
            PrivateFileDao = new PrivateFileDao(repository);
            ProductsDao = new ProductsDao(repository);
            PublicFileDao = new PublicFileDao(repository);
        }
    }
}
