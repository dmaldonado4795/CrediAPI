using CrediAPI.Domain.Services;
using CrediAPI.Web.DTOs.Request;
using CrediAPI.Web.DTOs.Response;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace CrediAPI.Web.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly IAuthService _authService;
        private readonly IUsuarioService _userService;

        public AuthController(
            IAuthService authService,
            IUsuarioService userService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost, Route("login")]
        public async Task<IHttpActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Invalid login request.");
            }

            try
            {
                var user = await _userService.FindByUsername(request.Username);
                if (user == null)
                {
                    return HandleUnauthorized();
                }

                var authResponse = _authService.Authenticate(user, request.Password);
                if (authResponse == null)
                {
                    return HandleUnauthorized();
                }

                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Title = "Internal Server Error",
                    Description = ex.Message,
                    Data = ex
                });
            }
        }

        private IHttpActionResult HandleUnauthorized()
        {
            return Content(HttpStatusCode.Unauthorized, new BaseResponse
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                Title = "Unauthorized",
                Description = "You are not authorized to access this resource.",
                Data = null
            });
        }
    }
}