using System;

namespace TarjetaSube
{
    public class TarjetaFranquiciaCompleta : Tarjeta
    {

        public TarjetaFranquiciaCompleta() : base()
        {
        }

        public decimal CalcularDescuento(decimal monto)
        {
            return 0;
        }

        // no cambia mucho pero nos aseguramos que funcione la acreditacion
        public override bool DescontarSaldo(decimal monto)
        {
            // no descuenta saldo, acredita pendiente
            if (ObtenerSaldoPendiente() > 0)
            {
                AcreditarCarga();
            }
            return true;
        }
    }
}