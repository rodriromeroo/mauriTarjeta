using System;

namespace TarjetaSube
{
    public class Colectivo
    {
        private string numeroLinea;
        private decimal valorPasaje;

        public Colectivo(string linea)
        {
            numeroLinea = linea;
            valorPasaje = 1580;
        }

        public string ObtenerLinea()
        {
            return numeroLinea;
        }

        public Boleto PagarCon(Tarjeta tarjeta)
        {
            bool pagoExitoso = tarjeta.DescontarSaldo(valorPasaje);

            if (pagoExitoso)
            {
                decimal saldoRestante = tarjeta.ObtenerSaldo();
                Boleto nuevoBoleto = new Boleto(numeroLinea, valorPasaje, saldoRestante);
                return nuevoBoleto;
            }
            else
            {
                return null;
            }
        }
    }
}