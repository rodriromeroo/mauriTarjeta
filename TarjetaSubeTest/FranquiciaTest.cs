using NUnit.Framework;
using TarjetaSube;

namespace TarjetaSubeTest
{

    [TestFixture]
    public class FranquiciaTest
    {

        [Test]
        public void FranquiciaCompletaSiemprePuedePagar()
        {
            // Arrange
            TarjetaFranquiciaCompleta tarjeta = new TarjetaFranquiciaCompleta();
            Colectivo colectivo = new Colectivo("102");

            // Act
            Boleto primerViaje = colectivo.PagarCon(tarjeta);
            Boleto segundoViaje = colectivo.PagarCon(tarjeta);
            Boleto tercerViaje = colectivo.PagarCon(tarjeta);

            // Assert
            Assert.IsNotNull(primerViaje);
            Assert.IsNotNull(segundoViaje);
            Assert.IsNotNull(tercerViaje);

            decimal saldo = tarjeta.ObtenerSaldo();
            Assert.AreEqual(0, saldo);
        }

        [Test]
        public void MedioBoletoCobraMitadDelPasaje()
        {
            // Arrange
            TarjetaMedioBoleto tarjeta = new TarjetaMedioBoleto();
            tarjeta.CargarSaldo(5000);

            decimal saldoInicial = tarjeta.ObtenerSaldo();

            Colectivo colectivo = new Colectivo("144");
            decimal tarifaEsperada = colectivo.ObtenerValorPasaje() / 2;

            // Act
            Boleto boleto = colectivo.PagarCon(tarjeta);

            // Assert
            Assert.IsNotNull(boleto);

            decimal saldoFinal = tarjeta.ObtenerSaldo();
            decimal montoDescontado = saldoInicial - saldoFinal;

            Assert.AreEqual(tarifaEsperada, montoDescontado);
            Assert.AreEqual(tarifaEsperada, boleto.ImportePagado);
        }

        [Test]
        public void MedioBoletoSiempreCobraMitad()
        {
            // Arrange
            TarjetaMedioBoleto tarjeta = new TarjetaMedioBoleto();
            tarjeta.CargarSaldo(10000);

            Colectivo colectivo = new Colectivo("K");
            decimal tarifaMedioBoleto = colectivo.ObtenerValorPasaje() / 2;

            // Act
            decimal saldo1 = tarjeta.ObtenerSaldo();
            Boleto boleto1 = colectivo.PagarCon(tarjeta);
            decimal saldo2 = tarjeta.ObtenerSaldo();
            decimal descuento1 = saldo1 - saldo2;

            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            decimal saldo3 = tarjeta.ObtenerSaldo();
            decimal descuento2 = saldo2 - saldo3;

            Boleto boleto3 = colectivo.PagarCon(tarjeta);
            decimal saldo4 = tarjeta.ObtenerSaldo();
            decimal descuento3 = saldo3 - saldo4;

            // Assert
            Assert.AreEqual(tarifaMedioBoleto, descuento1);
            Assert.AreEqual(tarifaMedioBoleto, descuento2);
            Assert.AreEqual(tarifaMedioBoleto, descuento3);
            Assert.AreEqual(tarifaMedioBoleto, boleto1.ImportePagado);
            Assert.AreEqual(tarifaMedioBoleto, boleto2.ImportePagado);
            Assert.AreEqual(tarifaMedioBoleto, boleto3.ImportePagado);
        }

        [Test]
        public void BoletoGratuitoNoCubraSaldo()
        {
            // Arrange
            TarjetaBoletoGratuito tarjeta = new TarjetaBoletoGratuito();

            decimal saldoInicial = tarjeta.ObtenerSaldo();

            Colectivo colectivo = new Colectivo("102");

            // Act
            Boleto boleto = colectivo.PagarCon(tarjeta);

            // Assert
            Assert.IsNotNull(boleto);

            decimal saldoFinal = tarjeta.ObtenerSaldo();
            Assert.AreEqual(saldoInicial, saldoFinal);
            Assert.AreEqual(0, boleto.ImportePagado);
        }

        [Test]
        public void FranquiciaCompletaNoCubraSaldo()
        {
            // Arrange
            TarjetaFranquiciaCompleta tarjeta = new TarjetaFranquiciaCompleta();

            decimal saldoInicial = tarjeta.ObtenerSaldo();

            Colectivo colectivo = new Colectivo("144");

            // Act
            Boleto boleto = colectivo.PagarCon(tarjeta);

            // Assert
            Assert.IsNotNull(boleto);

            decimal saldoFinal = tarjeta.ObtenerSaldo();
            Assert.AreEqual(saldoInicial, saldoFinal);
            Assert.AreEqual(0, boleto.ImportePagado);
        }

        [Test]
        public void MedioBoleto_ConColectivoInterurbano_CobraMitad()
        {
            // Arrange
            TarjetaMedioBoleto tarjeta = new TarjetaMedioBoleto();
            tarjeta.CargarSaldo(10000);

            Colectivo interurbano = new Colectivo("Gálvez", true);
            decimal tarifaEsperada = interurbano.ObtenerValorPasaje() / 2; // 1500

            // Act
            Boleto boleto = interurbano.PagarCon(tarjeta);

            // Assert
            Assert.IsNotNull(boleto);
            Assert.AreEqual(tarifaEsperada, boleto.ImportePagado);
            Assert.AreEqual(10000 - tarifaEsperada, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void FranquiciaCompleta_ConColectivoInterurbano_Gratis()
        {
            // Arrange
            TarjetaFranquiciaCompleta tarjeta = new TarjetaFranquiciaCompleta();
            Colectivo interurbano = new Colectivo("Baigorria", true);

            // Act
            Boleto boleto = interurbano.PagarCon(tarjeta);

            // Assert
            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, boleto.ImportePagado);
        }
    }
}