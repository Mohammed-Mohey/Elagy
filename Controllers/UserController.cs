using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Elagy.Data;
using Elagy.DTOs;
using Elagy.Models;
namespace Elagy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        ApplicationDbContext context;
        public UserController(ApplicationDbContext _context)
        {
            context = _context;
        }
        [HttpGet("user")]
        public IActionResult GetUsers()
        {
            var users = context.Users.Where(u => !context.UserRoles.Any(ur => ur.UserId == u.Id)).Select(u => new
            {
                u.Id,
                Name = u.FirstName + u.LastName,
                u.Email,
                u.Location,
                u.PhoneNumber

            }).ToList();


            return Ok(users);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await context.users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            context.users.Remove(user);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("Pharmacies")]
        public IActionResult GetPharmacies()
        {
            var PharmacyRole = context.Roles.FirstOrDefault(r => r.Name == "Pharmacy");

            var pharmacies = context.Users
    .Where(u => context.UserRoles
        .Any(ur => ur.UserId == u.Id && ur.RoleId == PharmacyRole.Id))
    .Select(u => new
    {
        u.Id,
        Name = u.FirstName + " " + u.LastName,
        u.Email,
        u.Location,
        u.PhoneNumber
    })
    .ToList();

            return Ok(pharmacies);

        }
    }
}
