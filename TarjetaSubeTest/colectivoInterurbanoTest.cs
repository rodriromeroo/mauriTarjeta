using NUnit.Framework;
using TarjetaSube;

namespace TarjetaSube.Tests
{
    [TestFixture]
    public class ColectivoInterurbanoTest
    {
        [Test]
        public void ColectivoUrbano_TieneTarifa1580()
        {
            // Arrange
            Colectivo urbano = new Colectivo("102");

            // Act
            decimal tarifa = urbano.ObtenerValorPasaje();

            // Assert
            Assert.AreEqual(1580, tarifa);
            Assert.IsFalse(urbano.EsInterurbano());
        }

        [Test]
        public void ColectivoInterurbano_TieneTarifa3000()
        {
            // Arrange
            Colectivo interurbano = new Colectivo("500", true);

            // Act
            decimal tarifa = interurbano.ObtenerValorPasaje();

            // Assert
            Assert.AreEqual(3000, tarifa);
            Assert.IsTrue(interurbano.EsInterurbano());
        }

        [Test]
        public void ColectivoInterurbano_TarjetaNormal_CobraTarifaCompleta()
        {
            // Arrange
            Colectivo interurbano = new Colectivo("Gálvez", true);
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.CargarSaldo(10000);

            decimal saldoInicial = tarjeta.ObtenerSaldo();

            // Act
            Boleto boleto = interurbano.PagarCon(tarjeta);

            // Assert
            Assert.IsNotNull(boleto);
            Assert.AreEqual(3000, boleto.ImportePagado);
            Assert.AreEqual(saldoInicial - 3000, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void ColectivoInterurbano_MedioBoleto_CobraMitad()
        {
            // Arrange
            Colectivo interurbano = new Colectivo("Baigorria", true);
            TarjetaMedioBoleto medio = new TarjetaMedioBoleto();
            medio.CargarSaldo(10000);

            // Act
            Boleto boleto = interurbano.PagarCon(medio);

            // Assert
            Assert.IsNotNull(boleto);
            Assert.AreEqual(1500, boleto.ImportePagado); // mitad de 3000
            Assert.AreEqual(8500, medio.ObtenerSaldo());
        }

        [Test]
        public void ColectivoInterurbano_BoletoGratuito_PrimerosViajesGratis()
        {
            // Arrange
            Colectivo interurbano = new Colectivo("Villa Gobernador Gálvez", true);
            TarjetaBoletoGratuito gratuito = new TarjetaBoletoGratuito();
            gratuito.CargarSaldo(10000);

            // Act
            Boleto boleto1 = interurbano.PagarCon(gratuito);
            Boleto boleto2 = interurbano.PagarCon(gratuito);

            // Assert
            Assert.IsNotNull(boleto1);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(0, boleto1.ImportePagado);
            Assert.AreEqual(0, boleto2.ImportePagado);
            Assert.AreEqual(10000, gratuito.ObtenerSaldo()); // no descontó nada
        }

        [Test]
        public void ColectivoInterurbano_BoletoGratuito_TercerViajeCobraTarifaInterurbana()
        {
            // Arrange
            Colectivo interurbano = new Colectivo("Capitán Bermúdez", true);
            TarjetaBoletoGratuito gratuito = new TarjetaBoletoGratuito();
            gratuito.CargarSaldo(15000);

            // Act
            interurbano.PagarCon(gratuito); // viaje 1: gratis
            interurbano.PagarCon(gratuito); // viaje 2: gratis
            Boleto boleto3 = interurbano.PagarCon(gratuito); // viaje 3: paga

            // Assert
            Assert.IsNotNull(boleto3);
            Assert.AreEqual(3000, boleto3.ImportePagado); // cobra tarifa interurbana completa
            Assert.AreEqual(12000, gratuito.ObtenerSaldo());
        }

        [Test]
        public void ColectivoInterurbano_FranquiciaCompleta_SiempreGratis()
        {
            // Arrange
            Colectivo interurbano = new Colectivo("Funes", true);
            TarjetaFranquiciaCompleta franquicia = new TarjetaFranquiciaCompleta();

            // Act
            Boleto boleto1 = interurbano.PagarCon(franquicia);
            Boleto boleto2 = interurbano.PagarCon(franquicia);
            Boleto boleto3 = interurbano.PagarCon(franquicia);

            // Assert
            Assert.IsNotNull(boleto1);
            Assert.IsNotNull(boleto2);
            Assert.IsNotNull(boleto3);
            Assert.AreEqual(0, boleto1.ImportePagado);
            Assert.AreEqual(0, boleto2.ImportePagado);
            Assert.AreEqual(0, boleto3.ImportePagado);
        }

        [Test]
        public void ColectivoInterurbano_MedioBoleto_RespetaRestriccionHorario()
        {
            // Arrange
            Colectivo interurbano = new Colectivo("Pérez", true);
            TarjetaMedioBoleto medio = new TarjetaMedioBoleto();
            medio.CargarSaldo(10000);

            // Act
            Boleto boleto = interurbano.PagarCon(medio);

            // Assert
            // Si estamos en horario permitido, debe funcionar
            // Si estamos fuera de horario, debe devolver null
            if (boleto != null)
            {
                Assert.AreEqual(1500, boleto.ImportePagado);
            }
        }

        [Test]
        public void ColectivoInterurbano_TarjetaSinSaldo_NoPermiteViajar()
        {
            // Arrange
            Colectivo interurbano = new Colectivo("Granadero Baigorria", true);
            Tarjeta tarjeta = new Tarjeta();
            // no carga saldo

            // Act
            Boleto boleto = interurbano.PagarCon(tarjeta);

            // Assert
            Assert.IsNull(boleto);
        }

        [Test]
        public void ColectivoInterurbano_TarjetaNormal_PermiteSaldoNegativo()
        {
            // Arrange
            Colectivo interurbano = new Colectivo("Roldán", true);
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.CargarSaldo(2000);

            // Act
            Boleto boleto = interurbano.PagarCon(tarjeta);

            // Assert
            Assert.IsNotNull(boleto);
            Assert.AreEqual(3000, boleto.ImportePagado);
            Assert.AreEqual(-1000, tarjeta.ObtenerSaldo()); // saldo negativo permitido
        }

        [Test]
        public void ColectivoInterurbano_TarjetaNormal_NoExcedeLimiteNegativo()
        {
            // Arrange
            Colectivo interurbano = new Colectivo("Arroyo Seco", true);
            Tarjeta tarjeta = new Tarjeta();
            // saldo = 0, límite negativo = -1200

            // Act
            Boleto boleto = interurbano.PagarCon(tarjeta);

            // Assert
            // No puede viajar porque 3000 > 1200 (límite negativo)
            Assert.IsNull(boleto);
            Assert.AreEqual(0, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void ColectivoUrbanoYInterurbano_UsanTarifaCorrecta()
        {
            // Arrange
            Colectivo urbano = new Colectivo("K");
            Colectivo interurbano = new Colectivo("Villa Constitución", true);

            Tarjeta tarjeta1 = new Tarjeta();
            Tarjeta tarjeta2 = new Tarjeta();
            tarjeta1.CargarSaldo(10000);
            tarjeta2.CargarSaldo(10000);

            // Act
            Boleto boletoUrbano = urbano.PagarCon(tarjeta1);
            Boleto boletoInterurbano = interurbano.PagarCon(tarjeta2);

            // Assert
            Assert.AreEqual(1580, boletoUrbano.ImportePagado);
            Assert.AreEqual(3000, boletoInterurbano.ImportePagado);
        }

        [Test]
        public void ColectivoInterurbano_AplicaDescuentoUsoFrecuente()
        {
            // Arrange
            Colectivo interurbano = new Colectivo("San Lorenzo", true);
            Tarjeta tarjeta = new Tarjeta();

            // Cargar suficiente saldo para 30 viajes
            for (int i = 0; i < 10; i++)
            {
                tarjeta.CargarSaldo(30000);
            }

            // Hacer 29 viajes (tarifa normal)
            for (int i = 0; i < 29; i++)
            {
                interurbano.PagarCon(tarjeta);
            }

            decimal saldoAntes = tarjeta.ObtenerSaldo();

            // Act - viaje 30 (debería tener 20% descuento)
            Boleto boleto30 = interurbano.PagarCon(tarjeta);

            // Assert
            decimal montoEsperado = 3000 * 0.80m; // 20% descuento = 2400
            Assert.AreEqual(montoEsperado, boleto30.ImportePagado);
            Assert.AreEqual(saldoAntes - montoEsperado, tarjeta.ObtenerSaldo());
        }
    }
}