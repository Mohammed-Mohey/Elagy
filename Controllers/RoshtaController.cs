using Elagy.DTOs;
using Elagy.Data;
using Elagy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;


[ApiController]
[Route("api/[controller]")]
public class RoshtaController : ControllerBase
{
    ApplicationDbContext context;
    public readonly IWebHostEnvironment env;

    public RoshtaController(ApplicationDbContext _context, IWebHostEnvironment _env)
    {
        context = _context;
        env = _env;

    }

    [HttpGet]

    public IActionResult GetAllRoshta()
    {
        List<Roshta> roshtat = context.roshtat.ToList();
        return Ok(roshtat);

    }
    [HttpPost]
    public async Task<IActionResult> CreateRoshta(AddRoshtaDTO req)
    {
        string? imagepath = null;

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (req.Image != null)
        {
            var fileName = $"{Guid.NewGuid()}-{Path.GetFileName(req.Image.FileName)}";
            var filePath = Path.Combine(env.WebRootPath, "Uploads", fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await req.Image.CopyToAsync(stream);
            }
            imagepath = Path.Combine("Uploads", fileName);
        }
        var roshta = new Roshta
        {
            UserName = req.UserName,
            Address = req.Address,
            SpeicalLocation = req.SpeicalLocation,
            PhoneNumber = req.PhoneNumber,
            Date = DateTime.Now,
            Status = "قيد المعالجة",
            price = 0,
            ImagePath = imagepath


        };
        context.roshtat.Add(roshta);
        await context.SaveChangesAsync();
        return Ok(new { roshta.Id });

    }
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateStatusRoshta(int id, UpdateRoshta UpStatus)
    {
        var roshta = context.roshtat.Find(id);
        roshta.Status = UpStatus.Status;
        roshta.price = UpStatus.price;
        context.SaveChanges();
        return Ok(roshta);
    }

}