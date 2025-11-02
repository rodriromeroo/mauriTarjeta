using System;
using System.Collections.Generic;

namespace TarjetaSube
{
    public class Tarjeta
    {
        private decimal saldo;
        private List<decimal> montosPermitidos;

        // limite negativo de -1200
        private const decimal LIMITE_NEGATIVO = -1200m;

        public Tarjeta()
        {
            saldo = 0;
            montosPermitidos = new List<decimal>
            {
                2000, 3000, 4000, 5000, 8000, 10000, 15000, 20000, 25000, 30000
            };
        }

        public decimal ObtenerSaldo()
        {
            return saldo;
        }

        /// carga saldo monto permitido y carga max de 40k, si el saldo es negativo se paga la deuda
       
        public bool CargarSaldo(decimal monto)
        {
            if (!montosPermitidos.Contains(monto))
            {
                return false;
            }

            decimal nuevoSaldo = saldo + monto;
            if (nuevoSaldo > 40000)
            {
                return false;
            }

            saldo = nuevoSaldo;
            return true;
        }

        /// descuenta saldo y no deja pasar de -1200
        public bool DescontarSaldo(decimal monto)
        {
            decimal saldoResultado = saldo - monto;

            if (saldoResultado < LIMITE_NEGATIVO)
            {
                return false;
            }

            saldo = saldoResultado;
            return true;
        }
    }
}
