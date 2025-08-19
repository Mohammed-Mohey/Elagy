using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
namespace Elagy.DTOs
{
    public class OrderDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

    }
}
