using Elagy.Data;
using Elagy.DTOs;
using Elagy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Elagy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PharmacyController : Controller
    {
        ApplicationDbContext context;
        public readonly IWebHostEnvironment env;
        public PharmacyController(ApplicationDbContext _context, IWebHostEnvironment _env)
        {
            context = _context;
            env = _env;
        }
        [HttpGet]
        public IActionResult GetPharmacy()
        {
            List<Pharmacy> pharmacies = context.pharmacies.ToList();
            return Ok(pharmacies);
        }
        [HttpGet("Nearby")]
        public async Task<IActionResult> GetNearstPharmacy(string Address)
        {
            var PharmacyRole = context.Roles.FirstOrDefault(r => r.Name == "Pharmacy");

            var Pharmacies = await context.Users
                .Where(u =>
                    context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == PharmacyRole.Id)
                    && u.Location == Address)
                .Select(u => new
                {
                    u.Id,
                    Name = u.FirstName + " " + u.LastName,
                    u.Email,
                    u.Location,
                    u.PhoneNumber
                })
                .ToListAsync();

            return Ok(Pharmacies);
        }


        [HttpGet]
        [Route("{id}")]
        public IActionResult GetPharmacyById(int id)
        {
            Pharmacy pharmacy = context.pharmacies.FirstOrDefault(d => d.Id == id);
            if (pharmacy is null)
            {
                return NotFound();
            }
            return Ok(pharmacy);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddPharmacy(AddPharmacyDTO pharmacyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string? taxCardPath = null;
            string? pharmacyLicensePath = null;
            string? tradeLicensePath = null;
            if(pharmacyDto.TaxCard!=null)
            {
                var fileName = $"{Guid.NewGuid()}-{Path.GetFileName(pharmacyDto.TaxCard.FileName)}";
                var filePath = Path.Combine(env.WebRootPath, "PharmaImages", fileName);
                using(var stream=new FileStream(filePath,FileMode.Create))
                {
                    await pharmacyDto.TaxCard.CopyToAsync(stream);
                }
                taxCardPath = Path.Combine("PharmaImages", fileName);
            }
            if (pharmacyDto.PharmacyLicense != null)
            {
                var fileName = $"{Guid.NewGuid()}-{Path.GetFileName(pharmacyDto.PharmacyLicense.FileName)}";
                var filePath = Path.Combine(env.WebRootPath, "PharmaImages", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await pharmacyDto.PharmacyLicense.CopyToAsync(stream);
                }
                pharmacyLicensePath = Path.Combine("PharmaImages", fileName);
            }
            if (pharmacyDto.TradeLicense != null)
            {
                var fileName = $"{Guid.NewGuid()}-{Path.GetFileName(pharmacyDto.TradeLicense.FileName)}";
                var filePath = Path.Combine(env.WebRootPath, "PharmaImages", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await pharmacyDto.TradeLicense.CopyToAsync(stream);
                }
                tradeLicensePath = Path.Combine("PharmaImages", fileName);
            }
            var pharmacy = new Pharmacy
            {
                PharmacyName = pharmacyDto.PharmacyName,
                Email = pharmacyDto.Email,
                Password = pharmacyDto.Password,
                PharmacyPhone = pharmacyDto.PharmacyPhone,
                WorkingHours = pharmacyDto.WorkingHours,
                DeliveryArea = pharmacyDto.DeliveryArea,
                ManagerName = pharmacyDto.ManagerName,
                ManagerPhone = pharmacyDto.ManagerPhone,
                TaxCard=taxCardPath,
                TradeLicense=tradeLicensePath,
                PharmacyLicense=pharmacyLicensePath
            };
            context.pharmacies.Add(pharmacy);

            context.SaveChanges();
            return Ok(pharmacy);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePharmacy(int id)
        {
            var pharmacy = await context.pharmacies.FindAsync(id);
            if (pharmacy == null)
            {
                return NotFound();
            }
            context.pharmacies.Remove(pharmacy);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PutItem(int id, AddPharmacyDTO updatePharmacyDTO)
        {
            var pharma=context.pharmacies.Find(id);
            if (pharma is  null)
            {
                return NotFound();
            }
            if (updatePharmacyDTO.TaxCard != null)
            {
                var fileName = $"{Guid.NewGuid()}-{Path.GetFileName(updatePharmacyDTO.TaxCard.FileName)}";
                var filePath = Path.Combine(env.WebRootPath, "PharmaImages", fileName);
                if (!string.IsNullOrEmpty(pharma.TaxCard))
                {
                    var oldFilePath = Path.Combine(env.WebRootPath, pharma.TaxCard.Replace('/', '\\'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await updatePharmacyDTO.TaxCard.CopyToAsync(stream);
                }
                pharma.TaxCard = Path.Combine("PharmaImages", fileName);
            }
            if (updatePharmacyDTO.PharmacyLicense != null)
            {
                var fileName = $"{Guid.NewGuid()}-{Path.GetFileName(updatePharmacyDTO.PharmacyLicense.FileName)}";
                var filePath = Path.Combine(env.WebRootPath, "PharmaImages", fileName);
                if (!string.IsNullOrEmpty(pharma.PharmacyLicense))
                {
                    var oldFilePath = Path.Combine(env.WebRootPath, pharma.PharmacyLicense.Replace('/', '\\'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await updatePharmacyDTO.PharmacyLicense.CopyToAsync(stream);
                }
                pharma.PharmacyLicense = Path.Combine("PharmaImages", fileName);
            }
            if (updatePharmacyDTO.TradeLicense != null)
            {
                var fileName = $"{Guid.NewGuid()}-{Path.GetFileName(updatePharmacyDTO.TradeLicense.FileName)}";
                var filePath = Path.Combine(env.WebRootPath, "PharmaImages", fileName);
                if (!string.IsNullOrEmpty(pharma.TradeLicense))
                {
                    var oldFilePath = Path.Combine(env.WebRootPath, pharma.TradeLicense.Replace('/', '\\'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await updatePharmacyDTO.TradeLicense.CopyToAsync(stream);
                }
                pharma.TradeLicense = Path.Combine("PharmaImages", fileName);
            }
            pharma.PharmacyName = updatePharmacyDTO.PharmacyName;
            pharma.PharmacyPhone = updatePharmacyDTO.PharmacyPhone;
            pharma.ManagerName=updatePharmacyDTO.ManagerName;
            pharma.ManagerPhone=updatePharmacyDTO.ManagerPhone;
            pharma.DeliveryArea=updatePharmacyDTO.DeliveryArea;
            pharma.WorkingHours=updatePharmacyDTO.WorkingHours;
            pharma.Email=updatePharmacyDTO.Email;
            pharma.Password=updatePharmacyDTO.Password;
            context.SaveChanges();
            return Ok(pharma);
        }
        
    }
}