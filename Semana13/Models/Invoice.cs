namespace Semana13.Models
{
    public class Invoice
    {
        public int InvoiceID { get; set; }
        public Customer Customer { get; set; }
        public int CustomerID { get; set; }
        public string Date { get; set; }
        public int InvoiceNumber { get; set; }
        public float Total { get; set; }
    }
}
