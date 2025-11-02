using System;
using System.Collections.Generic;

namespace TarjetaSube {
    public class Tarjeta {
        private decimal saldo;
        private List<decimal> montosPermitidos;
<<<<<<< HEAD
        
        public Tarjeta() {
=======

        // limite negativo de -1200
        private const decimal LIMITE_NEGATIVO = -1200m;

        public Tarjeta()
        {
>>>>>>> 3294baa320cb1fcc299e87b589c3caa8558f31d3
            saldo = 0;
            montosPermitidos = new List<decimal>
            {
                2000, 3000, 4000, 5000, 8000, 10000, 15000, 20000, 25000, 30000
            };
        }

        public decimal ObtenerSaldo() {
            return saldo;
        }

<<<<<<< HEAD
        public bool CargarSaldo(decimal monto) {
            if (!montosPermitidos.Contains(monto)) {
=======
        /// carga saldo monto permitido y carga max de 40k, si el saldo es negativo se paga la deuda
       
        public bool CargarSaldo(decimal monto)
        {
            if (!montosPermitidos.Contains(monto))
            {
>>>>>>> 3294baa320cb1fcc299e87b589c3caa8558f31d3
                return false;
            }

            decimal nuevoSaldo = saldo + monto;
            if (nuevoSaldo > 40000) {
                return false;
            }

            saldo = nuevoSaldo;
            return true;
        }

<<<<<<< HEAD
        public bool DescontarSaldo(decimal monto) {
            if (saldo < monto) {
=======
        /// descuenta saldo y no deja pasar de -1200
        public bool DescontarSaldo(decimal monto)
        {
            decimal saldoResultado = saldo - monto;

            if (saldoResultado < LIMITE_NEGATIVO)
            {
>>>>>>> 3294baa320cb1fcc299e87b589c3caa8558f31d3
                return false;
            }

            saldo = saldoResultado;
            return true;
        }
    }
}
