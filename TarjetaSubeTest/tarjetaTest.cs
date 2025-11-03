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
        public void Tarjeta_TieneIDUnico()
        {
            Tarjeta t1 = new Tarjeta();
            Tarjeta t2 = new Tarjeta();

            Assert.IsNotNull(t1.ID);
            Assert.IsNotNull(t2.ID);
            Assert.AreNotEqual(t1.ID, t2.ID);
        }

        [Test]
        public void ObtenerTipo_TarjetaNormal_DevuelveNormal()
        {
            Assert.AreEqual("Normal", tarjeta.ObtenerTipo());
        }
    }
}