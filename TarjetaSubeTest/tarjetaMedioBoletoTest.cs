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
            colectivo.PagarCon(tarjeta);
            Assert.AreEqual(4210, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void PagarCon_ImportePagadoEsMitad()
        {
            tarjeta.CargarSaldo(3000);
            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.AreEqual(790, boleto.ImportePagado);
        }

        [Test]
        public void MedioBoleto_HeredaDeTarjeta()
        {
            Assert.IsInstanceOf<Tarjeta>(tarjeta);
        }

        [Test]
        public void PagarCon_SinSaldo_UsaSaldoNegativo()
        {
            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(-790, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void PagarCon_ConSaldoInsuficiente_PuedeUsarHasta1200Negativo()
        {
            tarjeta.CargarSaldo(2000);
            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);
            Assert.AreEqual(-370, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void PagarCon_ExcedeLimiteNegativo_DevuelveNull()
        {
            Boleto b1 = colectivo.PagarCon(tarjeta);
            Boleto b2 = colectivo.PagarCon(tarjeta);
            Boleto b3 = colectivo.PagarCon(tarjeta);
            Assert.IsNull(b3);
        }
    }
}