using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountController: ControllerBase
{
    private IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
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
}