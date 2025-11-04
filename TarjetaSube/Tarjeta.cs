using System;
using System.Collections.Generic;

namespace TarjetaSube
{
    public class Tarjeta
    {
        protected decimal saldo;
        private List<decimal> montosPermitidos;
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

        public decimal ObtenerSaldoPendiente()
        {
            return saldoPendiente;
        }

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

        // para acreditar saldo pendiente
        public void AcreditarCarga()
        {
            if (saldoPendiente <= 0) return;

            decimal espacioDisponible = LIMITE_MAXIMO - saldo;
            if (espacioDisponible > 0)
            {
                decimal montoAAcreditar = Math.Min(saldoPendiente, espacioDisponible);
                saldo += montoAAcreditar;
                saldoPendiente -= montoAAcreditar;
            }
        }

        public virtual bool DescontarSaldo(decimal monto)
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