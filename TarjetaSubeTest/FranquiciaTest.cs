using NUnit.Framework;
using System;
using TarjetaSube;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class FranquiciaTest
    {
        [Test]
        public void FranquiciaCompletaSiemprePuedePagar()
        {
            TarjetaFranquiciaCompleta tarjeta = new TarjetaFranquiciaCompleta();
            Colectivo colectivo = new Colectivo("102");

            Boleto primerViaje = colectivo.PagarCon(tarjeta);
            Boleto segundoViaje = colectivo.PagarCon(tarjeta);
            Boleto tercerViaje = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(primerViaje);
            Assert.IsNotNull(segundoViaje);
            Assert.IsNotNull(tercerViaje);

            decimal saldo = tarjeta.ObtenerSaldo();
            Assert.AreEqual(0, saldo);
        }

        [Test]
        public void MedioBoletoCobraMitadDelPasaje()
        {
            TarjetaMedioBoleto tarjeta = new TarjetaMedioBoleto();
            tarjeta.CargarSaldo(5000);

            decimal saldoInicial = tarjeta.ObtenerSaldo();

            Colectivo colectivo = new Colectivo("144");

            DateTime tiempo = DateTime.Now;
            Boleto boleto = colectivo.PagarCon(tarjeta, tiempo);

            Assert.IsNotNull(boleto);

            decimal saldoFinal = tarjeta.ObtenerSaldo();
            decimal montoDescontado = saldoInicial - saldoFinal;

            decimal tarifaNormal = 1580;
            decimal mitadTarifa = tarifaNormal / 2;

            Assert.AreEqual(mitadTarifa, montoDescontado);
            Assert.AreEqual(790, boleto.ImportePagado);
        }

        [Test]
        public void MedioBoletoSiempreCobraMitad()
        {
            TarjetaMedioBoleto tarjeta = new TarjetaMedioBoleto();
            tarjeta.CargarSaldo(10000);

            Colectivo colectivo = new Colectivo("K");

            DateTime tiempo1 = new DateTime(2024, 11, 20, 10, 0, 0);
            decimal saldo1 = tarjeta.ObtenerSaldo();
            Boleto boleto1 = colectivo.PagarCon(tarjeta, tiempo1);
            decimal saldo2 = tarjeta.ObtenerSaldo();
            decimal descuento1 = saldo1 - saldo2;

            DateTime tiempo2 = tiempo1.AddMinutes(10);
            Boleto boleto2 = colectivo.PagarCon(tarjeta, tiempo2);
            decimal saldo3 = tarjeta.ObtenerSaldo();
            decimal descuento2 = saldo2 - saldo3;

            DateTime tiempo3 = tiempo2.AddMinutes(10);
            Boleto boleto3 = colectivo.PagarCon(tarjeta, tiempo3);
            decimal saldo4 = tarjeta.ObtenerSaldo();
            decimal descuento3 = saldo3 - saldo4;

            Assert.AreEqual(790, descuento1);
            Assert.AreEqual(790, descuento2);
            Assert.AreEqual(1580, descuento3);
            Assert.AreEqual(790, boleto1.ImportePagado);
            Assert.AreEqual(790, boleto2.ImportePagado);
            Assert.AreEqual(1580, boleto3.ImportePagado);
        }

        [Test]
        public void BoletoGratuitoNoCubraSaldo()
        {
            TarjetaBoletoGratuito tarjeta = new TarjetaBoletoGratuito();

            decimal saldoInicial = tarjeta.ObtenerSaldo();

            Colectivo colectivo = new Colectivo("102");

            Boleto boleto = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(boleto);

            decimal saldoFinal = tarjeta.ObtenerSaldo();
            Assert.AreEqual(saldoInicial, saldoFinal);
            Assert.AreEqual(0, boleto.ImportePagado);
        }

        [Test]
        public void FranquiciaCompletaNoCubraSaldo()
        {
            TarjetaFranquiciaCompleta tarjeta = new TarjetaFranquiciaCompleta();

            decimal saldoInicial = tarjeta.ObtenerSaldo();

            Colectivo colectivo = new Colectivo("144");

            Boleto boleto = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(boleto);

            decimal saldoFinal = tarjeta.ObtenerSaldo();
            Assert.AreEqual(saldoInicial, saldoFinal);
            Assert.AreEqual(0, boleto.ImportePagado);
        }
    }
}