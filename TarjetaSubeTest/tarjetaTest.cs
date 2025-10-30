using NUnit.Framework;
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
        public void CargarSaldo_SuperaLimite40000_RetornaFalse()
        {
            tarjeta.CargarSaldo(30000);
            bool resultado = tarjeta.CargarSaldo(15000);
            Assert.IsFalse(resultado);
            Assert.AreEqual(30000, tarjeta.ObtenerSaldo());
        }

        [Test]
        public void CargarSaldo_ExactamenteLimite40000_RetornaTrue()
        {
            tarjeta.CargarSaldo(20000);
            bool resultado = tarjeta.CargarSaldo(20000);
            Assert.IsTrue(resultado);
            Assert.AreEqual(40000, tarjeta.ObtenerSaldo());
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
        public void DescontarSaldo_SinSaldoSuficiente_RetornaFalse()
        {
            tarjeta.CargarSaldo(1000);
            bool resultado = tarjeta.DescontarSaldo(1580);
            Assert.IsFalse(resultado);
            Assert.AreEqual(0, tarjeta.ObtenerSaldo());
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
    }
}