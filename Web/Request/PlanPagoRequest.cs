namespace CrediAPI.Web.Request
{
    public class PlanPagoRequest
    {
        public int ProductoId { get; set; }
        public int PlazoId { get; set; }
        public decimal MontoSolicitado { get; set; }
    }
}