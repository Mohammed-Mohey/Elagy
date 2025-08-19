
using Elagy.Data;
using Elagy.DTOs;
using Elagy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Elagy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        ApplicationDbContext context;
        public readonly IWebHostEnvironment env;
        public ProductController(ApplicationDbContext _context, IWebHostEnvironment _env)
        {
            context = _context;
            env = _env;
        }
        [HttpGet]
        public IActionResult GetProducts()
        {
            List<Product> product = context.products.ToList();
            return Ok(product);
           
        }
        
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetProductById(int id)
        {
            Product product = context.products.Include(d => d.pharmacies).FirstOrDefault(d => d.Id == id);
            MedicineWithPharmacyDTO MedDto = new MedicineWithPharmacyDTO();
            MedDto.Id = product.Id;
            MedDto.Name = product.Name;
            MedDto.Description = product.Description;
            MedDto.Price = product.Price;
            MedDto.Quantity = product.Quantity;
            MedDto.ImagePath = product.ImagePath;
            foreach (var item in product.pharmacies)
            {
                MedDto.PharmaciesName.Add(item.PharmacyName);
            }
            return Ok(MedDto);
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Product name must be provided.");
            }

            var products = await context.products
                .Include(p => p.pharmacies)
                .Where(p => p.Name.Contains(name))
                .ToListAsync();

            if (products == null || products.Count == 0)
            {
                return NotFound($"No products found containing: {name}");
            }

            var result = products.Select(product => new MedicineWithPharmacyDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                ImagePath = product.ImagePath,
                PharmaciesName = product.pharmacies.Select(ph => ph.PharmacyName).ToList()
            }).ToList();

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductDTO addProductDTO)
        {
            string? imagepath = null;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(addProductDTO.Image != null) 
            {
                var fileName = $"{Guid.NewGuid()}-{Path.GetFileName(addProductDTO.Image.FileName)}";
                var filePath = Path.Combine(env.WebRootPath, "Uploads", fileName);
                using (var stream=new FileStream(filePath,FileMode.Create))
                {
                    await addProductDTO.Image.CopyToAsync(stream);
                }
                imagepath = Path.Combine("Uploads", fileName);
            }
            var product = new Product
            {
                Name = addProductDTO.Name,
            Description = addProductDTO.Description,
            Price = addProductDTO.Price,
            Quantity = addProductDTO.Quantity,
            ImagePath=imagepath

            };
            context.products.Add(product);

            await context.SaveChangesAsync();
            return Ok(product);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await context.products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            context.products.Remove(product);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, AddProductDTO updateProduct) 
        {
            var product = context.products.Find(id);
            if (product is null)
            {
                return NotFound();
            }
            if(updateProduct.Image!=null)
            {
                var fileName=$"{Guid.NewGuid()}-{Path.GetFileName(updateProduct.Image.FileName)}";
                var filePath = Path.Combine(env.WebRootPath, "Uploads", fileName);
                if(!string.IsNullOrEmpty(product.ImagePath))
                {
                    var oldFilePath=Path.Combine(env.WebRootPath,product.ImagePath.Replace('/','\\'));
                    if(System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await updateProduct.Image.CopyToAsync(stream);
                }
                product.ImagePath = Path.Combine("Uploads", fileName);
            }
            product.Name = updateProduct.Name;
            product.Description = updateProduct.Description;
            product.Price = updateProduct.Price;
            product.Quantity = updateProduct.Quantity;
            context.SaveChanges();
            return Ok(product);
        }
       
    }
}