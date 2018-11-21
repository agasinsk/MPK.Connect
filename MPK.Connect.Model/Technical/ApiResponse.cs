using System;
using System.Net;

namespace MPK.Connect.Model.Technical
{
    public abstract class ApiResponse<T> where T : class
    {
        public virtual HttpStatusCode StatusCode { get; set; }
        public string Text { get; set; }
        public T Result { get; set; }

        protected ApiResponse(T result)
        {
            Result = result ?? throw new ArgumentNullException(nameof(result));
        }

        protected ApiResponse(T result, string text)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Result = result ?? throw new ArgumentNullException(nameof(result));
        }
    }
}