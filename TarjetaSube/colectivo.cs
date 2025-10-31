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
            bool pagoExitoso = tarjeta.DescontarSaldo(valorPasaje);
            return pagoExitoso;
        }
    }
}