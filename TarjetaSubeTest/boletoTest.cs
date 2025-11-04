using NUnit.Framework;
using System;
using TarjetaSube;

namespace TarjetaSube.Tests
{
    [TestFixture]
    public class BoletoTests
    {
        [Test]
        public void Constructor_AsignaLineaCorrectamente()
        {
            Boleto boleto = new Boleto("120", 1580, 5000, false);
            Assert.AreEqual("120", boleto.LineaColectivo);
        }

        [Test]
        public void Constructor_AsignaImporteCorrectamente()
        {
            Boleto boleto = new Boleto("K", 1580, 3420, false);
            Assert.AreEqual(1580, boleto.ImportePagado);
        }

        [Test]
        public void Constructor_AsignaSaldoRestanteCorrectamente()
        {
            Boleto boleto = new Boleto("133", 1580, 8420, false);
            Assert.AreEqual(8420, boleto.SaldoRestante);
        }

        [Test]
        public void Constructor_AsignaFechaHora()
        {
            DateTime antes = DateTime.Now;
            Boleto boleto = new Boleto("G", 1580, 2000, false);
            DateTime despues = DateTime.Now;
            Assert.GreaterOrEqual(boleto.FechaHora, antes);
            Assert.LessOrEqual(boleto.FechaHora, despues);
        }

        [Test]
        public void MostrarInformacion_NoLanzaExcepcion()
        {
            Boleto boleto = new Boleto("144", 1580, 10000, false);
            Assert.DoesNotThrow(() => boleto.MostrarInformacion());
        }

        [Test]
        public void PropiedadLineaColectivo_EsPublica()
        {
            Boleto boleto = new Boleto("Panorámico", 1580, 500, false);
            string linea = boleto.LineaColectivo;
            Assert.IsNotNull(linea);
        }

        [Test]
        public void PropiedadImportePagado_EsPublica()
        {
            Boleto boleto = new Boleto("Ñ", 1580, 0, false);
            decimal importe = boleto.ImportePagado;
            Assert.AreEqual(1580, importe);
        }

        [Test]
        public void PropiedadSaldoRestante_EsPublica()
        {
            Boleto boleto = new Boleto("142", 1580, 38420, false);
            decimal saldo = boleto.SaldoRestante;
            Assert.AreEqual(38420, saldo);
        }

        [Test]
        public void PropiedadFechaHora_EsPublica()
        {
            Boleto boleto = new Boleto("101", 1580, 15000, false);
            DateTime fecha = boleto.FechaHora;
            Assert.IsNotNull(fecha);
        }

        [Test]
        public void Constructor_ConSaldoCero_FuncionaBien()
        {
            Boleto boleto = new Boleto("27", 1580, 0, false);
            Assert.AreEqual(0, boleto.SaldoRestante);
        }

        [Test]
        public void Constructor_ConSaldoNegativo_FuncionaBien()
        {
            Boleto boleto = new Boleto("K", 1580, -500, false);
            Assert.AreEqual(-500, boleto.SaldoRestante);
        }

        [Test]
        public void Constructor_ConImporteCero_FuncionaBien()
        {
            Boleto boleto = new Boleto("102", 0, 5000, false);
            Assert.AreEqual(0, boleto.ImportePagado);
        }

        [Test]
        public void Constructor_ConLineaVacia_FuncionaBien()
        {
            Boleto boleto = new Boleto("", 1580, 3000, false);
            Assert.AreEqual("", boleto.LineaColectivo);
        }

        [Test]
        public void Boleto_PropiedadesNoModificables()
        {
            Boleto boleto = new Boleto("144", 1580, 5000, false);
            string linea = boleto.LineaColectivo;
            decimal importe = boleto.ImportePagado;
            decimal saldo = boleto.SaldoRestante;
            DateTime fecha = boleto.FechaHora;

            Assert.IsNotNull(linea);
            Assert.Greater(importe, -1);
            Assert.IsNotNull(fecha);
        }

        [Test]
        public void Constructor_ConEsTransbordoTrue_AsignaCorrectamente()
        {
            Boleto boleto = new Boleto("120", 0, 5000, true);
            Assert.AreEqual(true, boleto.EsTransbordo);
        }

        [Test]
        public void Constructor_ConEsTransbordoFalse_AsignaCorrectamente()
        {
            Boleto boleto = new Boleto("K", 1580, 3420, false);
            Assert.AreEqual(false, boleto.EsTransbordo);
        }

        [Test]
        public void BoletoTransbordo_ImportePagadoCero()
        {
            Boleto boleto = new Boleto("144", 0, 5000, true);
            Assert.AreEqual(0, boleto.ImportePagado);
            Assert.IsTrue(boleto.EsTransbordo);
        }
    }
}