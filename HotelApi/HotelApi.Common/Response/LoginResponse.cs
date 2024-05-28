namespace HotelApi.Common.Response
{
    public class LoginResponse
    {

        public bool IsSuccess { get; set; } = true;
        public string ErrorMessages { get; set; }
        public object Result { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
