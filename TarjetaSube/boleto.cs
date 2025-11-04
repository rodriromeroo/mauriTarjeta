using System;

namespace TarjetaSube
{
    public class Boleto
    {
        public string LineaColectivo { get; private set; }
        public decimal ImportePagado { get; private set; }
        public decimal SaldoRestante { get; private set; }
        public DateTime FechaHora { get; private set; }
        public bool EsTransbordo { get; private set; }

        public Boleto(string linea, decimal importe, decimal saldo)
            : this(linea, importe, saldo, false)
        {
        }

        public Boleto(string linea, decimal importe, decimal saldo, bool esTransbordo)
        {
            LineaColectivo = linea;
            ImportePagado = importe;
            SaldoRestante = saldo;
            FechaHora = DateTime.Now;
            EsTransbordo = esTransbordo;
        }

        public void MostrarInformacion()
        {
            Console.WriteLine("================================");
            Console.WriteLine("      BOLETO DE COLECTIVO      ");
            Console.WriteLine("================================");
            Console.WriteLine("Línea: " + LineaColectivo);
            Console.WriteLine("Fecha: " + FechaHora.ToString("dd/MM/yyyy"));
            Console.WriteLine("Hora: " + FechaHora.ToString("HH:mm:ss"));
            Console.WriteLine("Importe pagado: $" + ImportePagado);
            Console.WriteLine("Saldo restante: $" + SaldoRestante);
            if (EsTransbordo)
            {
                Console.WriteLine("*** TRASBORDO ***");
            }
            Console.WriteLine("================================");
        }
    }
}