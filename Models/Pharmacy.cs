using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elagy.Models
{
    public class Pharmacy
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }

        public string PharmacyName { get; set; }

        public string PharmacyPhone { get; set; }
        public string WorkingHours { get; set; }

        public string DeliveryArea { get; set; }

        public string ManagerName { get; set; }

        public string ManagerPhone { get; set; }
        public string TradeLicense { get; set; }
        public string TaxCard { get; set; }
        public string PharmacyLicense { get; set; }
        


        // navigation prob
        public ICollection<Product>? Products { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
