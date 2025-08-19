using System.ComponentModel.DataAnnotations.Schema;

namespace Elagy.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        
        public string ImagePath { get; set; }


        // nav
        public ICollection<Pharmacy> pharmacies { get; set; }

        public ICollection<Order> orders { get; set; }

    }
}
