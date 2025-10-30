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
        public void ObtenerLinea_OtraLinea_DevuelveCorrectamente()
        {
            Colectivo otroColectivo = new Colectivo("K");
            string linea = otroColectivo.ObtenerLinea();
            Assert.AreEqual("K", linea);
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
        public void PagarCon_BoletoConSaldoRestante()
        {
            tarjeta.CargarSaldo(10000);
            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.AreEqual(8420, boleto.SaldoRestante);
        }

        [Test]
        public void PagarCon_VariosViajes_GeneraVariosBoletos()
        {
            tarjeta.CargarSaldo(10000);
            Boleto boleto1 = colectivo.PagarCon(tarjeta);
            Boleto boleto2 = colectivo.PagarCon(tarjeta);
            Boleto boleto3 = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto1);
            Assert.IsNotNull(boleto2);
            Assert.IsNotNull(boleto3);
            Assert.AreEqual(5260, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void PagarCon_SaldoJusto_GeneraBoleto()
        {
            tarjeta.CargarSaldo(2000);
            tarjeta.DescontarSaldo(420);
            Boleto boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, tarjeta.ObtenerSaldo());
        }
    }
}