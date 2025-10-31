using System;

namespace TarjetaSube {
    class Program {
        static void Main(string[] args) {
            Tarjeta miTarjeta = new Tarjeta();
            
            Console.WriteLine("Cargando $5000");
            bool cargoOk = miTarjeta.CargarSaldo(5000);
            if (cargoOk) {
                Console.WriteLine("Saldo actual: $" + miTarjeta.ObtenerSaldo());
            }
            
            Colectivo cole = new Colectivo("102 144");
            
            Console.WriteLine("\nPagando viaje.");
            bool pagoExitoso = cole.PagarCon(miTarjeta);
            if (pagoExitoso) {
                Console.WriteLine("Pago exitoso!");
                Console.WriteLine("Saldo restante: $" + miTarjeta.ObtenerSaldo());
            } else {
                Console.WriteLine("Saldo insuficiente.");
            }
            
            Console.WriteLine("\nCargando $7000");
            bool cargaInvalida = miTarjeta.CargarSaldo(7000);
            Console.WriteLine("Resultado: " + (cargaInvalida ? "OK" : "RECHAZADO"));
            
            Console.WriteLine("\nCargando $30000");
            miTarjeta.CargarSaldo(30000);
            Console.WriteLine("Saldo actual: $" + miTarjeta.ObtenerSaldo());
            
            Console.WriteLine("\nIntentando pagar otro viaje");
            bool pagoExitoso2 = cole.PagarCon(miTarjeta);
            if (pagoExitoso2) {
                Console.WriteLine("Pago exitoso!");
                Console.WriteLine("Saldo restante: $" + miTarjeta.ObtenerSaldo());
            } else {
                Console.WriteLine("Saldo insuficiente.");
            }
            
            Console.WriteLine("\nIntentando pagar sin saldo suficiente");
            Tarjeta tarjetaVacia = new Tarjeta();
            bool pagoFallido = cole.PagarCon(tarjetaVacia);
            if (pagoFallido) {
                Console.WriteLine("Pago exitoso!");
            } else {
                Console.WriteLine("Pago rechazado - Saldo insuficiente.");
            }
            
            Console.WriteLine("\nPresiona cualquier tecla para salir!");
            Console.ReadKey();
        }
    }
}