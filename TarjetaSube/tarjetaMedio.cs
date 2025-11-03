using System;

namespace TarjetaSube
{
    public class TarjetaMedioBoleto : Tarjeta
    {
        private DateTime ultimoViaje;
        private int viajesHoy;
        private DateTime fechaViajesHoy;

        public TarjetaMedioBoleto() : base()
        {
            ultimoViaje = DateTime.MinValue;
            viajesHoy = 0;
            fechaViajesHoy = DateTime.MinValue;
        }

        public override string ObtenerTipo()
        {
            return "Medio Boleto";
        }

        public decimal CalcularDescuento(decimal monto, DateTime ahora)
        {
            if (fechaViajesHoy.Date != ahora.Date)
            {
                viajesHoy = 0;
                fechaViajesHoy = ahora;
            }

            if (viajesHoy >= 2)
            {
                return monto;
            }

            return monto / 2;
        }

        public bool PuedeViajar(DateTime ahora)
        {
            TimeSpan tiempoTranscurrido = ahora - ultimoViaje;

            if (tiempoTranscurrido.TotalMinutes < 5)
            {
                return false;
            }

            return true;
        }

        public void RegistrarViaje(DateTime ahora)
        {
            if (fechaViajesHoy.Date != ahora.Date)
            {
                viajesHoy = 0;
                fechaViajesHoy = ahora;
            }

            ultimoViaje = ahora;
            viajesHoy++;
        }

        public int ObtenerViajesHoy()
        {
            return viajesHoy;
        }
    }
}