using NUnit.Framework;
using System;
using TarjetaSube;

namespace TarjetaSube.Tests
{
    [TestFixture]
    public class TarjetaMedioBoletoTests
    {
        private TarjetaMedioBoleto tarjeta;
        private Colectivo colectivo;

        [SetUp]
        public void Setup()
        {
            tarjeta = new TarjetaMedioBoleto();
            colectivo = new Colectivo("144");
        }

        [Test]
        public void CalcularDescuento_PrimerViaje_DevuelveMitad()
        {
            DateTime ahora = DateTime.Now;
            decimal descuento = tarjeta.CalcularDescuento(1580, ahora);
            Assert.AreEqual(790, descuento);
        }

        [Test]
        public void CalcularDescuento_TercerViajeDelDia_DevuelvePrecioCompleto()
        {
            DateTime ahora = DateTime.Now;
            tarjeta.CalcularDescuento(1580, ahora);
            tarjeta.RegistrarViaje(ahora);

            tarjeta.CalcularDescuento(1580, ahora.AddMinutes(10));
            tarjeta.RegistrarViaje(ahora.AddMinutes(10));

            decimal descuento = tarjeta.CalcularDescuento(1580, ahora.AddMinutes(20));
            Assert.AreEqual(1580, descuento);
        }

        [Test]
        public void PuedeViajar_MenosDe5Minutos_RetornaFalse()
        {
            DateTime primerViaje = new DateTime(2024, 11, 20, 10, 0, 0);
            tarjeta.RegistrarViaje(primerViaje);

            DateTime segundoIntento = primerViaje.AddMinutes(3);
            bool puede = tarjeta.PuedeViajar(segundoIntento);

            Assert.IsFalse(puede);
        }

        [Test]
        public void PuedeViajar_MasDe5Minutos_RetornaTrue()
        {
            DateTime primerViaje = new DateTime(2024, 11, 20, 10, 0, 0);
            tarjeta.RegistrarViaje(primerViaje);

            DateTime segundoIntento = primerViaje.AddMinutes(6);
            bool puede = tarjeta.PuedeViajar(segundoIntento);

            Assert.IsTrue(puede);
        }

        [Test]
        public void PuedeViajar_Exactamente5Minutos_RetornaTrue()
        {
            DateTime primerViaje = new DateTime(2024, 11, 20, 10, 0, 0);
            tarjeta.RegistrarViaje(primerViaje);

            DateTime segundoIntento = primerViaje.AddMinutes(5);
            bool puede = tarjeta.PuedeViajar(segundoIntento);

            Assert.IsTrue(puede);
        }

        [Test]
        public void PagarCon_AntesDe5Minutos_DevuelveNull()
        {
            tarjeta.CargarSaldo(5000);

            DateTime tiempo1 = new DateTime(2024, 11, 20, 10, 0, 0);
            Boleto boleto1 = colectivo.PagarCon(tarjeta, tiempo1);
            Assert.IsNotNull(boleto1);

            DateTime tiempo2 = tiempo1.AddMinutes(3);
            Boleto boleto2 = colectivo.PagarCon(tarjeta, tiempo2);
            Assert.IsNull(boleto2);
        }

        [Test]
        public void PagarCon_DespuesDe5Minutos_GeneraBoleto()
        {
            tarjeta.CargarSaldo(5000);

            DateTime tiempo1 = new DateTime(2024, 11, 20, 10, 0, 0);
            Boleto boleto1 = colectivo.PagarCon(tarjeta, tiempo1);
            Assert.IsNotNull(boleto1);

            DateTime tiempo2 = tiempo1.AddMinutes(6);
            Boleto boleto2 = colectivo.PagarCon(tarjeta, tiempo2);
            Assert.IsNotNull(boleto2);
        }

        [Test]
        public void ObtenerViajesHoy_TresViajesEnUnDia_Devuelve3()
        {
            DateTime tiempo1 = new DateTime(2024, 11, 20, 10, 0, 0);
            tarjeta.RegistrarViaje(tiempo1);

            DateTime tiempo2 = tiempo1.AddMinutes(10);
            tarjeta.RegistrarViaje(tiempo2);

            DateTime tiempo3 = tiempo2.AddMinutes(10);
            tarjeta.RegistrarViaje(tiempo3);

            Assert.AreEqual(3, tarjeta.ObtenerViajesHoy());
        }

        [Test]
        public void ObtenerViajesHoy_ViajesEnDiasDiferentes_ReiniciaContador()
        {
            DateTime dia1 = new DateTime(2024, 11, 20, 10, 0, 0);
            tarjeta.RegistrarViaje(dia1);
            tarjeta.RegistrarViaje(dia1.AddMinutes(10));

            DateTime dia2 = new DateTime(2024, 11, 21, 10, 0, 0);
            tarjeta.RegistrarViaje(dia2);

            Assert.AreEqual(1, tarjeta.ObtenerViajesHoy());
        }

        [Test]
        public void PagarCon_TercerViajeDelDia_CobraPrecioCompleto()
        {
            tarjeta.CargarSaldo(10000);

            DateTime tiempo1 = new DateTime(2024, 11, 20, 8, 0, 0);
            Boleto b1 = colectivo.PagarCon(tarjeta, tiempo1);

            DateTime tiempo2 = tiempo1.AddMinutes(10);
            Boleto b2 = colectivo.PagarCon(tarjeta, tiempo2);

            DateTime tiempo3 = tiempo2.AddMinutes(10);
            Boleto b3 = colectivo.PagarCon(tarjeta, tiempo3);

            Assert.AreEqual(790, b1.ImportePagado);
            Assert.AreEqual(790, b2.ImportePagado);
            Assert.AreEqual(1580, b3.ImportePagado);
        }

        [Test]
        public void MedioBoleto_HeredaDeTarjeta()
        {
            Assert.IsInstanceOf<Tarjeta>(tarjeta);
        }

        [Test]
        public void ObtenerTipo_DevuelveMedioBoleto()
        {
            Assert.AreEqual("Medio Boleto", tarjeta.ObtenerTipo());
        }
    }
}