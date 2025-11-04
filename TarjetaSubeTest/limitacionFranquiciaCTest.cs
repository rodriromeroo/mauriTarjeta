using NUnit.Framework;
using TarjetaSube;

namespace TarjetaSubeTest {
    
    [TestFixture]
    public class LimitacionFranquiciaCompletaTest {
        
        [Test]
        public void BoletoGratuito_NoPermiteMasDeDosViajesGratisPorDia() {
            // Arrange
            TarjetaBoletoGratuito tarjeta = new TarjetaBoletoGratuito();
            tarjeta.CargarSaldo(5000);
            Colectivo colectivo = new Colectivo("102");
            
            // Act
            Boleto primerViaje = colectivo.PagarCon(tarjeta);
            Boleto segundoViaje = colectivo.PagarCon(tarjeta);
            Boleto tercerViaje = colectivo.PagarCon(tarjeta);
            
            // Assert
            Assert.IsNotNull(primerViaje);
            Assert.IsNotNull(segundoViaje);
            Assert.IsNotNull(tercerViaje);
            
            // Los primeros dos viajes son gratis
            Assert.AreEqual(0, primerViaje.ImportePagado);
            Assert.AreEqual(0, segundoViaje.ImportePagado);
            
            // El tercer viaje cobra tarifa completa
            Assert.AreEqual(1580, tercerViaje.ImportePagado);
            
            // Verifica que haya usado 2 viajes gratuitos
            Assert.AreEqual(3, tarjeta.ObtenerViajesGratuitosHoy());
        }
        
        [Test]
        public void BoletoGratuito_ViajesPosterioresAlSegundoCobraCompleto() {
            // Arrange
            TarjetaBoletoGratuito tarjeta = new TarjetaBoletoGratuito();
            tarjeta.CargarSaldo(10000);
            Colectivo colectivo = new Colectivo("K");
            
            decimal saldoInicial = tarjeta.ObtenerSaldo();
            
            // Act
            colectivo.PagarCon(tarjeta); // Viaje 1: gratis
            colectivo.PagarCon(tarjeta); // Viaje 2: gratis
            
            decimal saldoDespuesDosGratis = tarjeta.ObtenerSaldo();
            
            Boleto tercerViaje = colectivo.PagarCon(tarjeta); // Viaje 3: paga completo
            Boleto cuartoViaje = colectivo.PagarCon(tarjeta);  // Viaje 4: paga completo
            
            decimal saldoFinal = tarjeta.ObtenerSaldo();
            
            // Assert
            // Los dos primeros no descuentan saldo
            Assert.AreEqual(saldoInicial, saldoDespuesDosGratis);
            
            // El tercero y cuarto cobran tarifa completa
            Assert.AreEqual(1580, tercerViaje.ImportePagado);
            Assert.AreEqual(1580, cuartoViaje.ImportePagado);
            
            // Verifica que se descontaron 2 viajes completos
            Assert.AreEqual(saldoInicial - 3160, saldoFinal);
        }
        
        [Test]
        public void BoletoGratuito_PuedeViajarGratisVerificaCorrectamente() {
            // Arrange
            TarjetaBoletoGratuito tarjeta = new TarjetaBoletoGratuito();
            Colectivo colectivo = new Colectivo("144");
            
            // Act y Assert
            // Al principio puede viajar gratis
            Assert.IsTrue(tarjeta.PuedeViajarGratis());
            
            // Primer viaje
            colectivo.PagarCon(tarjeta);
            Assert.IsTrue(tarjeta.PuedeViajarGratis());
            
            // Segundo viaje
            colectivo.PagarCon(tarjeta);
            Assert.IsFalse(tarjeta.PuedeViajarGratis());
            
            // Tercer viaje - ya no puede viajar gratis
            Assert.IsFalse(tarjeta.PuedeViajarGratis());
        }
        
        [Test]
        public void BoletoGratuito_ContadorViajesGratuitosFuncionaCorrectamente() {
            // Arrange
            TarjetaBoletoGratuito tarjeta = new TarjetaBoletoGratuito();
            tarjeta.CargarSaldo(5000);
            Colectivo colectivo = new Colectivo("133");
            
            // Act y Assert
            Assert.AreEqual(0, tarjeta.ObtenerViajesGratuitosHoy());
            
            colectivo.PagarCon(tarjeta);
            Assert.AreEqual(1, tarjeta.ObtenerViajesGratuitosHoy());
            
            colectivo.PagarCon(tarjeta);
            Assert.AreEqual(2, tarjeta.ObtenerViajesGratuitosHoy());
            
            colectivo.PagarCon(tarjeta);
            Assert.AreEqual(3, tarjeta.ObtenerViajesGratuitosHoy());
        }
        
        [Test]
        public void BoletoGratuito_SinSaldo_TercerViajeNoPermitido() {
            // Arrange
            TarjetaBoletoGratuito tarjeta = new TarjetaBoletoGratuito();
            // No carga saldo
            Colectivo colectivo = new Colectivo("27");
            
            // Act
            Boleto primerViaje = colectivo.PagarCon(tarjeta);
            Boleto segundoViaje = colectivo.PagarCon(tarjeta);
            Boleto tercerViaje = colectivo.PagarCon(tarjeta);
            
            // Assert
            // Los dos primeros viajes son exitosos (gratis)
            Assert.IsNotNull(primerViaje);
            Assert.IsNotNull(segundoViaje);
            
            // El tercer viaje falla porque no tiene saldo
            Assert.IsNull(tercerViaje);
        }
    }
}