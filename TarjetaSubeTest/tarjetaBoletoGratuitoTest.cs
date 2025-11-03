using NUnit.Framework;
using TarjetaSube;

namespace TarjetaSube.Tests
{
    [TestFixture]
    public class TarjetaBoletoGratuitoTests
    {
        private TarjetaBoletoGratuito tarjeta;
        private Colectivo colectivo;

        [SetUp]
        public void Setup()
        {
            tarjeta = new TarjetaBoletoGratuito();
            colectivo = new Colectivo("K");
        }

        [Test]
        public void CalcularDescuento_SiempreDevuelveCero()
        {
            decimal descuento = tarjeta.CalcularDescuento(1580);
            Assert.AreEqual(0, descuento);
        }

        [Test]
        public void CalcularDescuento_ConCualquierMonto_DevuelveCero()
        {
            Assert.AreEqual(0, tarjeta.CalcularDescuento(100));
            Assert.AreEqual(0, tarjeta.CalcularDescuento(5000));
            Assert.AreEqual(0, tarjeta.CalcularDescuento(99999));
        }

        [Test]
        public void PagarCon_SinCargarSaldo_GeneraBoleto()
        {
            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, boleto.ImportePagado);
        }

        [Test]
        public void PagarCon_VariosViajes_NuncaDescuenta()
        {
            tarjeta.CargarSaldo(5000);
            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);
            Assert.AreEqual(5000, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void BoletoGratuito_HeredaDeTarjeta()
        {
            Assert.IsInstanceOf<Tarjeta>(tarjeta);
        }

        [Test]
        public void PagarCon_ConSaldoNegativo_SigueFuncionando()
        {
            tarjeta.DescontarSaldo(500);
            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, boleto.ImportePagado);
            Assert.AreEqual(-500, tarjeta.ObtenerSaldo());
        }
    }
}
