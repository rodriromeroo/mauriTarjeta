using System;

namespace TarjetaSube {

    public class Colectivo {

        private string numeroLinea;
        private decimal valorPasaje;

        public Colectivo(string linea) {
            numeroLinea = linea;
            valorPasaje = 1580;
        }

        public string ObtenerLinea() {
            return numeroLinea;
        }

        public Boleto PagarCon(Tarjeta tarjeta) {
            decimal montoACobrar = valorPasaje;
            
            // Si es franquicia completa, siempre puede pagar
            if (tarjeta is TarjetaFranquiciaCompleta) {
                TarjetaFranquiciaCompleta franquicia = (TarjetaFranquiciaCompleta)tarjeta;
                montoACobrar = franquicia.CalcularDescuento(valorPasaje);
                Boleto boletito = new Boleto(numeroLinea, montoACobrar, tarjeta.ObtenerSaldo());
                return boletito;
            }
            
            // Si es medio boleto, cobra la mitad
            if (tarjeta is TarjetaMedioBoleto) {
                TarjetaMedioBoleto medioBoleto = (TarjetaMedioBoleto)tarjeta;
                montoACobrar = medioBoleto.CalcularDescuento(valorPasaje);
            }
            
            // Si es boleto gratuito, no cobra nada
            if (tarjeta is TarjetaBoletoGratuito) {
                TarjetaBoletoGratuito gratuito = (TarjetaBoletoGratuito)tarjeta;
                montoACobrar = gratuito.CalcularDescuento(valorPasaje);
            }
            
            // Intenta descontar el saldo
            bool pagoExitoso = tarjeta.DescontarSaldo(montoACobrar);
            
            if (pagoExitoso) {
                Boleto boleto = new Boleto(numeroLinea, montoACobrar, tarjeta.ObtenerSaldo());
                return boleto;
            } else {
                return null;
            }
        }
    }
}