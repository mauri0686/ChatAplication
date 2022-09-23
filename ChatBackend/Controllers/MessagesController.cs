using System.IdentityModel.Tokens.Jwt;
using System.Net;
using ChatBackend.Data;
using ChatBackend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatBackend.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")] // api/teams
[ApiController]
public class MessagesController : ControllerBase
{
    private static ChatAppDbContext _context;
    
    public MessagesController(ChatAppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var teams = await _context.Messages.ToListAsync();
        return Ok(teams);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var team = await _context.Messages.FirstOrDefaultAsync(x => x.id == id);

        if (team == null)
            return BadRequest("Invalid Id");

        return Ok(team);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Message message)
    {
        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction("Get", message.id, message);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var message = await _context.Messages.FirstOrDefaultAsync(x => x.id == id);

        if (message == null)
            return BadRequest("Invalid id");

        _context.Messages.Remove(message);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}