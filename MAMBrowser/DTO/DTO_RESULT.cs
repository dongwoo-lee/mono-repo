namespace MAMBrowser.DTO
{
    public class DTO_RESULT<T> where T : class, new()
    {
        public RESUlT_CODES ResultCode { get; set; }
        public T ResultObject { get; set; } 
        public string ErrorMsg { get; set; }

        public DTO_RESULT()
        {
            this.ResultObject = new T();
        }
    }

    public class DTO_RESULT
    {
        public RESUlT_CODES RESULT_CODE { get; set; }
        public string RESULT_OBJECT { get; set; }
        public string ERROR_MSG { get; set; }
    }
}
