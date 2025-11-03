using System;

namespace TarjetaSube
{
    public class TarjetaBoletoGratuito : Tarjeta
    {
        public TarjetaBoletoGratuito() : base()
        {
        }

        public override string ObtenerTipo()
        {
            return "Boleto Gratuito";
        }

        public decimal CalcularDescuento(decimal monto)
        {
            return 0;
        }
    }
}