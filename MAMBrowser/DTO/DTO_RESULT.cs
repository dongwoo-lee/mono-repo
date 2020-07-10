namespace MAMBrowser.DTO
{
    public class DTO_RESULT<T>
    {
        public RESUlT_CODES RESULT_CODE { get; set; }
        public T RESULT_OBJECT { get; set; }
        public string ERROR_MSG { get; set; }
    }

    public class DTO_RESULT
    {
        public RESUlT_CODES RESULT_CODE { get; set; }
        public string RESULT_OBJECT { get; set; }
        public string ERROR_MSG { get; set; }
    }
}
