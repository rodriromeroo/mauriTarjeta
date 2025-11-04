using NUnit.Framework;
using TarjetaSube;
using System;

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
        public void CalcularDescuento_PrimerosDosViajes_DevuelveCero()
        {
            // Los primeros 2 viajes del día son gratis
            decimal descuento = tarjeta.CalcularDescuento(1580);
            Assert.AreEqual(0, descuento);
        }

        [Test]
        public void CalcularDescuento_ConCualquierMonto_DevuelveCero()
        {
            // Cuando puede viajar gratis, siempre devuelve 0
            Assert.AreEqual(0, tarjeta.CalcularDescuento(100));
            Assert.AreEqual(0, tarjeta.CalcularDescuento(5000));
            Assert.AreEqual(0, tarjeta.CalcularDescuento(99999));
        }

        [Test]
        public void PagarCon_SinCargarSaldo_GeneraBoleto()
        {
            // Horario permitido (Lun-Vie 6-22)
            DateTime horarioPermitido = new DateTime(2024, 11, 20, 10, 0, 0);

            // Los primeros 2 viajes son gratis, no necesita saldo
            Boleto boleto = colectivo.PagarCon(tarjeta, horarioPermitido);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, boleto.ImportePagado);
        }

        [Test]
        public void PagarCon_VariosViajes_NuncaDescuenta()
        {
            tarjeta.CargarSaldo(5000);
            DateTime horarioPermitido = new DateTime(2024, 11, 20, 10, 0, 0);

            // Solo los primeros 2 viajes no descuentan
            colectivo.PagarCon(tarjeta, horarioPermitido);
            colectivo.PagarCon(tarjeta, horarioPermitido.AddMinutes(10));

            // Los primeros 2 viajes no descuentan saldo
            Assert.AreEqual(5000, tarjeta.ObtenerSaldo());

            // El tercer viaje sí descuenta
            colectivo.PagarCon(tarjeta, horarioPermitido.AddMinutes(20));
            colectivo.PagarCon(tarjeta, horarioPermitido.AddMinutes(30));

            // Ahora sí hay descuento (2 viajes x 1580)
            Assert.AreEqual(1840, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void BoletoGratuito_HeredaDeTarjeta()
        {
            Assert.IsInstanceOf<Tarjeta>(tarjeta);
        }

        [Test]
        public void PagarCon_ConSaldoNegativo_SigueFuncionando()
        {
            // Los primeros 2 viajes gratis funcionan con saldo negativo
            tarjeta.CargarSaldo(500);
            tarjeta.DescontarSaldo(1000); // Saldo negativo
            DateTime horarioPermitido = new DateTime(2024, 11, 20, 10, 0, 0);

            Boleto boleto = colectivo.PagarCon(tarjeta, horarioPermitido);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, boleto.ImportePagado);
        }
    }
}