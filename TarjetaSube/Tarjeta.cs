using System;
using System.Collections.Generic;

namespace TarjetaSube
{
    public class Tarjeta
    {
        protected decimal saldo;
        private List<decimal> montosPermitidos;
        private const decimal LIMITE_NEGATIVO = -1200m;
        private const decimal LIMITE_MAXIMO = 56000m;
        private decimal saldoPendiente;
        
        // para boleto de uso frecuente
        private int viajesDelMes;
        private DateTime ultimoMesRegistrado;

        public Tarjeta()
        {
            saldo = 0;
            saldoPendiente = 0;
            montosPermitidos = new List<decimal> {
                2000, 3000, 4000, 5000, 8000, 10000, 15000, 20000, 25000, 30000
            };
            viajesDelMes = 0;
            ultimoMesRegistrado = DateTime.Now;
        }

        public decimal ObtenerSaldo()
        {
            return saldo;
        }

        public decimal ObtenerSaldoPendiente()
        {
            return saldoPendiente;
        }

        public int ObtenerViajesDelMes()
        {
            VerificarMes();
            return viajesDelMes;
        }

        private void VerificarMes()
        {
            DateTime ahora = DateTime.Now;
            if (ahora.Month != ultimoMesRegistrado.Month || ahora.Year != ultimoMesRegistrado.Year)
            {
                viajesDelMes = 0;
                ultimoMesRegistrado = ahora;
            }
        }

        public void RegistrarViaje()
        {
            VerificarMes();
            viajesDelMes++;
        }

        public decimal CalcularDescuentoUsoFrecuente(decimal montoBase)
        {
            VerificarMes();
            
            if (viajesDelMes >= 30 && viajesDelMes < 60)
            {
                return montoBase * 0.80m; // 20% descuento
            }
            else if (viajesDelMes >= 60 && viajesDelMes < 80)
            {
                return montoBase * 0.75m; // 25% descuento
            }
            
            return montoBase; // tarifa normal
        }

        public bool CargarSaldo(decimal monto)
        {
            if (!montosPermitidos.Contains(monto))
            {
                return false;
            }

            if (saldoPendiente > 0)
            {
                AcreditarCarga();
            }

            decimal nuevoSaldo = saldo + monto;

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