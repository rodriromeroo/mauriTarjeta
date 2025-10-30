using System;

namespace TarjetaSube
{
    class Program
    {
        static void Main(string[] args)
        {
            Tarjeta miTarjeta = new Tarjeta();

            Console.WriteLine("Cargando $5000");
            bool cargoOk = miTarjeta.CargarSaldo(5000);

            if (cargoOk)
            {
                Console.WriteLine("Saldo actual: $" + miTarjeta.ObtenerSaldo());
            }

            Colectivo cole = new Colectivo("102 144");

            Console.WriteLine("\nPagando viaje.");
            Boleto boleto = cole.PagarCon(miTarjeta);

            if (boleto != null)
            {
                boleto.MostrarInformacion();
            }
            else
            {
                Console.WriteLine("Saldo insuficiente.");
            }

            Console.WriteLine("\nCargando $7000");
            bool cargaInvalida = miTarjeta.CargarSaldo(7000);
            Console.WriteLine("Resultado: " + (cargaInvalida ? "OK" : "RECHAZADO"));

            Console.WriteLine("\nCargando $30000");
            miTarjeta.CargarSaldo(30000);
            Console.WriteLine("Saldo actual: $" + miTarjeta.ObtenerSaldo());

            Console.WriteLine("\nIntentando pagar otro viaje");
            Boleto boleto2 = cole.PagarCon(miTarjeta);
            if (boleto2 != null)
            {
                boleto2.MostrarInformacion();
            }

            Console.WriteLine("\nPresiona cualquier tecla para salir!");
            Console.ReadKey();
        }
    }
}