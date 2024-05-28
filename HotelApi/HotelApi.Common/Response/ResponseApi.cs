using System.Net;

namespace HotelApi.Common.Response
{
    public class ResponseApi
    {
        public ResponseApi()
        {
            Messages = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> Messages { get; set; }
        public object Result { get; set; }
    }
}
