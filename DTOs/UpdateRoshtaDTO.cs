using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
namespace Elagy.DTOs
{
    public class UpdateRoshta
    {

        public string Status { get; set; }
        public decimal price { get; set; }

    }
}
