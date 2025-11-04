using NUnit.Framework;
using TarjetaSube;

namespace TarjetaSube.Tests
{
    [TestFixture]
    public class SaldoTarjetaTests
    {
        private Tarjeta tarjeta;
        private Colectivo colectivo;

        [SetUp]
        public void Setup()
        {
            tarjeta = new Tarjeta();
            colectivo = new Colectivo("133");
        }

        [Test]
        public void CargarSaldo_SuperaLimite56000_AcreditaHastaLimiteYGuardaExcedente()
        {
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000);

            Assert.AreEqual(56000, tarjeta.ObtenerSaldo());
            Assert.AreEqual(4000, tarjeta.ObtenerSaldoPendiente());
        }

        [Test]
        public void AcreditarCarga_DespuesDeViaje_RecargaSaldo()
        {
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000); // 56000 + 4000 pendiente
            decimal saldoInicial = tarjeta.ObtenerSaldo();
            decimal pendienteInicial = tarjeta.ObtenerSaldoPendiente();

            colectivo.PagarCon(tarjeta); // descuenta 1580 y tiene q acreditar
t
            Assert.AreEqual(54420, tarjeta.ObtenerSaldo()); // 56000 - 1580 = 54420
            Assert.Less(tarjeta.ObtenerSaldoPendiente(), pendienteInicial);
        }

        [Test]
        public void CargarSaldo_ConSaldoPendiente_IntentaAcreditarPrimero()
        {
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000); // 56000 + 4000 pendiente

            colectivo.PagarCon(tarjeta); // libera espacio

            bool resultado = tarjeta.CargarSaldo(2000);

            
            Assert.IsTrue(resultado);
        }

        [Test]
        public void TarjetaMedioBoleto_TambienRespetaLimite56000()
        {
            TarjetaMedioBoleto medioBoleto = new TarjetaMedioBoleto();

            medioBoleto.CargarSaldo(30000);
            medioBoleto.CargarSaldo(30000);

            Assert.AreEqual(56000, medioBoleto.ObtenerSaldo());
            Assert.AreEqual(4000, medioBoleto.ObtenerSaldoPendiente());
        }

        [Test]
        public void TarjetaBoletoGratuito_TambienRespetaLimite56000()
        {
            TarjetaBoletoGratuito gratuito = new TarjetaBoletoGratuito();

            gratuito.CargarSaldo(30000);
            gratuito.CargarSaldo(30000);

            Assert.AreEqual(56000, gratuito.ObtenerSaldo());
            Assert.AreEqual(4000, gratuito.ObtenerSaldoPendiente());
        }

        [Test]
        public void MultiplesViajes_AcreditanSaldoPendienteProgresivamente()
        {
            tarjeta.CargarSaldo(60000); // 56000 + 4000 pendiente
            decimal pendienteInicial = tarjeta.ObtenerSaldoPendiente();

            colectivo.PagarCon(tarjeta); // Viaje 1
            colectivo.PagarCon(tarjeta); // Viaje 2
            decimal pendienteFinal = tarjeta.ObtenerSaldoPendiente();

            Assert.Less(pendienteFinal, pendienteInicial);
        }
    }
}
