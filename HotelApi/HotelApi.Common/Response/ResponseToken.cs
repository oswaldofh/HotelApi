namespace HotelApi.Common.Response
{
    public class ResponseToken
    {
        public bool IsSuccess { get; set; } = true;
        public string? Message { get; set; }
        public string? AccessToken { get; set; }
    }
}
