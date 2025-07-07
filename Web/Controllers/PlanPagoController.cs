using CrediAPI.Domain.Services;
using CrediAPI.Web.DTOs.Response;
using CrediAPI.Web.Request;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace CrediAPI.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/plan-pago")]
    public class PlanPagoController : ApiController
    {
        private readonly IPlazosService _plazosService;
        private readonly IProductoService _productoService;
        private readonly IPlanPagoService _planService;

        public PlanPagoController(IPlazosService plazosService, IProductoService productoService, IPlanPagoService planService)
        {
            _plazosService = plazosService ?? throw new ArgumentNullException(nameof(plazosService));
            _productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            _planService = planService ?? throw new ArgumentNullException(nameof(planService));
        }

        [HttpPost, Route("calculate")]
        public async Task<IHttpActionResult> CalculatePlanPago([FromBody] PlanPagoRequest request)
        {
            if (request == null || request.MontoSolicitado <= 0m || request.PlazoId <= 0 || request.ProductoId <= 0)
            {
                return Content(HttpStatusCode.BadRequest, new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Title = "Bad Request",
                    Description = "Invalid request parameters.",
                    Data = null
                });
            }

            try
            {
                var producto = await _productoService.FindById(request.ProductoId);
                if (producto == null)
                {
                    throw new KeyNotFoundException($"Product with ID {request.ProductoId} not found.");
                }

                var plazo = await _plazosService.FindById(request.PlazoId);
                if (plazo == null)
                {
                    throw new KeyNotFoundException($"Plazo with ID {request.PlazoId} not found.");
                }

                bool allowedAmount = request.MontoSolicitado >= producto.MontoMinimo &&
                    request.MontoSolicitado <= producto.MontoMaximo;

                if (!allowedAmount)
                {
                    return Content(HttpStatusCode.BadRequest, new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Title = "Bad Request",
                        Description = $"The requested amount {request.MontoSolicitado} is not within the acceptable range for the product {producto.Nombre}.",
                        Data = null
                    });
                }

                var response = _planService.CalcularPlanDePago(
                     request.MontoSolicitado,
                     producto.TasaInteres,
                     plazo.Meses
                 );

                return Ok(new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Title = "Calculated Payment Plan",
                    Description = "The payment plan has been successfully calculated",
                    Data = response
                });
            }
            catch (KeyNotFoundException knfEx)
            {
                return Content(HttpStatusCode.BadRequest, new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Title = "Bad Request",
                    Description = knfEx.Message,
                    Data = null
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