using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WorkingOnJWT.Context;
using WorkingOnJWT.DtoS;
using WorkingOnJWT.Entities;
using WorkingOnJWT.TokenService;

namespace WorkingOnJWT.Controllers;

[Route("/api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    private readonly TokenGenerator _tokenGenerator;
    public UserController(AppDbContext context, TokenGenerator tokenGenerator)
    {
        _context = context;
        _tokenGenerator = tokenGenerator;
    }
    [HttpGet("Profile")]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _context.Users.FirstOrDefaultAsync(i => i.Id == userId);
        return Ok(user);
    }

    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUp([FromBody] CreateUserDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var user = dto.Adapt<User>();

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LogIn([FromBody] LoginUserModel dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var user = await _context.Users.FirstOrDefaultAsync(i => i.UserName == dto.UserName);
        if (user == null || user.Password != dto.Password)
        {
            ModelState.AddModelError("Username", "Username or password is incorrect");
            return NotFound();
        }
        var token = _tokenGenerator.GetToken(user);
        return Ok(token);
    }
}