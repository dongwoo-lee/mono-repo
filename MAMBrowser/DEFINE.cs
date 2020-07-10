using SmartSql;
using SmartSql.DbSession;

namespace MAMBrowser
{
    public enum RESUlT_CODES
    {
        SUCCESS = 0,
        INVALID_DATA = 1,
        DB_ERROR = 2,
        EXPIRATION_TOKEN = 3,
        SERVICE_ERROR = 4,
    }

    public enum LOG_CATEGORIES
    {
        CALL,   //debug system call method
        AUTO_EXECUTE,    
        MANUAL_EXECUTE,
        KNOWN_EXCEPTION,
        UNKNOWN_EXCEPTION,
    }

    public static class SqlMap
    {
        public static SmartSqlBuilder SqlBuilder;
        public static IDbSession GetSession()
        {
            if (SqlBuilder == null)
            {
                SqlBuilder = new SmartSqlBuilder()
               .UseXmlConfig()
               .Build();
            }
            return SqlBuilder.GetDbSessionFactory().Open();
        }
    }
}
