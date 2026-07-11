using System.Net;

namespace DinoArgentoApi.Utils
{
    public class ErrorResponse : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public ResponseMessage ErrorMessage { get; }

        public ErrorResponse(HttpStatusCode code, string msg) : base(msg)
        {
            ErrorMessage = new ResponseMessage(msg);
            StatusCode = code;
        }
    }
}
