namespace HotelApi.Common.Response
{
    public class ResponseEmail
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; }
        public object Result { get; set; }
    }
}
