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
            
            // Si es boleto gratuito, calcula el descuento y registra el viaje
            if (tarjeta is TarjetaBoletoGratuito) {
                TarjetaBoletoGratuito gratuito = (TarjetaBoletoGratuito)tarjeta;
                montoACobrar = gratuito.CalcularDescuento(valorPasaje);
                
                // Si es gratis (0), no descuenta saldo y registra el viaje
                if (montoACobrar == 0) {
                    gratuito.RegistrarViaje();
                    Boleto boletoGratis = new Boleto(numeroLinea, montoACobrar, tarjeta.ObtenerSaldo());
                    return boletoGratis;
                }
                // Si cobra tarifa completa, descuenta normalmente
            }
            
            // Intenta descontar el saldo
            bool pagoExitoso = tarjeta.DescontarSaldo(montoACobrar);
            
            if (pagoExitoso) {
                // Si es boleto gratuito y pago (tercer viaje o mas), registra el viaje
                if (tarjeta is TarjetaBoletoGratuito) {
                    TarjetaBoletoGratuito gratuito = (TarjetaBoletoGratuito)tarjeta;
                    gratuito.RegistrarViaje();
                }
                
                Boleto boleto = new Boleto(numeroLinea, montoACobrar, tarjeta.ObtenerSaldo());
                return boleto;
            } else {
                return null;
            }
        }
    }
}