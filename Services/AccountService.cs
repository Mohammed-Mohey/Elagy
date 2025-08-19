using Elagy.Data;
using Elagy.DTOs;
using Elagy.Helpers;
using Elagy.Interfaces;
using Elagy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Elagy.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtOptions _jwtOptions;
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;



        public AccountService(UserManager<User> userManager, JwtOptions jwtOptions, ApplicationDbContext context, IEmailService emailService)
        {
            _userManager = userManager;
            _jwtOptions = jwtOptions;
            _context = context;
            _emailService = emailService;

        }

        public async Task<ResponseModel> RegisterAsync(RegisterUserDTO data)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (await _userManager.FindByEmailAsync(data.Email) is not null)
                {
                    response.Message = "This email is not available";
                    response.StatusCode = 400;
                    return response;
                }

                var identityUser = new User
                {
                    UserName = Guid.NewGuid().ToString(),
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Email = data.Email,
                    PhoneNumber = data.PhoneNumber,
                    Location = data.Location,
                    //EmailConfirmed = false
                };

                var result = await _userManager.CreateAsync(identityUser, data.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Empty;

                    foreach (var error in result.Errors)
                        errors += $"{error.Description},";

                    response.Message = errors;
                    response.StatusCode = 400;
                    return response;
                }

                response.Message = "registration successfully.";
                response.StatusCode = 200;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = $"There was a problem Register in : {ex.Message}.";
                response.StatusCode = 500;
                return response;
            }

        }

        public async Task<ResponseModel> RegistrationAsAdminAsync(RegisterUserDTO data)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                //_context.pharmacies.Where(p => p.Email == data.Email);
                if (await _userManager.FindByEmailAsync(data.Email) is not null)
                {
                    response.Message = "This email is not available";
                    response.StatusCode = 400;
                    return response;
                }
                // pharmcy
                var identityUser = new User
                {
                    UserName = Guid.NewGuid().ToString(),
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Email = data.Email,
                    PhoneNumber = data.PhoneNumber,
                    Location = data.Location,
                    //EmailConfirmed = false
                };


                //_context.pharmacies.Add(user)
                var result = await _userManager.CreateAsync(identityUser, data.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Empty;

                    foreach (var error in result.Errors)
                        errors += $"{error.Description},";

                    response.Message = errors;
                    response.StatusCode = 400;
                    return response;
                }

                //Add Admin
                IdentityResult roleResult = await _userManager.AddToRoleAsync(identityUser, "Admin");
                if (!roleResult.Succeeded)
                {
                    // delete the user if adding to role failed
                    await _userManager.DeleteAsync(identityUser);
                    var errors = string.Empty;
                    foreach (var error in roleResult.Errors)
                        errors += $"{error.Description},";

                    response.Message = errors;
                    response.StatusCode = 400;
                    return response;
                }

                response.Message = "Admin created successfully.";
                response.StatusCode = 200;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = $"There was a problem Register in : {ex.Message}.";
                response.StatusCode = 500;
                return response;
            }
        }


        public async Task<ResponseModel> RegistrationAsPharmacyAsync(RegisterUserDTO data)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                //_context.pharmacies.Where(p => p.Email == data.Email);
                if (await _userManager.FindByEmailAsync(data.Email) is not null)
                {
                    response.Message = "This email is not available";
                    response.StatusCode = 400;
                    return response;
                }
                // pharmcy
                var identityUser = new User
                {
                    UserName = Guid.NewGuid().ToString(),
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Email = data.Email,
                    PhoneNumber = data.PhoneNumber,
                    Location = data.Location,
                    //EmailConfirmed = false
                };


                //_context.pharmacies.Add(user)
                var result = await _userManager.CreateAsync(identityUser, data.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Empty;

                    foreach (var error in result.Errors)
                        errors += $"{error.Description},";

                    response.Message = errors;
                    response.StatusCode = 400;
                    return response;
                }

                //Add Admin
                IdentityResult roleResult = await _userManager.AddToRoleAsync(identityUser, "Pharmacy");
                if (!roleResult.Succeeded)
                {
                    // delete the user if adding to role failed
                    await _userManager.DeleteAsync(identityUser);
                    var errors = string.Empty;
                    foreach (var error in roleResult.Errors)
                        errors += $"{error.Description},";

                    response.Message = errors;
                    response.StatusCode = 400;
                    return response;
                }

                response.Message = "Pharmacy created successfully.";
                response.StatusCode = 200;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = $"There was a problem Register in : {ex.Message}.";
                response.StatusCode = 500;
                return response;
            }
        }

        public async Task<ResponseModel> LoginAsync(LoginUserDTo data)
        {
            var user = await _userManager.FindByEmailAsync(data.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, data.Password))
                return new ResponseModel { Message = "Email or password is Incorrect.", StatusCode = 400 };

            //if (!user.EmailConfirmed)
            //    return new ResponseModel { Message = "please activate your account before Login.", StatusCode = 400 };

            var jwtSecurityToken = await CreateJwtTokenAsync(user);

            return new ResponseModel { StatusCode = 200, Message = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken) };
        }

        private async Task<JwtSecurityToken> CreateJwtTokenAsync(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim(ClaimTypes.Role, role));

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Name, user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.StreetAddress,user.Location),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            }.Union(userClaims).Union(roleClaims);


            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_jwtOptions.LifeTime),
                signingCredentials: signingCredentials
                );

            return jwtSecurityToken;
        }
       
        
        
    }
}
