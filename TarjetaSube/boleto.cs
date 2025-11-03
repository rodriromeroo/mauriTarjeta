using System;

namespace TarjetaSube
{
    public class Boleto
    {
        public string LineaColectivo { get; private set; }
        public decimal ImportePagado { get; private set; }
        public decimal SaldoRestante { get; private set; }
        public DateTime FechaHora { get; private set; }
        public string TipoTarjeta { get; private set; }
        public string IDTarjeta { get; private set; }
        public decimal TotalAbonado { get; private set; }

        public Boleto(string linea, decimal importe, decimal saldo, string tipoTarjeta, string idTarjeta, decimal totalAbonado)
        {
            LineaColectivo = linea;
            ImportePagado = importe;
            SaldoRestante = saldo;
            FechaHora = DateTime.Now;
            TipoTarjeta = tipoTarjeta;
            IDTarjeta = idTarjeta;
            TotalAbonado = totalAbonado;
        }

        public void MostrarInformacion()
        {
            Console.WriteLine("================================");
            Console.WriteLine("      BOLETO DE COLECTIVO      ");
            Console.WriteLine("================================");
            Console.WriteLine("Línea: " + LineaColectivo);
            Console.WriteLine("Fecha: " + FechaHora.ToString("dd/MM/yyyy"));
            Console.WriteLine("Hora: " + FechaHora.ToString("HH:mm:ss"));
            Console.WriteLine("Tipo Tarjeta: " + TipoTarjeta);
            Console.WriteLine("ID Tarjeta: " + IDTarjeta);
            Console.WriteLine("Importe pagado: $" + ImportePagado);
            Console.WriteLine("Total abonado: $" + TotalAbonado);
            Console.WriteLine("Saldo restante: $" + SaldoRestante);
            Console.WriteLine("================================");
        }
    }
}