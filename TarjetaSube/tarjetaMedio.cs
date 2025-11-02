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
    }
}
