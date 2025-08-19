using Elagy.DTOs;
using Elagy.Data;
using Elagy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;


[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    ApplicationDbContext context;
    public CartController(ApplicationDbContext _context)
    {
        context = _context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderWithProductsDTO req)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var order = new Order
        {
            UserName = req.UserName,
            Address = req.Address,
            SpeicalLocation = req.SpeicalLocation,
            PhoneNumber = req.PhoneNumber,
            Date = DateTime.Now,
            Status = "قيد المعالجة",
             OrderItems=req.Items.Select(i=>new OrderItem
             {
                 ProductID=i.ProductId,
                 Quantity=i.Quantity,
             }).ToList()
        };
        context.orders.Add(order);
        await context.SaveChangesAsync();
        return Ok(new {order.Id});
       
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await context.orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).ToListAsync();
        var res = orders.Select(o => new
        {
            o.Id,
            o.UserName,
            o.PhoneNumber,
            o.Address,
            o.SpeicalLocation,
            o.Status,
            o.Date,
            Items = o.OrderItems.Select(i => new
            {
                i.ProductID,
                ProductName = i.Product.Name,
                i.Quantity,
                PriceProduct=i.Product.Price
            })
        });
        return Ok(res);
    }
    
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateStatusDTO UpStatus)
    {
        var order = context.orders.Find(id);
        order.Status = UpStatus.Status;
        context.SaveChanges();
        return Ok(order);
    }
    
   
}