namespace MAMBrowser
{
    /// <summary>
    /// 결과 코드
    /// </summary>
    public enum RESUlT_CODES
    {
        SUCCESS = 0,
        INVALID_DATA = 1,
        DENY_ACCESS = 2,
        DB_ERROR = 3,
        TOKEN_ERROR = 4,
        TOKEN_EXPIRATION = 5,
        SERVICE_ERROR = 6,
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
