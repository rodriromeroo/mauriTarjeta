using System;
using System.Collections.Generic;

namespace TarjetaSube
{
    public class Tarjeta
    {
        private decimal saldo;
        private List<decimal> montosPermitidos;

        public Tarjeta()
        {
            saldo = 0;
            montosPermitidos = new List<decimal>();
            montosPermitidos.Add(2000);
            montosPermitidos.Add(3000);
            montosPermitidos.Add(4000);
            montosPermitidos.Add(5000);
            montosPermitidos.Add(8000);
            montosPermitidos.Add(10000);
            montosPermitidos.Add(15000);
            montosPermitidos.Add(20000);
            montosPermitidos.Add(25000);
            montosPermitidos.Add(30000);
        }

        public decimal ObtenerSaldo()
        {
            return saldo;
        }

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

        public bool DescontarSaldo(decimal monto)
        {
            if (saldo < monto)
            {
                return false;
            }

            saldo = saldo - monto;
            return true;
        }
    }
}