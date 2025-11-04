using System;

namespace TarjetaSube
{
    public class Colectivo
    {
        private string numeroLinea;
        private decimal valorPasaje;

        public Colectivo(string linea)
        {
            numeroLinea = linea;
            valorPasaje = 1580;
        }

        public string ObtenerLinea()
        {
            return numeroLinea;
        }

        public Boleto PagarCon(Tarjeta tarjeta)
        {
            return PagarCon(tarjeta, DateTime.Now);
        }

        public Boleto PagarCon(Tarjeta tarjeta, DateTime tiempo)
        {
            decimal montoACobrar = valorPasaje;
            bool esTransbordo = false;

            // Verificar si puede hacer trasbordo (todas las tarjetas pueden)
            if (tarjeta.PuedeHacerTrasbordo(numeroLinea, tiempo))
            {
                esTransbordo = true;
                montoACobrar = 0;
            }

            // Procesar TarjetaMedioBoleto
            if (tarjeta is TarjetaMedioBoleto medioBoleto)
            {
                if (!medioBoleto.PuedeViajarEnEsteHorario())
                {
                    return null;
                }

                if (!esTransbordo)
                {
                    montoACobrar = medioBoleto.CalcularDescuento(valorPasaje);
                }
            }

            // Procesar TarjetaBoletoGratuito
            else if (tarjeta is TarjetaBoletoGratuito boletoGratuito)
            {
                // Verificar horario de franquicia (Lun-Vie 6-22)
                DateTime ahora = tiempo;
                bool esHorarioPermitido = (ahora.DayOfWeek != DayOfWeek.Saturday &&
                                          ahora.DayOfWeek != DayOfWeek.Sunday &&
                                          ahora.Hour >= 6 && ahora.Hour < 22);

                if (!esHorarioPermitido)
                {
                    return null;
                }

                if (!esTransbordo)
                {
                    montoACobrar = boletoGratuito.CalcularDescuento(valorPasaje);
                }

                // Procesar el pago (siempre exitoso para boleto gratuito si hay saldo o dentro del límite negativo)
                bool pagoExitoso = tarjeta.DescontarSaldo(montoACobrar);

                if (pagoExitoso)
                {
                    // Registrar el viaje
                    boletoGratuito.RegistrarViaje();
                    tarjeta.RegistrarViajeParaTrasbordo(numeroLinea, tiempo);

                    Boleto boletoBG = new Boleto(numeroLinea, montoACobrar, tarjeta.ObtenerSaldo(), esTransbordo);
                    return boletoBG;
                }
                else
                {
                    return null;
                }
            }

            // Procesar TarjetaFranquiciaCompleta
            else if (tarjeta is TarjetaFranquiciaCompleta franquicia)
            {
                if (!franquicia.PuedeViajarEnEsteHorario())
                {
                    return null;
                }

                if (!esTransbordo)
                {
                    montoACobrar = franquicia.CalcularDescuento(valorPasaje);
                }

                // Para franquicia completa, registrar y retornar inmediatamente
                tarjeta.RegistrarViajeParaTrasbordo(numeroLinea, tiempo);
                Boleto boletoFC = new Boleto(numeroLinea, montoACobrar, tarjeta.ObtenerSaldo(), esTransbordo);
                return boletoFC;
            }

            // Aplicar descuento por uso frecuente solo para tarjeta normal
            if (tarjeta.GetType() == typeof(Tarjeta) && !esTransbordo)
            {
                montoACobrar = tarjeta.CalcularDescuentoUsoFrecuente(valorPasaje);
            }

            // Procesar el pago
            bool pagoExitoso2 = tarjeta.DescontarSaldo(montoACobrar);

            if (pagoExitoso2)
            {
                // Registrar el viaje
                tarjeta.RegistrarViaje();
                tarjeta.RegistrarViajeParaTrasbordo(numeroLinea, tiempo);

                Boleto boleto = new Boleto(numeroLinea, montoACobrar, tarjeta.ObtenerSaldo(), esTransbordo);
                return boleto;
            }
            else
            {
                return null;
            }
        }
    }
}