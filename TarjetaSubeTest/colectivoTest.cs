using NUnit.Framework;
using System;
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

        [Test]
        public void PagarCon_TarjetaMedioBoleto_DescuentaMitad()
        {
            TarjetaMedioBoleto medio = new TarjetaMedioBoleto();
            medio.CargarSaldo(5000);
            colectivo.PagarCon(medio);
            Assert.AreEqual(4210, medio.ObtenerSaldo());
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

            Boleto b1 = colectivo.PagarCon(normal);
            Boleto b2 = colectivo.PagarCon(medio);
            Boleto b3 = colectivo.PagarCon(gratuito);

            Assert.AreEqual(1580, b1.ImportePagado);
            Assert.AreEqual(790, b2.ImportePagado);
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
        public void PagarCon_Trasbordo_NoCobraNada()
        {
            tarjeta.CargarSaldo(5000);

            DateTime tiempo1 = new DateTime(2024, 11, 20, 10, 0, 0);
            Colectivo cole1 = new Colectivo("120");
            Boleto boleto1 = cole1.PagarCon(tarjeta, tiempo1);

            decimal saldoDespuesPrimero = tarjeta.ObtenerSaldo();

            DateTime tiempo2 = tiempo1.AddMinutes(30);
            Colectivo cole2 = new Colectivo("144");
            Boleto boleto2 = cole2.PagarCon(tarjeta, tiempo2);

            Assert.IsTrue(boleto2.EsTransbordo);
            Assert.AreEqual(0, boleto2.ImportePagado);
            Assert.AreEqual(saldoDespuesPrimero, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void PagarCon_Trasbordo_MismaLinea_CobraNormal()
        {
            tarjeta.CargarSaldo(10000);

            DateTime tiempo1 = new DateTime(2024, 11, 20, 10, 0, 0);
            Boleto boleto1 = colectivo.PagarCon(tarjeta, tiempo1);
            decimal saldoDespues1 = tarjeta.ObtenerSaldo();

            DateTime tiempo2 = tiempo1.AddMinutes(30);
            Boleto boleto2 = colectivo.PagarCon(tarjeta, tiempo2);

            Assert.IsFalse(boleto2.EsTransbordo);
            Assert.AreEqual(1580, boleto2.ImportePagado);
        }

        [Test]
        public void PagarCon_TrasbordomasDe1Hora_CobraNormal()
        {
            tarjeta.CargarSaldo(10000);

            DateTime tiempo1 = new DateTime(2024, 11, 20, 10, 0, 0);
            Colectivo cole1 = new Colectivo("120");
            cole1.PagarCon(tarjeta, tiempo1);

            DateTime tiempo2 = tiempo1.AddHours(2);
            Colectivo cole2 = new Colectivo("144");
            Boleto boleto2 = cole2.PagarCon(tarjeta, tiempo2);

            Assert.IsFalse(boleto2.EsTransbordo);
            Assert.AreEqual(1580, boleto2.ImportePagado);
        }

        [Test]
        public void PagarCon_MedioBoleto_PuedeTrasbordar()
        {
            TarjetaMedioBoleto medio = new TarjetaMedioBoleto();
            medio.CargarSaldo(5000);

            DateTime tiempo1 = new DateTime(2024, 11, 20, 10, 0, 0);
            Colectivo cole1 = new Colectivo("120");
            Boleto b1 = cole1.PagarCon(medio, tiempo1);

            DateTime tiempo2 = tiempo1.AddMinutes(30);
            Colectivo cole2 = new Colectivo("144");
            Boleto b2 = cole2.PagarCon(medio, tiempo2);

            Assert.IsTrue(b2.EsTransbordo);
            Assert.AreEqual(0, b2.ImportePagado);
        }

        [Test]
        public void PagarCon_BoletoGratuito_PuedeTrasbordar()
        {
            TarjetaBoletoGratuito gratuito = new TarjetaBoletoGratuito();

            DateTime tiempo1 = new DateTime(2024, 11, 20, 10, 0, 0);
            Colectivo cole1 = new Colectivo("K");
            Boleto b1 = cole1.PagarCon(gratuito, tiempo1);

            DateTime tiempo2 = tiempo1.AddMinutes(30);
            Colectivo cole2 = new Colectivo("G");
            Boleto b2 = cole2.PagarCon(gratuito, tiempo2);

            Assert.IsTrue(b2.EsTransbordo);
        }

        [Test]
        public void PagarCon_FranquiciaCompleta_PuedeTrasbordar()
        {
            TarjetaFranquiciaCompleta franquicia = new TarjetaFranquiciaCompleta();

            DateTime tiempo1 = new DateTime(2024, 11, 20, 10, 0, 0);
            Colectivo cole1 = new Colectivo("102");
            Boleto b1 = cole1.PagarCon(franquicia, tiempo1);

            DateTime tiempo2 = tiempo1.AddMinutes(30);
            Colectivo cole2 = new Colectivo("144");
            Boleto b2 = cole2.PagarCon(franquicia, tiempo2);

            Assert.IsTrue(b2.EsTransbordo);
        }
    }
}