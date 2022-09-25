
using ChatDomain.Models;
using ChatInfrastruncture.Data;
using ChatInfrastruncture.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatBackend.Controllers;

[Authorize]
[Route("api/[controller]")] 
[ApiController]
public class UsersController : ControllerBase
{
    private static UserService? _userService;
    
    public UsersController(UserService? userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var users = await _userService.Get();
        return Ok(users);
    }

    [HttpGet("{email}")]
    public async Task<IActionResult> Get(string email)
    {
        var user = await _userService.GetByEmail(email);

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Post(User user)
    {
        await  _userService.Add(user);
        await _userService.SaveAsync();
        
        return CreatedAtAction("Get", user.id, user);
    }

    [HttpPatch]
    public async Task<IActionResult> Patch(string id, string name)
    {
     
        var user = await _userService.Get(id);

        if (user == null)
            return BadRequest("Invalid id");

        user.UserName = name;
        await _userService.Update(user);
        await _userService.SaveAsync();

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userService.Get(id);
        if (user == null)
            return BadRequest("Invalid id");

        await _userService.Delete(user);
        await _userService.SaveAsync();

        return NoContent();
    }
}