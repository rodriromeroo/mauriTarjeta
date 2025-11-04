using System;
using NUnit.Framework;
using TarjetaSube;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class LimitacionFranquiciaCompletaTest
    {
        [Test]
        public void BoletoGratuito_PermiteTodosLosViajesGratis()
        {
            // Arrange
            TarjetaBoletoGratuito tarjeta = new TarjetaBoletoGratuito();
            tarjeta.CargarSaldo(5000);
            Colectivo colectivo = new Colectivo("102");

            // Crear viajes en horario permitido (Lun-Vie 6-22)
            DateTime horarioPermitido = new DateTime(2024, 11, 20, 10, 0, 0); // Miércoles 10am

            // Act
            Boleto primerViaje = colectivo.PagarCon(tarjeta, horarioPermitido);
            Boleto segundoViaje = colectivo.PagarCon(tarjeta, horarioPermitido.AddMinutes(10));
            Boleto tercerViaje = colectivo.PagarCon(tarjeta, horarioPermitido.AddMinutes(20));

            // Assert - solo los primeros 2 viajes del día son gratis
            Assert.IsNotNull(primerViaje);
            Assert.IsNotNull(segundoViaje);
            Assert.IsNotNull(tercerViaje);

            Assert.AreEqual(0, primerViaje.ImportePagado);
            Assert.AreEqual(0, segundoViaje.ImportePagado);
            Assert.AreEqual(1580, tercerViaje.ImportePagado); // Tercer viaje cobra tarifa completa
        }

        [Test]
        public void BoletoGratuito_NuncaDescuentaSaldo()
        {
            // Arrange
            TarjetaBoletoGratuito tarjeta = new TarjetaBoletoGratuito();
            tarjeta.CargarSaldo(10000);
            Colectivo colectivo = new Colectivo("K");
            DateTime horarioPermitido = new DateTime(2024, 11, 20, 10, 0, 0);

            decimal saldoInicial = tarjeta.ObtenerSaldo();

            // Act - primeros 2 viajes (gratis)
            colectivo.PagarCon(tarjeta, horarioPermitido);
            colectivo.PagarCon(tarjeta, horarioPermitido.AddMinutes(10));

            decimal saldoFinal = tarjeta.ObtenerSaldo();

            // Assert - los 2 primeros viajes no descuentan
            Assert.AreEqual(saldoInicial, saldoFinal);
        }

        [Test]
        public void BoletoGratuito_SiemprePuedeViajar()
        {
            // Arrange
            TarjetaBoletoGratuito tarjeta = new TarjetaBoletoGratuito();
            tarjeta.CargarSaldo(20000);
            Colectivo colectivo = new Colectivo("144");
            DateTime horarioPermitido = new DateTime(2024, 11, 20, 10, 0, 0);

            // Act y Assert - puede hacer todos los viajes, aunque después del 2do pague
            for (int i = 0; i < 10; i++)
            {
                Boleto boleto = colectivo.PagarCon(tarjeta, horarioPermitido.AddMinutes(i * 10));
                Assert.IsNotNull(boleto);
            }
        }

        [Test]
        public void BoletoGratuito_SinSaldo_SigueFuncionando()
        {
            // Arrange
            TarjetaBoletoGratuito tarjeta = new TarjetaBoletoGratuito();
            // No carga saldo
            Colectivo colectivo = new Colectivo("27");
            DateTime horarioPermitido = new DateTime(2024, 11, 20, 10, 0, 0);

            // Act - primeros 2 viajes gratis, el tercero necesita saldo
            Boleto primerViaje = colectivo.PagarCon(tarjeta, horarioPermitido);
            Boleto segundoViaje = colectivo.PagarCon(tarjeta, horarioPermitido.AddMinutes(10));
            Boleto tercerViaje = colectivo.PagarCon(tarjeta, horarioPermitido.AddMinutes(20));

            // Assert - los primeros 2 funcionan, el tercero necesita saldo
            Assert.IsNotNull(primerViaje);
            Assert.IsNotNull(segundoViaje);
            Assert.IsNull(tercerViaje); // No tiene saldo para el tercer viaje
        }

        [Test]
        public void BoletoGratuito_PuedeViajarGratisVerificaCorrectamente()
        {
            // Arrange
            TarjetaBoletoGratuito tarjeta = new TarjetaBoletoGratuito();
            tarjeta.CargarSaldo(10000);
            Colectivo colectivo = new Colectivo("144");
            DateTime horarioPermitido = new DateTime(2024, 11, 20, 10, 0, 0);

            // Act y Assert - verifica que PuedeViajarGratis funcione correctamente
            Assert.IsTrue(tarjeta.PuedeViajarGratis()); // Puede viajar gratis al inicio

            Boleto boleto1 = colectivo.PagarCon(tarjeta, horarioPermitido);
            Assert.IsNotNull(boleto1);
            Assert.IsTrue(tarjeta.PuedeViajarGratis()); // Aún puede viajar gratis (1 de 2)

            Boleto boleto2 = colectivo.PagarCon(tarjeta, horarioPermitido.AddMinutes(10));
            Assert.IsNotNull(boleto2);
            Assert.IsFalse(tarjeta.PuedeViajarGratis()); // Ya no puede viajar gratis (2 de 2)

            Boleto boleto3 = colectivo.PagarCon(tarjeta, horarioPermitido.AddMinutes(20));
            Assert.IsNotNull(boleto3);
        }

        [Test]
        public void BoletoGratuito_ContadorViajesGratuitosFuncionaCorrectamente()
        {
            // Arrange
            TarjetaBoletoGratuito tarjeta = new TarjetaBoletoGratuito();
            tarjeta.CargarSaldo(10000);
            Colectivo colectivo = new Colectivo("133");
            DateTime horarioPermitido = new DateTime(2024, 11, 20, 10, 0, 0);

            // Act y Assert
            Assert.AreEqual(0, tarjeta.ObtenerViajesGratuitosHoy());

            Boleto boleto1 = colectivo.PagarCon(tarjeta, horarioPermitido);
            Assert.AreEqual(0, boleto1.ImportePagado);
            Assert.AreEqual(1, tarjeta.ObtenerViajesGratuitosHoy());

            Boleto boleto2 = colectivo.PagarCon(tarjeta, horarioPermitido.AddMinutes(10));
            Assert.AreEqual(0, boleto2.ImportePagado);
            Assert.AreEqual(2, tarjeta.ObtenerViajesGratuitosHoy());

            Boleto boleto3 = colectivo.PagarCon(tarjeta, horarioPermitido.AddMinutes(20));
            Assert.AreEqual(1580, boleto3.ImportePagado); // Ya no es gratis
            Assert.AreEqual(3, tarjeta.ObtenerViajesGratuitosHoy()); // Aumenta a 3 porque registra el viaje
        }
    }
}