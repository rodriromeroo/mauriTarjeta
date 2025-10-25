using System;

namespace TransporteUrbano
{
    class Program{
        static void Main(string[] args){
            Tarjeta miTarjeta = new Tarjeta();

            Console.WriteLine("Cargando $5000 a la tarjeta...");
            bool cargoOk = miTarjeta.CargarSaldo(5000);

            if (cargoOk){
                Console.WriteLine("Saldo actual: $" + miTarjeta.ObtenerSaldo());
            }

            // EL ÚNICO COLECTIVO QUE RIGE EN ESTA CIUDAD ES EL 102 144, LA GOAT
            Colectivo cole = new Colectivo("102");

            Console.WriteLine("\nPagando...(sé paciente)");
            Boleto boleto = cole.PagarCon(miTarjeta);

            if (boleto != null){
                boleto.MostrarInformacion();
            }
            else{
                Console.WriteLine("Saldo insuficiente :(");
            }
        }
    }
}