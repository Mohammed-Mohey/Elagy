using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
namespace Elagy.DTOs
{
    public class AddPharmacyDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PharmacyName { get; set; }

        public string PharmacyPhone { get; set; }

        [Required]
        public string WorkingHours { get; set; }

        [Required]
        public string DeliveryArea { get; set; }

        [Required]
        public string ManagerName { get; set; }
        [Required]

        public string ManagerPhone { get; set; }

        [Required]
        public IFormFile TradeLicense { get; set; }

        [Required]
        public IFormFile TaxCard { get; set; }
        [Required]
        public IFormFile PharmacyLicense { get; set; }


    }
}
