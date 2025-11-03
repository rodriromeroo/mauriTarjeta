using NUnit.Framework;
using System;
using TarjetaSube;

namespace TarjetaSube.Tests
{
    [TestFixture]
    public class BoletoTests
    {
        [Test]
        public void Constructor_AsignaTodosCamposCorrectamente()
        {
            Boleto boleto = new Boleto("120", 1580, 5000, "Normal", "ABC123", 1580);
            Assert.AreEqual("120", boleto.LineaColectivo);
            Assert.AreEqual(1580, boleto.ImportePagado);
            Assert.AreEqual(5000, boleto.SaldoRestante);
            Assert.AreEqual("Normal", boleto.TipoTarjeta);
            Assert.AreEqual("ABC123", boleto.IDTarjeta);
            Assert.AreEqual(1580, boleto.TotalAbonado);
        }

        [Test]
        public void Constructor_ConTarjetaMedioBoleto()
        {
            Boleto boleto = new Boleto("K", 790, 4210, "Medio Boleto", "MB001", 790);
            Assert.AreEqual("Medio Boleto", boleto.TipoTarjeta);
            Assert.AreEqual(790, boleto.ImportePagado);
        }

        [Test]
        public void Constructor_ConFranquiciaCompleta()
        {
            Boleto boleto = new Boleto("102", 0, 0, "Franquicia Completa", "FC001", 0);
            Assert.AreEqual("Franquicia Completa", boleto.TipoTarjeta);
            Assert.AreEqual(0, boleto.ImportePagado);
        }

        [Test]
        public void Constructor_ConBoletoGratuito()
        {
            Boleto boleto = new Boleto("G", 0, 5000, "Boleto Gratuito", "BG001", 0);
            Assert.AreEqual("Boleto Gratuito", boleto.TipoTarjeta);
            Assert.AreEqual(0, boleto.TotalAbonado);
        }

        [Test]
        public void Constructor_AsignaFechaHora()
        {
            DateTime antes = DateTime.Now;
            Boleto boleto = new Boleto("133", 1580, 3420, "Normal", "N001", 1580);
            DateTime despues = DateTime.Now;
            Assert.GreaterOrEqual(boleto.FechaHora, antes);
            Assert.LessOrEqual(boleto.FechaHora, despues);
        }

        [Test]
        public void MostrarInformacion_NoLanzaExcepcion()
        {
            Boleto boleto = new Boleto("144", 1580, 10000, "Normal", "TEST01", 1580);
            Assert.DoesNotThrow(() => boleto.MostrarInformacion());
        }

        [Test]
        public void Constructor_ConSaldoNegativo_FuncionaBien()
        {
            Boleto boleto = new Boleto("K", 1580, -500, "Normal", "NEG01", 1580);
            Assert.AreEqual(-500, boleto.SaldoRestante);
        }

        [Test]
        public void PropiedadIDTarjeta_EsPublica()
        {
            Boleto boleto = new Boleto("102", 1580, 5000, "Normal", "ID123", 1580);
            string id = boleto.IDTarjeta;
            Assert.IsNotNull(id);
            Assert.AreEqual("ID123", id);
        }

        [Test]
        public void PropiedadTipoTarjeta_EsPublica()
        {
            Boleto boleto = new Boleto("144", 790, 4000, "Medio Boleto", "MB01", 790);
            string tipo = boleto.TipoTarjeta;
            Assert.AreEqual("Medio Boleto", tipo);
        }

        [Test]
        public void PropiedadTotalAbonado_EsPublica()
        {
            Boleto boleto = new Boleto("K", 1580, 3000, "Normal", "N01", 1580);
            decimal total = boleto.TotalAbonado;
            Assert.AreEqual(1580, total);
        }
    }
}