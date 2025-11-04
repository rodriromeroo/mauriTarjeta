using NUnit.Framework;
using System;
using TarjetaSube;

namespace TarjetaSube.Tests
{
    [TestFixture]
    public class TarjetaTests
    {
        private Tarjeta tarjeta;

        [SetUp]
        public void Setup()
        {
            tarjeta = new Tarjeta();
        }

        [Test]
        public void ObtenerSaldo_TarjetaNueva_DevuelveCero()
        {
            decimal saldo = tarjeta.ObtenerSaldo();
            Assert.AreEqual(0, saldo);
        }

        [Test]
        public void CargarSaldo_MontoValido2000_RetornaTrue()
        {
            bool resultado = tarjeta.CargarSaldo(2000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(2000, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void CargarSaldo_MontoValido5000_RetornaTrue()
        {
            bool resultado = tarjeta.CargarSaldo(5000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(5000, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void CargarSaldo_MontoValido30000_RetornaTrue()
        {
            bool resultado = tarjeta.CargarSaldo(30000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(30000, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void CargarSaldo_MontoInvalido1000_RetornaFalse()
        {
            bool resultado = tarjeta.CargarSaldo(1000);
            Assert.IsFalse(resultado);
            Assert.AreEqual(0, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void CargarSaldo_MontoInvalido7000_RetornaFalse()
        {
            bool resultado = tarjeta.CargarSaldo(7000);
            Assert.IsFalse(resultado);
            Assert.AreEqual(0, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void CargarSaldo_SuperaLimite56000_GuardaExcedente()
        {
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000);
            // Según implementación actual, permite cargar y guarda excedente
            bool resultado = tarjeta.CargarSaldo(15000);
            Assert.IsTrue(resultado); // Tu implementación retorna true
            Assert.AreEqual(56000, tarjeta.ObtenerSaldo());
            Assert.Greater(tarjeta.ObtenerSaldoPendiente(), 0); // Tiene saldo pendiente
        }

        [Test]
        public void CargarSaldo_ExactamenteLimite56000_RetornaTrue()
        {
            // Para llegar exactamente a 56000, necesitamos combinar cargas permitidas
            // 30000 + 15000 + 8000 + 3000 = 56000
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(15000);
            tarjeta.CargarSaldo(8000);
            tarjeta.CargarSaldo(3000);

            Assert.AreEqual(56000, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void CargarSaldo_VariasCargas_SumaCorrectamente()
        {
            tarjeta.CargarSaldo(5000);
            tarjeta.CargarSaldo(3000);
            tarjeta.CargarSaldo(2000);
            Assert.AreEqual(10000, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void DescontarSaldo_ConSaldoSuficiente_RetornaTrue()
        {
            tarjeta.CargarSaldo(5000);
            bool resultado = tarjeta.DescontarSaldo(1580);
            Assert.IsTrue(resultado);
            Assert.AreEqual(3420, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void DescontarSaldo_MontoExacto_DejaEnCero()
        {
            tarjeta.CargarSaldo(2000);
            bool resultado = tarjeta.DescontarSaldo(2000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(0, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void DescontarSaldo_VariosDescuentos_CalculaBien()
        {
            tarjeta.CargarSaldo(10000);
            tarjeta.DescontarSaldo(1580);
            tarjeta.DescontarSaldo(1580);
            tarjeta.DescontarSaldo(1580);
            Assert.AreEqual(5260, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void DescontarSaldo_NoPermiteMenosDeMenos1200_RetornaFalse()
        {
            bool resultado = tarjeta.DescontarSaldo(1300);
            Assert.IsFalse(resultado);
            Assert.AreEqual(0, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void DescontarSaldo_PermiteHastaMenos1200_RetornaTrue()
        {
            bool resultado = tarjeta.DescontarSaldo(1200);
            Assert.IsTrue(resultado);
            Assert.AreEqual(-1200, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void DescontarSaldo_PuedeUsarSaldoNegativo()
        {
            tarjeta.CargarSaldo(2000);
            tarjeta.DescontarSaldo(2500);
            Assert.AreEqual(-500, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void CargarSaldo_ConSaldoNegativo_DescuentaDeuda()
        {
            tarjeta.DescontarSaldo(1000);
            tarjeta.CargarSaldo(3000);
            Assert.AreEqual(2000, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void CargarSaldo_ConSaldoNegativoMayor_DescuentaCorrectamente()
        {
            tarjeta.DescontarSaldo(1200);
            tarjeta.CargarSaldo(5000);
            Assert.AreEqual(3800, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void DescontarSaldo_ViajesPlus_DescuentaCorrectamente()
        {
            tarjeta.CargarSaldo(2000);
            tarjeta.DescontarSaldo(1580);
            tarjeta.DescontarSaldo(1580);
            Assert.AreEqual(-1160, tarjeta.ObtenerSaldo());

            tarjeta.CargarSaldo(5000);
            Assert.AreEqual(3840, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void CargarSaldo_TodosLosMontos_Funcionan()
        {
            Assert.IsTrue(tarjeta.CargarSaldo(2000));
            tarjeta = new Tarjeta();
            Assert.IsTrue(tarjeta.CargarSaldo(3000));
            tarjeta = new Tarjeta();
            Assert.IsTrue(tarjeta.CargarSaldo(4000));
            tarjeta = new Tarjeta();
            Assert.IsTrue(tarjeta.CargarSaldo(5000));
            tarjeta = new Tarjeta();
            Assert.IsTrue(tarjeta.CargarSaldo(8000));
            tarjeta = new Tarjeta();
            Assert.IsTrue(tarjeta.CargarSaldo(10000));
            tarjeta = new Tarjeta();
            Assert.IsTrue(tarjeta.CargarSaldo(15000));
            tarjeta = new Tarjeta();
            Assert.IsTrue(tarjeta.CargarSaldo(20000));
            tarjeta = new Tarjeta();
            Assert.IsTrue(tarjeta.CargarSaldo(25000));
            tarjeta = new Tarjeta();
            Assert.IsTrue(tarjeta.CargarSaldo(30000));
        }

        [Test]
        public void DescontarSaldo_EnLimiteExacto_Funciona()
        {
            bool resultado = tarjeta.DescontarSaldo(1200);
            Assert.IsTrue(resultado);
            Assert.AreEqual(-1200, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void DescontarSaldo_UnPesoMasDelLimite_RetornaFalse()
        {
            bool resultado = tarjeta.DescontarSaldo(1201);
            Assert.IsFalse(resultado);
            Assert.AreEqual(0, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void CargarSaldo_DespuesDeLimiteNegativo_Recupera()
        {
            tarjeta.DescontarSaldo(1200);
            tarjeta.CargarSaldo(5000);
            Assert.AreEqual(3800, tarjeta.ObtenerSaldo());

            bool puedeCargarMas = tarjeta.CargarSaldo(10000);
            Assert.IsTrue(puedeCargarMas);
            Assert.AreEqual(13800, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void PuedeHacerTrasbordo_DentroDeUnaHora_LineaDiferente_RetornaTrue()
        {
            tarjeta.CargarSaldo(5000);
            DateTime primerViaje = new DateTime(2024, 11, 20, 10, 0, 0);
            tarjeta.RegistrarViajeParaTrasbordo("120", primerViaje);

            DateTime segundoViaje = primerViaje.AddMinutes(30);
            bool puede = tarjeta.PuedeHacerTrasbordo("144", segundoViaje);

            Assert.IsTrue(puede);
        }

        [Test]
        public void PuedeHacerTrasbordo_MismaLinea_RetornaFalse()
        {
            DateTime primerViaje = new DateTime(2024, 11, 20, 10, 0, 0);
            tarjeta.RegistrarViajeParaTrasbordo("120", primerViaje);

            DateTime segundoViaje = primerViaje.AddMinutes(30);
            bool puede = tarjeta.PuedeHacerTrasbordo("120", segundoViaje);

            Assert.IsFalse(puede);
        }

        [Test]
        public void PuedeHacerTrasbordo_MasDe1Hora_RetornaFalse()
        {
            DateTime primerViaje = new DateTime(2024, 11, 20, 10, 0, 0);
            tarjeta.RegistrarViajeParaTrasbordo("120", primerViaje);

            DateTime segundoViaje = primerViaje.AddHours(2);
            bool puede = tarjeta.PuedeHacerTrasbordo("144", segundoViaje);

            Assert.IsFalse(puede);
        }

        [Test]
        public void PuedeHacerTrasbordo_Domingo_RetornaFalse()
        {
            DateTime primerViaje = new DateTime(2024, 11, 17, 10, 0, 0); // Domingo
            tarjeta.RegistrarViajeParaTrasbordo("120", primerViaje);

            DateTime segundoViaje = primerViaje.AddMinutes(30);
            bool puede = tarjeta.PuedeHacerTrasbordo("144", segundoViaje);

            Assert.IsFalse(puede);
        }

        [Test]
        public void PuedeHacerTrasbordo_FueraDeHorario_RetornaFalse()
        {
            DateTime primerViaje = new DateTime(2024, 11, 20, 23, 0, 0);
            tarjeta.RegistrarViajeParaTrasbordo("120", primerViaje);

            DateTime segundoViaje = primerViaje.AddMinutes(30);
            bool puede = tarjeta.PuedeHacerTrasbordo("144", segundoViaje);

            Assert.IsFalse(puede);
        }
    }
}