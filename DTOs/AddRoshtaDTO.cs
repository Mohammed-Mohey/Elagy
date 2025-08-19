using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
namespace Elagy.DTOs
{
    public class AddRoshtaDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string SpeicalLocation { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public string Status { get; set; }
        public decimal price { get; set; } = 0;
        public IFormFile Image { get; set; }
    }
}
