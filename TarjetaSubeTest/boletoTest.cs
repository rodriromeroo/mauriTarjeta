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
            Boleto boleto = new Boleto("120", 1580, 5000);
            Assert.AreEqual("120", boleto.LineaColectivo);
        }

        [Test]
        public void Constructor_AsignaImporteCorrectamente()
        {
            Boleto boleto = new Boleto("K", 1580, 3420);
            Assert.AreEqual(1580, boleto.ImportePagado);
        }

        [Test]
        public void Constructor_AsignaSaldoRestanteCorrectamente()
        {
            Boleto boleto = new Boleto("133", 1580, 8420);
            Assert.AreEqual(8420, boleto.SaldoRestante);
        }

        [Test]
        public void Constructor_AsignaFechaHora()
        {
            DateTime antes = DateTime.Now;
            Boleto boleto = new Boleto("G", 1580, 2000);
            DateTime despues = DateTime.Now;
            Assert.GreaterOrEqual(boleto.FechaHora, antes);
            Assert.LessOrEqual(boleto.FechaHora, despues);
        }

        [Test]
        public void MostrarInformacion_NoLanzaExcepcion()
        {
            Boleto boleto = new Boleto("144", 1580, 10000);
            Assert.DoesNotThrow(() => boleto.MostrarInformacion());
        }

        [Test]
        public void PropiedadLineaColectivo_EsPublica()
        {
            Boleto boleto = new Boleto("Panorámico", 1580, 500);
            string linea = boleto.LineaColectivo;
            Assert.IsNotNull(linea);
        }

        [Test]
        public void PropiedadImportePagado_EsPublica()
        {
            Boleto boleto = new Boleto("Ñ", 1580, 0);
            decimal importe = boleto.ImportePagado;
            Assert.AreEqual(1580, importe);
        }

        [Test]
        public void PropiedadSaldoRestante_EsPublica()
        {
            Boleto boleto = new Boleto("142", 1580, 38420);
            decimal saldo = boleto.SaldoRestante;
            Assert.AreEqual(38420, saldo);
        }

        [Test]
        public void PropiedadFechaHora_EsPublica()
        {
            Boleto boleto = new Boleto("101", 1580, 15000);
            DateTime fecha = boleto.FechaHora;
            Assert.IsNotNull(fecha);
        }

        [Test]
        public void Constructor_ConSaldoCero_FuncionaBien()
        {
            Boleto boleto = new Boleto("27", 1580, 0);
            Assert.AreEqual(0, boleto.SaldoRestante);
        }
    }
}