using System;

namespace TarjetaSube
{
    public class TarjetaBoletoGratuito : Tarjeta
    {
        private int viajesGratuitosHoy;
        private DateTime ultimaFechaViaje;
        private const int MAX_VIAJES_GRATUITOS_DIA = 2;

        public TarjetaBoletoGratuito() : base()
        {
            viajesGratuitosHoy = 0;
            ultimaFechaViaje = DateTime.MinValue;
        }

        public decimal CalcularDescuento(decimal monto)
        {
            DateTime hoy = DateTime.Now.Date;

            // Resetear contador si es un nuevo día
            if (ultimaFechaViaje.Date != hoy)
            {
                viajesGratuitosHoy = 0;
            }

            // Si ya usó los 2 viajes gratuitos del día, cobra precio completo
            if (viajesGratuitosHoy >= MAX_VIAJES_GRATUITOS_DIA)
            {
                return monto;
            }

            // Viaje gratuito
            return 0;
        }

        public bool PuedeViajarGratis()
        {
            DateTime hoy = DateTime.Now.Date;

            if (ultimaFechaViaje.Date != hoy)
            {
                viajesGratuitosHoy = 0;
            }

            return viajesGratuitosHoy < MAX_VIAJES_GRATUITOS_DIA;
        }

        public bool PuedeViajarEnEsteHorario()
        {
            DateTime ahora = DateTime.Now;
            
            // Verifica si es lunes a viernes
            if (ahora.DayOfWeek == DayOfWeek.Saturday || ahora.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }
            
            // Verifica si está entre las 6 y las 22
            if (ahora.Hour < 6 || ahora.Hour >= 22)
            {
                return false;
            }
            
            return true;
        }

        public void RegistrarViaje()
        {
            DateTime hoy = DateTime.Now.Date;

            // Resetear contador si es un nuevo día
            if (ultimaFechaViaje.Date != hoy)
            {
                viajesGratuitosHoy = 0;
            }

            viajesGratuitosHoy++;
            ultimaFechaViaje = DateTime.Now;
        }

        public void RegistrarViajeGratuito()
        {
            RegistrarViaje();
        }

        public int ObtenerViajesGratuitosHoy()
        {
            DateTime hoy = DateTime.Now.Date;

            if (ultimaFechaViaje.Date != hoy)
            {
                return 0;
            }

            return viajesGratuitosHoy;
        }
    }
}