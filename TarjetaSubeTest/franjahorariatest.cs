using NUnit.Framework;
using TarjetaSube;
using System;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class FranquiciaHorarioTest
    {
        [Test]
        public void MedioBoleto_PuedeViajarEnHorarioPermitido()
        {
            // Este test solo funciona si se ejecuta en dias de semana entre 6 y 22hs
            // Para testing completo, se deberia usar una interfaz de tiempo mockeada

            TarjetaMedioBoleto medio = new TarjetaMedioBoleto();
            DateTime ahora = DateTime.Now;

            // verificar si estamos en horario permitido
            bool esHorarioPermitido = (ahora.DayOfWeek != DayOfWeek.Saturday &&
                                       ahora.DayOfWeek != DayOfWeek.Sunday &&
                                       ahora.Hour >= 6 && ahora.Hour < 22);

            Assert.AreEqual(esHorarioPermitido, medio.PuedeViajarEnEsteHorario());
        }

        [Test]
        public void MedioBoleto_VerificaDiaLaboral()
        {
            TarjetaMedioBoleto medio = new TarjetaMedioBoleto();
            DateTime ahora = DateTime.Now;

            bool resultado = medio.PuedeViajarEnEsteHorario();

            // si es sabado o domingo, no deberia poder
            if (ahora.DayOfWeek == DayOfWeek.Saturday || ahora.DayOfWeek == DayOfWeek.Sunday)
            {
                Assert.IsFalse(resultado);
            }
        }

        [Test]
        public void BoletoGratuito_NoTieneRestriccionHorario()
        {
            // Cambiado: TarjetaBoletoGratuito ya no tiene restricciones de horario
            TarjetaBoletoGratuito gratuito = new TarjetaBoletoGratuito();

            // Según implementación actual, siempre puede viajar
            // El método PuedeViajarEnEsteHorario fue removido
            Colectivo colectivo = new Colectivo("102");
            Boleto boleto = colectivo.PagarCon(gratuito);

            Assert.IsNotNull(boleto); // Siempre puede generar boleto
        }

        [Test]
        public void FranquiciaCompleta_VerificaHorario()
        {
            TarjetaFranquiciaCompleta franquicia = new TarjetaFranquiciaCompleta();
            DateTime ahora = DateTime.Now;

            bool resultado = franquicia.PuedeViajarEnEsteHorario();

            // verificar que en fin de semana no puede
            if (ahora.DayOfWeek == DayOfWeek.Saturday || ahora.DayOfWeek == DayOfWeek.Sunday)
            {
                Assert.IsFalse(resultado);
            }
        }

        [Test]
        public void Colectivo_MedioBoleto_NoPermitePagoFueraHorario()
        {
            // Nota: este test solo pasara si se ejecuta fuera del horario permitido
            // En un entorno de produccion, se usaria dependency injection para el tiempo

            TarjetaMedioBoleto medio = new TarjetaMedioBoleto();
            medio.CargarSaldo(5000);
            Colectivo colectivo = new Colectivo("102");
            DateTime ahora = DateTime.Now;

            Boleto boleto = colectivo.PagarCon(medio);

            // si estamos fuera de horario, deberia devolver null
            bool esHorarioPermitido = (ahora.DayOfWeek != DayOfWeek.Saturday &&
                                       ahora.DayOfWeek != DayOfWeek.Sunday &&
                                       ahora.Hour >= 6 && ahora.Hour < 22);

            if (!esHorarioPermitido)
            {
                Assert.IsNull(boleto);
            }
            else
            {
                Assert.IsNotNull(boleto);
            }
        }

        [Test]
        public void Colectivo_BoletoGratuito_SiemprePermitePago()
        {
            // Cambiado: BoletoGratuito siempre permite pago sin restricciones
            TarjetaBoletoGratuito gratuito = new TarjetaBoletoGratuito();
            gratuito.CargarSaldo(5000);
            Colectivo colectivo = new Colectivo("K");

            Boleto boleto = colectivo.PagarCon(gratuito);

            // Siempre deberia poder pagar, sin importar horario
            Assert.IsNotNull(boleto);
        }

        [Test]
        public void Colectivo_FranquiciaCompleta_NoPermitePagoFueraHorario()
        {
            TarjetaFranquiciaCompleta franquicia = new TarjetaFranquiciaCompleta();
            Colectivo colectivo = new Colectivo("144");
            DateTime ahora = DateTime.Now;

            Boleto boleto = colectivo.PagarCon(franquicia);

            bool esHorarioPermitido = (ahora.DayOfWeek != DayOfWeek.Saturday &&
                                       ahora.DayOfWeek != DayOfWeek.Sunday &&
                                       ahora.Hour >= 6 && ahora.Hour < 22);

            if (!esHorarioPermitido)
            {
                Assert.IsNull(boleto);
            }
        }

        [Test]
        public void TarjetaNormal_NoTieneRestriccionHorario()
        {
            // Las tarjetas normales pueden viajar en cualquier horario
            Tarjeta normal = new Tarjeta();
            normal.CargarSaldo(5000);
            Colectivo colectivo = new Colectivo("27");

            Boleto boleto = colectivo.PagarCon(normal);

            // siempre deberia poder pagar, sin importar horario
            Assert.IsNotNull(boleto);
        }

        [Test]
        public void MetodosPuedeViajar_DevuelvenBooleano()
        {
            TarjetaMedioBoleto medio = new TarjetaMedioBoleto();
            TarjetaFranquiciaCompleta franquicia = new TarjetaFranquiciaCompleta();

            // verificar que los metodos existen y devuelven bool
            Assert.IsInstanceOf<bool>(medio.PuedeViajarEnEsteHorario());
            Assert.IsInstanceOf<bool>(franquicia.PuedeViajarEnEsteHorario());

            // TarjetaBoletoGratuito ya no tiene este método
            TarjetaBoletoGratuito gratuito = new TarjetaBoletoGratuito();
            Colectivo colectivo = new Colectivo("102");
            Boleto boleto = colectivo.PagarCon(gratuito);
            Assert.IsNotNull(boleto); // Verificamos que funciona en su lugar
        }
    }
}