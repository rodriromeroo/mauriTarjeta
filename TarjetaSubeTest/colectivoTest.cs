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
            decimal saldoInicial = tarjeta.ObtenerSaldo();
            decimal tarifaEsperada = colectivo.ObtenerValorPasaje();

            colectivo.PagarCon(tarjeta);

            Assert.AreEqual(saldoInicial - tarifaEsperada, tarjeta.ObtenerSaldo());
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
            decimal tarifaEsperada = colectivo.ObtenerValorPasaje();

            Boleto boleto = colectivo.PagarCon(tarjeta);

            Assert.AreEqual(tarifaEsperada, boleto.ImportePagado);
        }

        [Test]
        public void PagarCon_ConSaldoNegativoPermitido()
        {
            tarjeta.CargarSaldo(2000);
            decimal tarifa = colectivo.ObtenerValorPasaje();

            colectivo.PagarCon(tarjeta);
            Boleto boleto2 = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(boleto2);
            Assert.AreEqual(2000 - (tarifa * 2), tarjeta.ObtenerSaldo());
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

        [Test]
        public void PagarCon_TarjetaMedioBoleto_DescuentaMitad()
        {
            TarjetaMedioBoleto medio = new TarjetaMedioBoleto();
            medio.CargarSaldo(5000);
            decimal saldoInicial = medio.ObtenerSaldo();
            decimal tarifaEsperada = colectivo.ObtenerValorPasaje() / 2;

            colectivo.PagarCon(medio);

            Assert.AreEqual(saldoInicial - tarifaEsperada, medio.ObtenerSaldo());
        }

        [Test]
        public void PagarCon_TarjetaBoletoGratuito_NoDescuenta()
        {
            TarjetaBoletoGratuito gratuito = new TarjetaBoletoGratuito();
            gratuito.CargarSaldo(5000);
            colectivo.PagarCon(gratuito);
            Assert.AreEqual(5000, gratuito.ObtenerSaldo());
        }

        [Test]
        public void PagarCon_TarjetaFranquiciaCompleta_SiempreGeneraBoleto()
        {
            TarjetaFranquiciaCompleta franquicia = new TarjetaFranquiciaCompleta();
            Boleto b1 = colectivo.PagarCon(franquicia);
            Boleto b2 = colectivo.PagarCon(franquicia);
            Boleto b3 = colectivo.PagarCon(franquicia);

            Assert.IsNotNull(b1);
            Assert.IsNotNull(b2);
            Assert.IsNotNull(b3);
        }

        [Test]
        public void ObtenerLinea_ConLineaLarga_DevuelveCompleta()
        {
            Colectivo coleLineaLarga = new Colectivo("Línea Panorámica del Centro");
            Assert.AreEqual("Línea Panorámica del Centro", coleLineaLarga.ObtenerLinea());
        }

        [Test]
        public void PagarCon_VariasTarjetasDiferentes_FuncionaCorrectamente()
        {
            Tarjeta normal = new Tarjeta();
            TarjetaMedioBoleto medio = new TarjetaMedioBoleto();
            TarjetaBoletoGratuito gratuito = new TarjetaBoletoGratuito();

            normal.CargarSaldo(5000);
            medio.CargarSaldo(5000);
            gratuito.CargarSaldo(5000);

            decimal tarifa = colectivo.ObtenerValorPasaje();

            Boleto b1 = colectivo.PagarCon(normal);
            Boleto b2 = colectivo.PagarCon(medio);
            Boleto b3 = colectivo.PagarCon(gratuito);

            Assert.AreEqual(tarifa, b1.ImportePagado);
            Assert.AreEqual(tarifa / 2, b2.ImportePagado);
            Assert.AreEqual(0, b3.ImportePagado);
        }

        [Test]
        public void PagarCon_BoletoTieneLineaCorrecta()
        {
            Colectivo cole102 = new Colectivo("102");
            Tarjeta t = new Tarjeta();
            t.CargarSaldo(5000);

            Boleto boleto = cole102.PagarCon(t);
            Assert.AreEqual("102", boleto.LineaColectivo);
        }

        [Test]
        public void ColectivoUrbano_TieneTarifa1580()
        {
            Colectivo urbano = new Colectivo("K");
            Assert.AreEqual(1580, urbano.ObtenerValorPasaje());
            Assert.IsFalse(urbano.EsInterurbano());
        }
    }
}