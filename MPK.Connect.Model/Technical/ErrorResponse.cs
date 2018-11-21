using System.Net;

namespace MPK.Connect.Model.Technical
{
    public class ErrorResponse<T> : ApiResponse<T> where T : class
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public ErrorResponse(T result) : base(result)
        {
        }

        public ErrorResponse(T result, string text) : base(result, text)
        {
        }
    }
}