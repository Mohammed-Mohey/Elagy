using System.ComponentModel.DataAnnotations.Schema;

namespace Elagy.Models
{
    public class Roshta
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string SpeicalLocation { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Date { get; set; }= DateTime.Now;

        public string Status { get; set; } = "قيد المعالجه";
        public decimal price { get; set; } = 0;
        public string ImagePath { get; set; }



        [ForeignKey("Pharmacy")]
        public Pharmacy? pharmacyId { get; set; }

    }
}
