using System;
using NUnit.Framework;
using TarjetaSube;

namespace TarjetaSube.Tests
{
    [TestFixture]
    public class BoletoUsoFrecuenteTests
    {
        private Tarjeta tarjeta;

        [SetUp]
        public void Setup()
        {
            tarjeta = new Tarjeta();
        }

        [Test]
        public void ObtenerSaldo_TarjetaNueva_DevuelveCero()
        {
            decimal saldo = tarjeta.ObtenerSaldo();
            Assert.AreEqual(0, saldo);
        }

        [Test]
        public void CargarSaldo_MontoValido2000_RetornaTrue()
        {
            bool resultado = tarjeta.CargarSaldo(2000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(2000, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void CargarSaldo_MontoValido5000_RetornaTrue()
        {
            bool resultado = tarjeta.CargarSaldo(5000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(5000, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void CargarSaldo_MontoValido30000_RetornaTrue()
        {
            bool resultado = tarjeta.CargarSaldo(30000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(30000, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void CargarSaldo_MontoInvalido1000_RetornaFalse()
        {
            bool resultado = tarjeta.CargarSaldo(1000);
            Assert.IsFalse(resultado);
            Assert.AreEqual(0, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void CargarSaldo_MontoInvalido7000_RetornaFalse()
        {
            bool resultado = tarjeta.CargarSaldo(7000);
            Assert.IsFalse(resultado);
            Assert.AreEqual(0, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void CargarSaldo_SuperaLimite56000_GuardaExcedente()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000);
            Colectivo colectivo = new Colectivo("102");

            decimal saldoInicial = tarjeta.ObtenerSaldo();
            decimal tarifa = colectivo.ObtenerValorPasaje();

            // Act - hacer 29 viajes
            for (int i = 0; i < 29; i++)
            {
                Boleto boleto = colectivo.PagarCon(tarjeta);
                Assert.IsNotNull(boleto);
                Assert.AreEqual(tarifa, boleto.ImportePagado);
            }

            // Assert
            decimal saldoFinal = tarjeta.ObtenerSaldo();
            decimal totalGastado = saldoInicial - saldoFinal;
            Assert.AreEqual(29 * tarifa, totalGastado);
            Assert.AreEqual(29, tarjeta.ObtenerViajesDelMes());
        }

        [Test]
        public void Tarjeta_Viajes30a59_Descuento20Porciento()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000);
            Colectivo colectivo = new Colectivo("K");

            decimal tarifa = colectivo.ObtenerValorPasaje();

            // hacer los primeros 29 viajes
            for (int i = 0; i < 29; i++)
            {
                colectivo.PagarCon(tarjeta);
            }

            decimal saldoAntes = tarjeta.ObtenerSaldo();

            // Act - viaje 30 (deberia tener 20% descuento)
            Boleto boleto30 = colectivo.PagarCon(tarjeta);

            // Assert
            decimal montoEsperado = tarifa * 0.80m; // 20% descuento
            Assert.AreEqual(montoEsperado, boleto30.ImportePagado);
            Assert.AreEqual(saldoAntes - montoEsperado, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void Tarjeta_Viajes60a80_Descuento25Porciento()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000);
            Colectivo colectivo = new Colectivo("144");

            decimal tarifa = colectivo.ObtenerValorPasaje();

            // hacer los primeros 59 viajes
            for (int i = 0; i < 59; i++)
            {
                colectivo.PagarCon(tarjeta);
            }

            decimal saldoAntes = tarjeta.ObtenerSaldo();

            // Act - viaje 60 (deberia tener 25% descuento)
            Boleto boleto60 = colectivo.PagarCon(tarjeta);

            // Assert
            decimal montoEsperado = tarifa * 0.75m; // 25% descuento
            Assert.AreEqual(montoEsperado, boleto60.ImportePagado);
            Assert.AreEqual(saldoAntes - montoEsperado, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void Tarjeta_Viajes81EnAdelante_TarifaNormal()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta();
            // cargar bastante saldo
            for (int i = 0; i < 5; i++)
            {
                tarjeta.CargarSaldo(30000);
            }

            Colectivo colectivo = new Colectivo("27");
            decimal tarifa = colectivo.ObtenerValorPasaje();

            // hacer los primeros 80 viajes
            for (int i = 0; i < 80; i++)
            {
                colectivo.PagarCon(tarjeta);
            }

            decimal saldoAntes = tarjeta.ObtenerSaldo();

            // Act - viaje 81 (vuelve a tarifa normal)
            Boleto boleto81 = colectivo.PagarCon(tarjeta);

            // Assert
            Assert.AreEqual(tarifa, boleto81.ImportePagado);
            Assert.AreEqual(saldoAntes - tarifa, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void Tarjeta_VerificaDescuentos_EnRangos()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta();
            for (int i = 0; i < 5; i++)
            {
                tarjeta.CargarSaldo(30000);
            }
            Colectivo colectivo = new Colectivo("133");
            decimal tarifa = colectivo.ObtenerValorPasaje();

            // Act y Assert
            // viajes 1-29: tarifa normal
            for (int i = 1; i <= 29; i++)
            {
                Boleto b = colectivo.PagarCon(tarjeta);
                Assert.AreEqual(tarifa, b.ImportePagado, $"Viaje {i} deberia ser {tarifa}");
            }

            // viajes 30-59: 20% descuento
            decimal tarifa20desc = tarifa * 0.80m;
            for (int i = 30; i <= 59; i++)
            {
                Boleto b = colectivo.PagarCon(tarjeta);
                Assert.AreEqual(tarifa20desc, b.ImportePagado, $"Viaje {i} deberia ser {tarifa20desc}");
            }

            // viajes 60-80: 25% descuento
            decimal tarifa25desc = tarifa * 0.75m;
            for (int i = 60; i <= 80; i++)
            {
                Boleto b = colectivo.PagarCon(tarjeta);
                Assert.AreEqual(tarifa25desc, b.ImportePagado, $"Viaje {i} deberia ser {tarifa25desc}");
            }

            // viaje 81: vuelve a normal
            Boleto b81 = colectivo.PagarCon(tarjeta);
            Assert.AreEqual(tarifa, b81.ImportePagado);
        }

        [Test]
        public void Tarjeta_ContadorViajes_FuncionaCorrectamente()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.CargarSaldo(30000);
            Colectivo colectivo = new Colectivo("G");

            // Act y Assert
            Assert.AreEqual(0, tarjeta.ObtenerViajesDelMes());

            colectivo.PagarCon(tarjeta);
            Assert.AreEqual(1, tarjeta.ObtenerViajesDelMes());

            colectivo.PagarCon(tarjeta);
            Assert.AreEqual(2, tarjeta.ObtenerViajesDelMes());

            for (int i = 0; i < 10; i++)
            {
                colectivo.PagarCon(tarjeta);
            }
            Assert.AreEqual(12, tarjeta.ObtenerViajesDelMes());
        }

        [Test]
        public void MedioBoleto_NoAplicaUsoFrecuente()
        {
            // Arrange
            TarjetaMedioBoleto medio = new TarjetaMedioBoleto();
            medio.CargarSaldo(30000);
            medio.CargarSaldo(30000);
            Colectivo colectivo = new Colectivo("102");

            decimal tarifaMedioBoleto = colectivo.ObtenerValorPasaje() / 2;

            // hacer muchos viajes
            for (int i = 0; i < 50; i++)
            {
                colectivo.PagarCon(medio);
            }

            // Act - el viaje 51 deberia seguir siendo medio boleto normal
            Boleto boleto = colectivo.PagarCon(medio);

            // Assert
            Assert.AreEqual(tarifaMedioBoleto, boleto.ImportePagado); // siempre mitad, no descuento adicional
        }

        [Test]
        public void BoletoGratuito_NoAplicaUsoFrecuente()
        {
            // Arrange
            TarjetaBoletoGratuito gratuito = new TarjetaBoletoGratuito();
            gratuito.CargarSaldo(20000);
            gratuito.CargarSaldo(30000);
            Colectivo colectivo = new Colectivo("K");

            decimal tarifa = colectivo.ObtenerValorPasaje();

            // hacer varios viajes
            for (int i = 0; i < 40; i++)
            {
                colectivo.PagarCon(gratuito);
            }

            // Act - verificar saldo
            decimal saldo = gratuito.ObtenerSaldo();

            // Assert - los dos primeros gratis, el resto a precio completo sin descuento
            decimal gastoEsperado = 38 * tarifa; // 40 viajes - 2 gratis = 38 * tarifa
            Assert.AreEqual(50000 - gastoEsperado, saldo);
        }

        [Test]
        public void Tarjeta_UsoFrecuente_ConColectivoInterurbano()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta();
            for (int i = 0; i < 10; i++)
            {
                tarjeta.CargarSaldo(30000);
            }

            Colectivo interurbano = new Colectivo("Gálvez", true);
            decimal tarifa = interurbano.ObtenerValorPasaje(); // 3000

            // hacer 29 viajes
            for (int i = 0; i < 29; i++)
            {
                interurbano.PagarCon(tarjeta);
            }

            decimal saldoAntes = tarjeta.ObtenerSaldo();

            // Act - viaje 30 con 20% descuento
            Boleto boleto30 = interurbano.PagarCon(tarjeta);

            // Assert
            decimal montoEsperado = tarifa * 0.80m; // 20% descuento sobre 3000 = 2400
            Assert.AreEqual(montoEsperado, boleto30.ImportePagado);
            Assert.AreEqual(saldoAntes - montoEsperado, tarjeta.ObtenerSaldo());
        }
    }
}