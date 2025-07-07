using CrediAPI.Web.Response;
using System;
using System.Collections.Generic;

namespace CrediAPI.Domain.Services.Impl
{
    public class PlanPagoServiceImpl : IPlanPagoService
    {
        public PlanPagoServiceImpl()
        {

        }

        public List<PlanPagoResponse> CalcularPlanDePago(decimal monto, decimal interes, int meses)
        {

            decimal tasaMensual = interes / 12m;

            var planDePago = new List<PlanPagoResponse>();
            decimal saldoPendiente = monto;
            DateTime fechaVencimientoActual = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1);

            decimal cuotaTotalFija;
            if (tasaMensual == 0)
            {
                cuotaTotalFija = monto / meses;
            }
            else
            {
                cuotaTotalFija = (monto * tasaMensual) / (1m - (decimal)Math.Pow(Convert.ToDouble(1m + tasaMensual), -meses));
            }

            cuotaTotalFija = Math.Round(cuotaTotalFija, 2);

            //NOTE: si la cuota total fija es menor que 0, se ajusta a 0
            for (int i = 1; i <= meses; i++)
            {
                decimal interesPeriodo = saldoPendiente * tasaMensual;
                decimal montoCapital = cuotaTotalFija - interesPeriodo;

                if (i == meses)
                {
                    montoCapital = saldoPendiente;
                    interesPeriodo = cuotaTotalFija - montoCapital;
                    if (interesPeriodo < 0) interesPeriodo = 0;
                    cuotaTotalFija = montoCapital + interesPeriodo;
                }

                if (montoCapital < 0) montoCapital = 0;

                saldoPendiente -= montoCapital;

                if (Math.Abs(saldoPendiente) < 0.01m)
                {
                    saldoPendiente = 0;
                }

                planDePago.Add(new PlanPagoResponse
                {
                    NumeroCuota = i,
                    FechaVencimiento = fechaVencimientoActual,
                    MontoCapital = Math.Round(montoCapital, 2),
                    Interes = Math.Round(interesPeriodo, 2),
                    CuotaTotal = Math.Round(cuotaTotalFija, 2),
                    SaldoPendiente = Math.Round(saldoPendiente, 2)
                });

                fechaVencimientoActual = fechaVencimientoActual.AddMonths(1);
            }

            if (planDePago.Count > 0 && planDePago[planDePago.Count - 1].SaldoPendiente != 0)
            {
                decimal ajusteFinal = planDePago[planDePago.Count - 1].SaldoPendiente;
                planDePago[planDePago.Count - 1].MontoCapital += ajusteFinal;
                planDePago[planDePago.Count - 1].CuotaTotal += ajusteFinal;
                planDePago[planDePago.Count - 1].SaldoPendiente = 0;
            }

            return planDePago;
        }
    }
}