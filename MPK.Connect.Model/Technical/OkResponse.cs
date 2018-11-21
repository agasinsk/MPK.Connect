using System.Net;

namespace MPK.Connect.Model.Technical
{
    public class OkResponse<T> : ApiResponse<T> where T : class
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.OK;

        public OkResponse(T result) : base(result)
        {
        }

        public OkResponse(T result, string text) : base(result, text)
        {
        }
    }
}