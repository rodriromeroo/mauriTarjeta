using NUnit.Framework;
using TarjetaSube;

namespace TarjetaSube.Tests
{
    [TestFixture]
    public class ColectivoTests
    {
        private Colectivo colectivo;
        private Tarjeta tarjeta;

        [SetUp]
        public void Setup()
        {
            colectivo = new Colectivo("133");
            tarjeta = new Tarjeta();
        }

        [Test]
        public void ObtenerLinea_DevuelveLineaCorrecta()
        {
            string linea = colectivo.ObtenerLinea();
            Assert.AreEqual("133", linea);
        }

        [Test]
        public void PagarCon_TarjetaConSaldo_DevuelveBoleto()
        {
            tarjeta.CargarSaldo(5000);
            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto);
        }

        [Test]
        public void PagarCon_TarjetaSinSaldo_DevuelveNull()
        {
            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNull(boleto);
        }

        [Test]
        public void PagarCon_DescontaSaldoDeTarjeta()
        {
            tarjeta.CargarSaldo(5000);
            colectivo.PagarCon(tarjeta);
            Assert.AreEqual(3420, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void PagarCon_BoletoConLineaCorrecta()
        {
            tarjeta.CargarSaldo(5000);
            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.AreEqual("133", boleto.LineaColectivo);
        }

        [Test]
        public void PagarCon_BoletoConImporteCorrecto()
        {
            tarjeta.CargarSaldo(5000);
            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.AreEqual(1580, boleto.ImportePagado);
        }

        [Test]
        public void PagarCon_ConSaldoNegativoPermitido()
        {
            tarjeta.CargarSaldo(2000);
            colectivo.PagarCon(tarjeta);
            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(-1160, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void PagarCon_ExcedeLimiteNegativo_DevuelveNull()
        {
            tarjeta.CargarSaldo(2000);
            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);
            Boleto boleto3 = colectivo.PagarCon(tarjeta);
            Assert.IsNull(boleto3);
        }
    }
}