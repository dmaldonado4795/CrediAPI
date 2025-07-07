using CrediAPI.Domain.Services;
using CrediAPI.Web.DTOs.Response;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace CrediAPI.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/plazos")] 
    public class PlazosController : ApiController
    {
        private readonly IPlazosService _service;

        public PlazosController(IPlazosService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet, Route("find-all")]
        public async Task<IHttpActionResult> GetPlazos()
        {
            try
            {
                var plazos = await _service.FindAll();
                return Ok(new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Title = "Success",
                    Description = plazos.Any() ? "Plazos retrieved successfully" : "No plazos found",
                    Data = plazos
                });
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
    }
}