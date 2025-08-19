using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
namespace Elagy.DTOs
{
    public class UserDTO
    {
        public string Name { get; set; }

        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }


    }
}
