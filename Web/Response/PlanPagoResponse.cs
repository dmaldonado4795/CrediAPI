using System;

namespace CrediAPI.Web.Response
{
    public class PlanPagoResponse
    {
        public int NumeroCuota { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal MontoCapital { get; set; }
        public decimal Interes { get; set; }
        public decimal CuotaTotal { get; set; }
        public decimal SaldoPendiente { get; set; }
    }
}