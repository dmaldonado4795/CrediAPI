using CrediAPI.Web.Response;
using System.Collections.Generic;

namespace CrediAPI.Domain.Services
{
    public interface IPlanPagoService
    {
        List<PlanPagoResponse> CalcularPlanDePago(decimal monto, decimal interes, int meses);
    }
}
