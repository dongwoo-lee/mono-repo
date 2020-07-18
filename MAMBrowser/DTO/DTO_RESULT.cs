namespace MAMBrowser.DTO
{
    public class DTO_RESULT<T> 
    {
        public RESUlT_CODES ResultCode { get; set; }
        public T ResultObject { get; set; } 
        public string ErrorMsg { get; set; }

        public DTO_RESULT()
        {
        }   
    }

    public class DTO_RESULT
    {
        public RESUlT_CODES ResultCode { get; set; }
        public string ResultObject { get; set; }
        public string ErrorMsg { get; set; }
    }
}
