using NUnit.Framework;
using TarjetaSube;

namespace TarjetaSube.Tests
{
    [TestFixture]
    public class TarjetaFranquiciaCompletaTests
    {
        private TarjetaFranquiciaCompleta tarjeta;
        private Colectivo colectivo;

        [SetUp]
        public void Setup()
        {
            tarjeta = new TarjetaFranquiciaCompleta();
            colectivo = new Colectivo("102");
        }

        [Test]
        public void CalcularDescuento_SiempreDevuelveCero()
        {
            decimal descuento = tarjeta.CalcularDescuento(1580);
            Assert.AreEqual(0, descuento);
        }

        [Test]
        public void CalcularDescuento_ConCualquierMontoDevuelveCero()
        {
            Assert.AreEqual(0, tarjeta.CalcularDescuento(100));
            Assert.AreEqual(0, tarjeta.CalcularDescuento(5000));
            Assert.AreEqual(0, tarjeta.CalcularDescuento(999999));
        }

        [Test]
        public void DescontarSaldo_SiempreRetornaTrue()
        {
            Assert.IsTrue(tarjeta.DescontarSaldo(1000));
            Assert.IsTrue(tarjeta.DescontarSaldo(50000));
            Assert.IsTrue(tarjeta.DescontarSaldo(999999));
        }

        [Test]
        public void DescontarSaldo_NoModificaSaldo()
        {
            decimal saldoInicial = tarjeta.ObtenerSaldo();
            tarjeta.DescontarSaldo(10000);
            Assert.AreEqual(saldoInicial, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void PagarCon_SinSaldo_GeneraBoleto()
        {
            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto);
        }

        [Test]
        public void PagarCon_ImportePagadoSiempreCero()
        {
            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.AreEqual(0, boleto.ImportePagado);
        }

        [Test]
        public void PagarCon_MultiplesViajes_SiempreGeneraBoleto()
        {
            for (int i = 0; i < 100; i++)
            {
                Boleto boleto = colectivo.PagarCon(tarjeta);
                Assert.IsNotNull(boleto);
            }
        }

        [Test]
        public void FranquiciaCompleta_HeredaDeTarjeta()
        {
            Assert.IsInstanceOf<Tarjeta>(tarjeta);
        }

        [Test]
        public void PagarCon_ConSaldoCargado_NoModificaSaldo()
        {
            tarjeta.CargarSaldo(10000);
            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);
            Assert.AreEqual(10000, tarjeta.ObtenerSaldo());
        }
    }
}