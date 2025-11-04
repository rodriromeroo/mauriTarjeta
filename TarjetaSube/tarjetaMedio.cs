using System;

namespace TarjetaSube
{
    public class TarjetaMedioBoleto : Tarjeta
    {
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

        // no cambia mucho pero nos aseguramos que funcione la acreditacion
        public override bool DescontarSaldo(decimal monto)
        {
            bool resultado = base.DescontarSaldo(monto);
            return resultado;
        }
    }
}