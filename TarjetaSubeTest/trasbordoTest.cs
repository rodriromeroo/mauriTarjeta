using NUnit.Framework;
using System;
using TarjetaSube;

namespace TarjetaSube.Tests
{
    [TestFixture]
    public class TrasbordoTest
    {

        [Test]
        public void Trasbordo_DentroDeUnaHora_EsGratis()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.CargarSaldo(5000);

            DateTime tiempo1 = new DateTime(2024, 11, 20, 10, 0, 0);
            Colectivo cole1 = new Colectivo("120");
            Boleto b1 = cole1.PagarCon(tarjeta, tiempo1);

            decimal saldoDespues1 = tarjeta.ObtenerSaldo();

            DateTime tiempo2 = tiempo1.AddMinutes(45);
            Colectivo cole2 = new Colectivo("144");
            Boleto b2 = cole2.PagarCon(tarjeta, tiempo2);

            Assert.IsFalse(b1.EsTransbordo);
            Assert.IsTrue(b2.EsTransbordo);
            Assert.AreEqual(0, b2.ImportePagado);
            Assert.AreEqual(saldoDespues1, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void Trasbordo_VariasLineas_PrimeraCobraRestantesGratis()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.CargarSaldo(10000);

            DateTime tiempo = new DateTime(2024, 11, 20, 10, 0, 0);

            Colectivo cole1 = new Colectivo("120");
            Boleto b1 = cole1.PagarCon(tarjeta, tiempo);
            decimal saldoDespues1 = tarjeta.ObtenerSaldo();

            Colectivo cole2 = new Colectivo("144");
            Boleto b2 = cole2.PagarCon(tarjeta, tiempo.AddMinutes(20));

            Colectivo cole3 = new Colectivo("K");
            Boleto b3 = cole3.PagarCon(tarjeta, tiempo.AddMinutes(40));

            Assert.AreEqual(1580, b1.ImportePagado);
            Assert.AreEqual(0, b2.ImportePagado);
            Assert.AreEqual(0, b3.ImportePagado);
            Assert.IsTrue(b2.EsTransbordo);
            Assert.IsTrue(b3.EsTransbordo);
            Assert.AreEqual(saldoDespues1, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void Trasbordo_Domingo_NoPuedeHacerTrasbordo()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.CargarSaldo(10000);

            DateTime tiempo1 = new DateTime(2024, 11, 17, 10, 0, 0); // Domingo
            Colectivo cole1 = new Colectivo("120");
            Boleto b1 = cole1.PagarCon(tarjeta, tiempo1);

            DateTime tiempo2 = tiempo1.AddMinutes(30);
            Colectivo cole2 = new Colectivo("144");
            Boleto b2 = cole2.PagarCon(tarjeta, tiempo2);

            Assert.IsFalse(b2.EsTransbordo);
            Assert.AreEqual(1580, b2.ImportePagado);
        }

        [Test]
        public void Trasbordo_HorarioLimite_Hora22_NoPuede()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.CargarSaldo(10000);

            DateTime tiempo1 = new DateTime(2024, 11, 20, 21, 45, 0);
            Colectivo cole1 = new Colectivo("120");
            Boleto b1 = cole1.PagarCon(tarjeta, tiempo1);

            DateTime tiempo2 = tiempo1.AddMinutes(20);
            Colectivo cole2 = new Colectivo("144");
            Boleto b2 = cole2.PagarCon(tarjeta, tiempo2);

            Assert.IsFalse(b2.EsTransbordo);
        }

        [Test]
        public void Trasbordo_Exactamente1Hora_YaNoPuede()
        {
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.CargarSaldo(10000);

            DateTime tiempo1 = new DateTime(2024, 11, 20, 10, 0, 0);
            Colectivo cole1 = new Colectivo("120");
            cole1.PagarCon(tarjeta, tiempo1);

            DateTime tiempo2 = tiempo1.AddHours(1).AddMinutes(1);
            Colectivo cole2 = new Colectivo("144");
            Boleto b2 = cole2.PagarCon(tarjeta, tiempo2);

            Assert.IsFalse(b2.EsTransbordo);
            Assert.AreEqual(1580, b2.ImportePagado);
        }
    }
}