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
            return PagarCon(tarjeta, DateTime.Now);
        }

        public Boleto PagarCon(Tarjeta tarjeta, DateTime tiempo)
        {
            decimal montoACobrar = valorPasaje;
            decimal saldoAnterior = tarjeta.ObtenerSaldo();

            if (tarjeta is TarjetaFranquiciaCompleta)
            {
                TarjetaFranquiciaCompleta franquicia = (TarjetaFranquiciaCompleta)tarjeta;
                montoACobrar = franquicia.CalcularDescuento(valorPasaje);
                decimal totalAbonado = montoACobrar;
                Boleto boletito = new Boleto(numeroLinea, montoACobrar, tarjeta.ObtenerSaldo(), tarjeta.ObtenerTipo(), tarjeta.ID, totalAbonado);
                return boletito;
            }

            if (tarjeta is TarjetaMedioBoleto)
            {
                TarjetaMedioBoleto medioBoleto = (TarjetaMedioBoleto)tarjeta;

                if (!medioBoleto.PuedeViajar(tiempo))
                {
                    return null;
                }

                montoACobrar = medioBoleto.CalcularDescuento(valorPasaje, tiempo);
            }

            if (tarjeta is TarjetaBoletoGratuito)
            {
                TarjetaBoletoGratuito gratuito = (TarjetaBoletoGratuito)tarjeta;
                montoACobrar = gratuito.CalcularDescuento(valorPasaje);
            }

            bool pagoExitoso = tarjeta.DescontarSaldo(montoACobrar);

            if (pagoExitoso)
            {
                if (tarjeta is TarjetaMedioBoleto)
                {
                    ((TarjetaMedioBoleto)tarjeta).RegistrarViaje(tiempo);
                }

                decimal totalAbonado = montoACobrar;
                if (saldoAnterior < 0)
                {
                    decimal deudaAnterior = Math.Abs(saldoAnterior);
                    totalAbonado = montoACobrar;
                }

                Boleto boleto = new Boleto(numeroLinea, montoACobrar, tarjeta.ObtenerSaldo(), tarjeta.ObtenerTipo(), tarjeta.ID, totalAbonado);
                return boleto;
            }
            else
            {
                return null;
            }
        }
    }
}