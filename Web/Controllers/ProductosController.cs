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
    [RoutePrefix("api/productos")]
    public class ProductosController : ApiController
    {
        private readonly IProductoService _service;

        public ProductosController(IProductoService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet, Route("find-all")]
        public async Task<IHttpActionResult> GetProductos()
        {
            try
            {
                var productos = await _service.FindAll();
                return Ok(new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Title = "Success",
                    Description = productos.Any() ? "Productos retrieved successfully" : "No productos found",
                    Data = productos
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