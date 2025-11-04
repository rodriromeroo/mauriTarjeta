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
            
            // verifica horario para medio boleto
            if (tarjeta is TarjetaMedioBoleto) {
                TarjetaMedioBoleto medioBoleto = (TarjetaMedioBoleto)tarjeta;
                
                if (!medioBoleto.PuedeViajarEnEsteHorario()) {
                    return null; // no puede viajar fuera de horario
                }
                
                montoACobrar = medioBoleto.CalcularDescuento(valorPasaje);
            }
            
            // verifica horario para boleto gratuito
            if (tarjeta is TarjetaBoletoGratuito) {
                TarjetaBoletoGratuito gratuito = (TarjetaBoletoGratuito)tarjeta;
                
                if (!gratuito.PuedeViajarEnEsteHorario()) {
                    return null; // no puede viajar fuera de horario
                }
                
                if (gratuito.PuedeViajarGratis()) {
                    gratuito.RegistrarViajeGratuito();
                    montoACobrar = 0;
                } else {
                    montoACobrar = valorPasaje; // cobra completo despues del segundo viaje
                }
            }
            
            // verifica horario para franquicia completa
            if (tarjeta is TarjetaFranquiciaCompleta) {
                TarjetaFranquiciaCompleta franquicia = (TarjetaFranquiciaCompleta)tarjeta;
                
                if (!franquicia.PuedeViajarEnEsteHorario()) {
                    return null; // no puede viajar fuera de horario
                }
                
                montoACobrar = franquicia.CalcularDescuento(valorPasaje);
                Boleto boletito = new Boleto(numeroLinea, montoACobrar, tarjeta.ObtenerSaldo());
                return boletito;
            }
            
            // aplica descuento de uso frecuente solo para tarjetas normales
            if (tarjeta.GetType() == typeof(Tarjeta)) {
                montoACobrar = tarjeta.CalcularDescuentoUsoFrecuente(valorPasaje);
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