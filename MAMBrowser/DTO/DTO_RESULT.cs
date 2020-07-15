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
        public RESUlT_CODES ResultCode { get; set; }
        public string ResultObject { get; set; }
        public string ErrorMsg { get; set; }
    }
}
