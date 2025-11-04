using System;

namespace TarjetaSube {
    public class TarjetaBoletoGratuito : Tarjeta {
        
        private int viajesGratuitosHoy;
        private DateTime ultimaFechaViaje;
        private const int MAX_VIAJES_GRATUITOS_DIA = 2;
        
        public TarjetaBoletoGratuito() : base() {
            viajesGratuitosHoy = 0;
            ultimaFechaViaje = DateTime.MinValue;
        }

        public decimal CalcularDescuento(decimal monto) {
            DateTime hoy = DateTime.Now.Date;
            
            // Si es un nuevo dia, reinicia el contador
            if (ultimaFechaViaje.Date != hoy) {
                viajesGratuitosHoy = 0;
            }
            
            // Si ya uso los 2 viajes gratuitos del dia, cobra tarifa completa
            if (viajesGratuitosHoy >= MAX_VIAJES_GRATUITOS_DIA) {
                return monto;
            }
            
            // Si todavia tiene viajes gratuitos, es gratis
            return 0;
        }
        
        public bool PuedeViajarGratis() {
            DateTime hoy = DateTime.Now.Date;
            
            // Si es un nuevo dia, reinicia el contador
            if (ultimaFechaViaje.Date != hoy) {
                viajesGratuitosHoy = 0;
            }
            
            return viajesGratuitosHoy < MAX_VIAJES_GRATUITOS_DIA;
        }
        
        public void RegistrarViaje() {
            DateTime hoy = DateTime.Now.Date;
            
            // Si es un nuevo dia, reinicia el contador
            if (ultimaFechaViaje.Date != hoy) {
                viajesGratuitosHoy = 0;
            }
            
            viajesGratuitosHoy++;
            ultimaFechaViaje = DateTime.Now;
        }
        
        public int ObtenerViajesGratuitosHoy() {
            DateTime hoy = DateTime.Now.Date;
            
            // Si es un nuevo dia, devuelve 0
            if (ultimaFechaViaje.Date != hoy) {
                return 0;
            }
            
            return viajesGratuitosHoy;
        }
    }
}