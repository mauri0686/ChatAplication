
using ChatBackend.Data;
using ChatBackend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
 
namespace ChatBackend.Controllers;

[Authorize]
[Route("api/[controller]")] 
[ApiController]
public class UsersController : ControllerBase
{
    private static ChatAppDbContext _context;
    
    public UsersController(ChatAppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var teams = await _context.Users.ToListAsync();
        return Ok(teams);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var team = await _context.Users.FirstOrDefaultAsync(x => x.Id == id.ToString());

        if (team == null)
            return BadRequest("Invalid Id");

        return Ok(team);
    }

    [HttpPost]
    public async Task<IActionResult> Post(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction("Get", user.Id, user);
    }

    [HttpPatch]
    public async Task<IActionResult> Patch(string id, string name)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
            return BadRequest("Invalid id");

        user.UserName = name;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
            return BadRequest("Invalid id");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}