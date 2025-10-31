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

        public bool PagarCon(Tarjeta tarjeta) {
            decimal montoACobrar = valorPasaje;
            
            if (tarjeta is TarjetaFranquiciaCompleta) {
                TarjetaFranquiciaCompleta franquicia = (TarjetaFranquiciaCompleta)tarjeta;
                return franquicia.SiemprePuedePagar();
            }
            
            if (tarjeta is TarjetaMedioBoleto) {
                TarjetaMedioBoleto medioBoleto = (TarjetaMedioBoleto)tarjeta;
                montoACobrar = medioBoleto.CalcularDescuento(valorPasaje);
            }
            if (tarjeta is TarjetaMedioBoleto) {
                TarjetaMedioBoleto medioBoleto = (TarjetaMedioBoleto)tarjeta;
                montoACobrar = medioBoleto.CalcularDescuento(valorPasaje);
            }

            if (tarjeta is TarjetaFranquiciaCompleta) {
                TarjetaFranquiciaCompleta gratuito = (TarjetaFranquiciaCompleta)tarjeta;
                montoACobrar = gratuito.CalcularDescuento(valorPasaje);
                return true;
            }
            
            bool pagoExitoso = tarjeta.DescontarSaldo(montoACobrar);
            return pagoExitoso;
        }
    }
}