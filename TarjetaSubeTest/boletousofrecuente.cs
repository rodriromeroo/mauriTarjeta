using NUnit.Framework;
using TarjetaSube;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class BoletoUsoFrecuenteTest
    {
        [Test]
        public void Tarjeta_Viajes1a29_TarifaNormal()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000);
            Colectivo colectivo = new Colectivo("102");
            
            decimal saldoInicial = tarjeta.ObtenerSaldo();
            
            // Act - hacer 29 viajes
            for (int i = 0; i < 29; i++)
            {
                Boleto boleto = colectivo.PagarCon(tarjeta);
                Assert.IsNotNull(boleto);
                Assert.AreEqual(1580, boleto.ImportePagado);
            }
            
            // Assert
            decimal saldoFinal = tarjeta.ObtenerSaldo();
            decimal totalGastado = saldoInicial - saldoFinal;
            Assert.AreEqual(29 * 1580, totalGastado);
            Assert.AreEqual(29, tarjeta.ObtenerViajesDelMes());
        }
        
        [Test]
        public void Tarjeta_Viajes30a59_Descuento20Porciento()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000);
            Colectivo colectivo = new Colectivo("K");
            
            // hacer los primeros 29 viajes
            for (int i = 0; i < 29; i++)
            {
                colectivo.PagarCon(tarjeta);
            }
            
            decimal saldoAntes = tarjeta.ObtenerSaldo();
            
            // Act - viaje 30 (deberia tener 20% descuento)
            Boleto boleto30 = colectivo.PagarCon(tarjeta);
            
            // Assert
            decimal montoEsperado = 1580 * 0.80m; // 20% descuento = 1264
            Assert.AreEqual(montoEsperado, boleto30.ImportePagado);
            Assert.AreEqual(saldoAntes - montoEsperado, tarjeta.ObtenerSaldo());
        }
        
        [Test]
        public void Tarjeta_Viajes60a80_Descuento25Porciento()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000);
            tarjeta.CargarSaldo(30000);
            Colectivo colectivo = new Colectivo("144");
            
            // hacer los primeros 59 viajes
            for (int i = 0; i < 59; i++)
            {
                colectivo.PagarCon(tarjeta);
            }
            
            decimal saldoAntes = tarjeta.ObtenerSaldo();
            
            // Act - viaje 60 (deberia tener 25% descuento)
            Boleto boleto60 = colectivo.PagarCon(tarjeta);
            
            // Assert
            decimal montoEsperado = 1580 * 0.75m; // 25% descuento = 1185
            Assert.AreEqual(montoEsperado, boleto60.ImportePagado);
            Assert.AreEqual(saldoAntes - montoEsperado, tarjeta.ObtenerSaldo());
        }
        
        [Test]
        public void Tarjeta_Viajes81EnAdelante_TarifaNormal()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta();
            // cargar bastante saldo
            for (int i = 0; i < 5; i++)
            {
                tarjeta.CargarSaldo(30000);
            }
            
            Colectivo colectivo = new Colectivo("27");
            
            // hacer los primeros 80 viajes
            for (int i = 0; i < 80; i++)
            {
                colectivo.PagarCon(tarjeta);
            }
            
            decimal saldoAntes = tarjeta.ObtenerSaldo();
            
            // Act - viaje 81 (vuelve a tarifa normal)
            Boleto boleto81 = colectivo.PagarCon(tarjeta);
            
            // Assert
            Assert.AreEqual(1580, boleto81.ImportePagado);
            Assert.AreEqual(saldoAntes - 1580, tarjeta.ObtenerSaldo());
        }
        
        [Test]
        public void Tarjeta_VerificaDescuentos_EnRangos()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta();
            for (int i = 0; i < 5; i++)
            {
                tarjeta.CargarSaldo(30000);
            }
            Colectivo colectivo = new Colectivo("133");
            
            // Act y Assert
            // viajes 1-29: tarifa normal
            for (int i = 1; i <= 29; i++)
            {
                Boleto b = colectivo.PagarCon(tarjeta);
                Assert.AreEqual(1580, b.ImportePagado, $"Viaje {i} deberia ser 1580");
            }
            
            // viajes 30-59: 20% descuento
            for (int i = 30; i <= 59; i++)
            {
                Boleto b = colectivo.PagarCon(tarjeta);
                Assert.AreEqual(1264, b.ImportePagado, $"Viaje {i} deberia ser 1264");
            }
            
            // viajes 60-80: 25% descuento
            for (int i = 60; i <= 80; i++)
            {
                Boleto b = colectivo.PagarCon(tarjeta);
                Assert.AreEqual(1185, b.ImportePagado, $"Viaje {i} deberia ser 1185");
            }
            
            // viaje 81: vuelve a normal
            Boleto b81 = colectivo.PagarCon(tarjeta);
            Assert.AreEqual(1580, b81.ImportePagado);
        }
        
        [Test]
        public void Tarjeta_ContadorViajes_FuncionaCorrectamente()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta();
            tarjeta.CargarSaldo(30000);
            Colectivo colectivo = new Colectivo("G");
            
            // Act y Assert
            Assert.AreEqual(0, tarjeta.ObtenerViajesDelMes());
            
            colectivo.PagarCon(tarjeta);
            Assert.AreEqual(1, tarjeta.ObtenerViajesDelMes());
            
            colectivo.PagarCon(tarjeta);
            Assert.AreEqual(2, tarjeta.ObtenerViajesDelMes());
            
            for (int i = 0; i < 10; i++)
            {
                colectivo.PagarCon(tarjeta);
            }
            Assert.AreEqual(12, tarjeta.ObtenerViajesDelMes());
        }
        
        [Test]
        public void MedioBoleto_NoAplicaUsoFrecuente()
        {
            // Arrange
            TarjetaMedioBoleto medio = new TarjetaMedioBoleto();
            medio.CargarSaldo(30000);
            medio.CargarSaldo(30000);
            Colectivo colectivo = new Colectivo("102");
            
            // hacer muchos viajes
            for (int i = 0; i < 50; i++)
            {
                colectivo.PagarCon(medio);
            }
            
            // Act - el viaje 51 deberia seguir siendo medio boleto normal
            Boleto boleto = colectivo.PagarCon(medio);
            
            // Assert
            Assert.AreEqual(790, boleto.ImportePagado); // siempre mitad, no descuento adicional
        }
        
        [Test]
        public void BoletoGratuito_NoAplicaUsoFrecuente()
        {
            // Arrange
            TarjetaBoletoGratuito gratuito = new TarjetaBoletoGratuito();
            gratuito.CargarSaldo(20000);
            Colectivo colectivo = new Colectivo("K");
            
            // hacer varios viajes
            for (int i = 0; i < 40; i++)
            {
                colectivo.PagarCon(gratuito);
            }
            
            // Act - verificar saldo
            decimal saldo = gratuito.ObtenerSaldo();
            
            // Assert - los dos primeros gratis, el resto a precio completo sin descuento
            decimal gastoEsperado = 38 * 1580; // 40 viajes - 2 gratis = 38 * 1580
            Assert.AreEqual(20000 - gastoEsperado, saldo);
        }
    }
}