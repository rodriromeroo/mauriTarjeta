using System;

namespace TarjetaSube
{
    public class TarjetaMedioBoleto : Tarjeta
    {
        public TarjetaMedioBoleto() : base()
        {
        }

        public decimal CalcularDescuento(decimal monto)
        {
            return monto / 2;
        }

        public bool PuedeViajarEnEsteHorario()
        {
            DateTime ahora = DateTime.Now;
            
            // verifica si es lunes a viernes
            if (ahora.DayOfWeek == DayOfWeek.Saturday || ahora.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }
            
            // verifica si esta entre las 6 y las 22
            if (ahora.Hour < 6 || ahora.Hour >= 22)
            {
                return false;
            }
            
            return true;
        }

        public override bool DescontarSaldo(decimal monto)
        {
            bool resultado = base.DescontarSaldo(monto);
            return resultado;
        }
    }
}