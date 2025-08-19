using Elagy.DTOs;
using Elagy.Interfaces;
using Elagy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elagy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("/Registration")]
        public async Task<IActionResult> Registration([FromBody] RegisterUserDTO user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ResponseModel response = await _accountService.RegisterAsync(user);

            if (response == null)
                return BadRequest("An error occurred while processing your request.");

            return response.StatusCode switch
            {
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred , try again."),
            };
        }
        

        [Authorize(Roles = "Admin"), HttpPost("/RegistrationAsAdmin")]
        public async Task<IActionResult> RegistrationAsAdmin([FromBody] RegisterUserDTO user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ResponseModel response = await _accountService.RegistrationAsAdminAsync(user);

            if (response == null)
                return BadRequest("An error occurred while processing your request.");

            return response.StatusCode switch
            {
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred , try again."),
            };
        }

        [Authorize(Roles = "Pharmacy,Admin"), HttpPost("/RegistrationAsPharmacy")]
        public async Task<IActionResult> RegistrationAsPharmacy([FromBody] RegisterUserDTO pharmacy)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ResponseModel response = await _accountService.RegistrationAsPharmacyAsync(pharmacy);

            if (response == null)
                return BadRequest("An error occurred while processing your request.");

            return response.StatusCode switch
            {
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred , try again."),
            };
        }


        [HttpPost("/Login")]
        public async Task<IActionResult> Login(LoginUserDTo user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _accountService.LoginAsync(user);

            if (response == null)
                return BadRequest("An error occurred while processing your request.");

            return response.StatusCode switch
            {
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred , try again."),
            };
        }
        //[HttpPost("add")]
        //public async Task<IActionResult> AddPharmacy([FromForm] RegisterPharmacyDTO pharmacy)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var tradeLicensePath = Path.Combine("Uploads", pharmacy.TradeLicense.FileName);
        //    var taxCardPath = Path.Combine("Uploads", pharmacy.TaxCard.FileName);

        //    using (var stream = new FileStream(tradeLicensePath, FileMode.Create))
        //    {
        //        await pharmacy.TradeLicense.CopyToAsync(stream);
        //    }

        //    using (var stream = new FileStream(taxCardPath, FileMode.Create))
        //    {
        //        await pharmacy.TaxCard.CopyToAsync(stream);
        //    }

        //    return Ok(new { Message = "Pharmacy added successfully" });
        //}
        

        



    }
}
