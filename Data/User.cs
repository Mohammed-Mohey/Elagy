using System.ComponentModel.DataAnnotations;
using Elagy.Models;
using Microsoft.AspNetCore.Identity;

namespace Elagy.Data
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public override string? UserName { get; set; }

        public string Location { get; set; }


        // nav
        public ICollection<Order> Orders { get; set; }
    }
}
