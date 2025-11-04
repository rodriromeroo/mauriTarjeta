using NUnit.Framework;
using TarjetaSube;

namespace TarjetaSube.Tests
{
    [TestFixture]
    public class TarjetaMedioBoletoTests
    {
        private TarjetaMedioBoleto tarjeta;
        private Colectivo colectivo;

        [SetUp]
        public void Setup()
        {
            tarjeta = new TarjetaMedioBoleto();
            colectivo = new Colectivo("144");
        }

        [Test]
        public void CalcularDescuento_DevuelveMitad()
        {
            decimal descuento = tarjeta.CalcularDescuento(1580);
            Assert.AreEqual(790, descuento);
        }

        [Test]
        public void CalcularDescuento_ConDiferentesMontosDevuelveMitad()
        {
            Assert.AreEqual(50, tarjeta.CalcularDescuento(100));
            Assert.AreEqual(250, tarjeta.CalcularDescuento(500));
            Assert.AreEqual(1000, tarjeta.CalcularDescuento(2000));
        }

        [Test]
        public void PagarCon_DescuentaSoloMitad()
        {
            tarjeta.CargarSaldo(5000);
            decimal saldoInicial = tarjeta.ObtenerSaldo();
            decimal tarifaMedioBoleto = colectivo.ObtenerValorPasaje() / 2;

            colectivo.PagarCon(tarjeta);

            Assert.AreEqual(saldoInicial - tarifaMedioBoleto, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void PagarCon_ImportePagadoEsMitad()
        {
            tarjeta.CargarSaldo(3000);
            decimal tarifaEsperada = colectivo.ObtenerValorPasaje() / 2;

            Boleto boleto = colectivo.PagarCon(tarjeta);

            Assert.AreEqual(tarifaEsperada, boleto.ImportePagado);
        }

        [Test]
        public void MedioBoleto_HeredaDeTarjeta()
        {
            Assert.IsInstanceOf<Tarjeta>(tarjeta);
        }

        [Test]
        public void PagarCon_SinSaldo_UsaSaldoNegativo()
        {
            decimal tarifaMedioBoleto = colectivo.ObtenerValorPasaje() / 2;

            Boleto boleto = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(boleto);
            Assert.AreEqual(-tarifaMedioBoleto, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void PagarCon_ConSaldoInsuficiente_PuedeUsarHasta1200Negativo()
        {
            tarjeta.CargarSaldo(2000);
            decimal tarifaMedioBoleto = colectivo.ObtenerValorPasaje() / 2;

            colectivo.PagarCon(tarjeta); // viaje 1
            colectivo.PagarCon(tarjeta); // viaje 2
            colectivo.PagarCon(tarjeta); // viaje 3

            decimal saldoEsperado = 2000 - (tarifaMedioBoleto * 3);
            Assert.AreEqual(saldoEsperado, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void PagarCon_ExcedeLimiteNegativo_DevuelveNull()
        {
            // Con saldo 0 y limite -1200, solo puede hacer 1 viaje (790)
            // El segundo dejaria el saldo en -1580, que excede el limite
            Boleto b1 = colectivo.PagarCon(tarjeta);
            Boleto b2 = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(b1);
            Assert.IsNull(b2); // Este deberia fallar porque -1580 < -1200
        }

        [Test]
        public void MedioBoleto_ConColectivoInterurbano_CobraMitadDe3000()
        {
            Colectivo interurbano = new Colectivo("Gálvez", true);
            tarjeta.CargarSaldo(5000);

            Boleto boleto = interurbano.PagarCon(tarjeta);

            Assert.IsNotNull(boleto);
            Assert.AreEqual(1500, boleto.ImportePagado); // mitad de 3000
            Assert.AreEqual(3500, tarjeta.ObtenerSaldo());
        }
    }
}