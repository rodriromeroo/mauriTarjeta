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
            DateTime ahora = DateTime.Now;

            // Si cambió el mes, reiniciar contador
            if (ahora.Month != ultimoMesRegistrado.Month || ahora.Year != ultimoMesRegistrado.Year)
            {
                viajesDelMes = 0;
                ultimoMesRegistrado = ahora;
            }

            return viajesDelMes;
        }

        public decimal CalcularDescuentoUsoFrecuente(decimal montoBase)
        {
            DateTime ahora = DateTime.Now;

            // Verificar si cambió el mes
            if (ahora.Month != ultimoMesRegistrado.Month || ahora.Year != ultimoMesRegistrado.Year)
            {
                viajesDelMes = 0;
                ultimoMesRegistrado = ahora;
            }

            // Incrementar contador de viajes
            viajesDelMes++;

            // Aplicar descuentos según cantidad de viajes
            if (viajesDelMes >= 1 && viajesDelMes <= 29)
            {
                return montoBase; // Tarifa normal
            }
            else if (viajesDelMes >= 30 && viajesDelMes <= 59)
            {
                return montoBase * 0.80m; // 20% descuento
            }
            else if (viajesDelMes >= 60 && viajesDelMes <= 80)
            {
                return montoBase * 0.75m; // 25% descuento
            }
            else // viaje 81 en adelante
            {
                return montoBase; // Tarifa normal
            }
        }

        /// carga saldo monto permitido y carga max de 56k, si el saldo es negativo se paga la deuda
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
            if (saldoPendiente > 0)
            {
                decimal espacioDisponible = LIMITE_MAXIMO - saldo;

                if (espacioDisponible > 0)
                {
                    if (saldoPendiente <= espacioDisponible)
                    {
                        saldo += saldoPendiente;
                        saldoPendiente = 0;
                    }
                    else
                    {
                        saldo += espacioDisponible;
                        saldoPendiente -= espacioDisponible;
                    }
                }
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