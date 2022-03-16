using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MovieShopAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountController: ControllerBase
{
    private IAccountService _accountService;

    private IConfiguration _configuration;
    
    public AccountController(IAccountService accountService, IConfiguration configuration)
    {
        _accountService = accountService;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(UserRegisterRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            // 400 bad request
            return BadRequest();
        }
        var user = await _accountService.CreateUser(model);
        if (user == null) return BadRequest();
        return Ok(user);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(UserLoginRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var user = await _accountService.ValidateUser(model);
        if (user == null)
        {
            return Unauthorized();
        }

        var token = GenerateToken(user);
        return Ok(user);
    }

    private string GenerateToken(LoginResponseModel user)
    {
        //create claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(JwtRegisteredClaimNames.Birthdate, user.DateOfBirth.ToShortDateString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new("Language", "en")
        };
        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);
        
        //return token with secret signatures
        //exp time
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var expirationTime = DateTime.UtcNow.AddHours(_configuration.GetValue<int>("ExpirationHours"));

        var tokenHandler = new JwtSecurityTokenHandler();
        
        //Describe token contents

        var token = new SecurityTokenDescriptor
        {
            Subject = identityClaims,
            Expires = expirationTime,
            SigningCredentials = credentials,
            Issuer = _configuration["Issuer"],
            Audience = _configuration["Audience"]
        };

        var encodedJwt = tokenHandler.CreateToken(token);
        return tokenHandler.WriteToken(encodedJwt);
    }
}