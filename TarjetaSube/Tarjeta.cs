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
        protected decimal saldoPendiente;

        private int viajesDelMes;
        private DateTime ultimoMesRegistrado;

        private DateTime ultimoViajeParaTrasbordo;
        private string ultimaLineaViajada;

        public Tarjeta()
        {
            saldo = 0;
            saldoPendiente = 0;
            montosPermitidos = new List<decimal> {
                2000, 3000, 4000, 5000, 8000, 10000, 15000, 20000, 25000, 30000
            };
            viajesDelMes = 0;
            ultimoMesRegistrado = DateTime.Now;
            ultimoViajeParaTrasbordo = DateTime.MinValue;
            ultimaLineaViajada = "";
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
            if (ahora.Month != ultimoMesRegistrado.Month || ahora.Year != ultimoMesRegistrado.Year)
            {
                viajesDelMes = 0;
                ultimoMesRegistrado = ahora;
            }
            return viajesDelMes;
        }

        public void RegistrarViaje()
        {
            DateTime ahora = DateTime.Now;
            if (ahora.Month != ultimoMesRegistrado.Month || ahora.Year != ultimoMesRegistrado.Year)
            {
                viajesDelMes = 0;
                ultimoMesRegistrado = ahora;
            }
            viajesDelMes++;
        }

        public decimal CalcularDescuentoUsoFrecuente(decimal montoBase)
        {
            int viajes = ObtenerViajesDelMes();

            if (viajes >= 1 && viajes <= 29)
            {
                return montoBase;
            }
            else if (viajes >= 30 && viajes <= 59)
            {
                return montoBase * 0.80m;
            }
            else if (viajes >= 60 && viajes <= 80)
            {
                return montoBase * 0.75m;
            }
            else
            {
                return montoBase;
            }
        }

        public virtual bool PuedeHacerTrasbordo(string lineaColectivo, DateTime ahora)
        {
            // Debe ser línea diferente
            if (ultimaLineaViajada == lineaColectivo)
            {
                return false;
            }

            // Debe ser dentro de 1 hora
            TimeSpan tiempoTranscurrido = ahora - ultimoViajeParaTrasbordo;
            if (tiempoTranscurrido.TotalHours > 1)
            {
                return false;
            }

            // Lunes a sábado (no domingos)
            if (ahora.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }

            // De 7:00 a 22:00
            if (ahora.Hour < 7 || ahora.Hour >= 22)
            {
                return false;
            }

            return true;
        }

        public void RegistrarViajeParaTrasbordo(string lineaColectivo, DateTime ahora)
        {
            ultimoViajeParaTrasbordo = ahora;
            ultimaLineaViajada = lineaColectivo;
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

        protected void AcreditarCarga()
        {
            if (saldoPendiente <= 0)
            {
                return;
            }

            decimal espacioDisponible = LIMITE_MAXIMO - saldo;

            if (espacioDisponible <= 0)
            {
                return;
            }

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