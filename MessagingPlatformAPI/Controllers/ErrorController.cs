using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessagingPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult HandleError()
        {
            // ASP.NET automatically redirects unhandled exceptions here.
            return Problem(
                statusCode: 500,
                title: "Internal Server Error",
                detail: "Something went wrong. Please try again later."
            );
        }
    }
}
