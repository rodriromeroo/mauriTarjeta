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

        // no cambia mucho pero nos aseguramos que funcione la acreditacion
        public override bool DescontarSaldo(decimal monto)
        {
            bool resultado = base.DescontarSaldo(monto);
            return resultado;
        }
    }
}