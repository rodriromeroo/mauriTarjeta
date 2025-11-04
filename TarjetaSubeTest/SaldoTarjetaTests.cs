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
            tarjeta.CargarSaldo(30000);
            decimal pendienteInicial = tarjeta.ObtenerSaldoPendiente();

        
            colectivo.PagarCon(tarjeta);

         
            Assert.Less(tarjeta.ObtenerSaldoPendiente(), pendienteInicial);
        }

        [Test]
        public void TarjetaMedioBoleto_TambienRespetaLimite56000()
        {
            // Arrange
            TarjetaMedioBoleto medioBoleto = new TarjetaMedioBoleto();

            // Act
            medioBoleto.CargarSaldo(30000);
            medioBoleto.CargarSaldo(30000);

            // Assert
            Assert.AreEqual(56000, medioBoleto.ObtenerSaldo());
        }

        [Test]
        public void TarjetaBoletoGratuito_TambienRespetaLimite56000()
        {
          
            TarjetaBoletoGratuito gratuito = new TarjetaBoletoGratuito();

       
            gratuito.CargarSaldo(30000);
            gratuito.CargarSaldo(30000);

         
            Assert.AreEqual(56000, gratuito.ObtenerSaldo());
        }
    }
}