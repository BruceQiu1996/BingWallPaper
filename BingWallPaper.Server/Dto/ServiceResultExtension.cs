using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BingWallPaper.Server.Dto
{
    public static class ServiceResultExtension
    {
        public static ActionResult ToActionResult(this ServiceResult result)
        {
            ActionResult actionResult = result.StatusCode switch
            {
                HttpStatusCode.BadRequest => new BadRequestObjectResult(result.Message),
                HttpStatusCode.NotFound => new NotFoundObjectResult(result.Message),
                _ => new OkResult(),
            };

            return actionResult;
        }

        public static ActionResult ToActionResult<T>(this ServiceResult<T> result)
        {
            if (typeof(T) == typeof(string))
            {
                return new ContentResult()
                {
                    StatusCode = (int)result.StatusCode,
                    Content = result.Value as string,
                    ContentType = "text/plain"
                };
            }
            ActionResult actionResult = result.StatusCode switch
            {
                HttpStatusCode.BadRequest => new BadRequestObjectResult(result.Message),
                HttpStatusCode.NotFound => new NotFoundObjectResult(result.Message),
                _ => new OkObjectResult(result.Value),
            };

            return actionResult;
        }
    }
}
