namespace MAMBrowser
{
    /// <summary>
    /// 결과 코드
    /// </summary>
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
   
}
