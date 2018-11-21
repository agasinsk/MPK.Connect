using System.Net;
using Microsoft.AspNetCore.Mvc;
using MPK.Connect.Model.Technical;

namespace MPK.Connect.WebApp
{
    public static class ResponseExtensions
    {
        public static IActionResult GetActionResult<T>(this ApiResponse<T> response) where T : class
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return new BadRequestObjectResult(response);
            }

            return new OkObjectResult(response);
        }
    }
}