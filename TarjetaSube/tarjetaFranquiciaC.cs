using System;

namespace TarjetaSube {
    public class TarjetaFranquiciaCompleta : Tarjeta {
        
        public TarjetaFranquiciaCompleta() : base() {
        }

        public decimal CalcularDescuento(decimal monto) {
            return 0;
        }
        
        public bool SiemprePuedePagar() {
            return true;
        }
    }
}