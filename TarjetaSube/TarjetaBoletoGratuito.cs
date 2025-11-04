using System;

namespace TarjetaSube
{
    public class TarjetaBoletoGratuito : Tarjeta
    {
        private int viajesGratuitosHoy;
        private DateTime ultimoDiaRegistrado;

        public TarjetaBoletoGratuito() : base()
        {
            viajesGratuitosHoy = 0;
            ultimoDiaRegistrado = DateTime.Now.Date;
        }

        public decimal CalcularDescuento(decimal monto)
        {
            return 0;
        }

        private void VerificarDia()
        {
            DateTime hoy = DateTime.Now.Date;
            if (hoy != ultimoDiaRegistrado)
            {
                viajesGratuitosHoy = 0;
                ultimoDiaRegistrado = hoy;
            }
        }

        public bool PuedeViajarGratis()
        {
            VerificarDia();
            return viajesGratuitosHoy < 2;
        }

        public int ObtenerViajesGratuitosHoy()
        {
            VerificarDia();
            return viajesGratuitosHoy;
        }

        public void RegistrarViajeGratuito()
        {
            VerificarDia();
            viajesGratuitosHoy++;
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
            if (ObtenerSaldoPendiente() > 0)
            {
                AcreditarCarga();
            }
            return true;
        }
    }
}