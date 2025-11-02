using System;

namespace TarjetaSube {
    public class TarjetaBoletoGratuito : Tarjeta {
        
        public TarjetaBoletoGratuito() : base() {
        }

        public decimal CalcularDescuento(decimal monto) {
            return 0;
        }
    }
}