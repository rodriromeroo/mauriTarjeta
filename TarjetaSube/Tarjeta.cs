using System;
using System.Collections.Generic;

namespace TarjetaSube
{
    public class Tarjeta
    {
        protected decimal saldo;
        private List<decimal> montosPermitidos;

        // limite negativo de -1200
        private const decimal LIMITE_NEGATIVO = -1200m;
        private const decimal LIMITE_MAXIMO = 56000m; // cambiia de 40000 a 56000
        private decimal saldoPendiente; //  para saldo excedente

        public Tarjeta()
        {
            saldo = 0;
            saldoPendiente = 0;
            montosPermitidos = new List<decimal> {
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

            // intentar acreditar saldo pendiente si existe
            if (saldoPendiente > 0)
            {
                AcreditarCarga();
            }

            decimal nuevoSaldo = saldo + monto;

            // guardar excedente como pendiente
            if (nuevoSaldo > LIMITE_MAXIMO)
            {
                decimal excedente = nuevoSaldo - LIMITE_MAXIMO;
                saldo = LIMITE_MAXIMO;
                saldoPendiente += excedente;
                return true;
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

            if (saldoPendiente > 0)
            {
                AcreditarCarga();
            }

            return true;
        }
    }
}