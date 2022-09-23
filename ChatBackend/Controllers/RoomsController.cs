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
public class RoomsController : ControllerBase
{
    private static ChatAppDbContext _context;
    
    public RoomsController(ChatAppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var teams = await _context.Rooms.ToListAsync();
        return Ok(teams);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var team = await _context.Rooms.FirstOrDefaultAsync(x => x.id == id);

        if (team == null)
            return BadRequest("Invalid Id");

        return Ok(team);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Room room)
    {
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction("Get", room.id, room);
    }

    [HttpPatch]
    public async Task<IActionResult> Patch(Guid id, string name)
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(x => x.id == id);

        if (room == null)
            return BadRequest("Invalid id");

        room.name = name;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(x => x.id == id);

        if (room == null)
            return BadRequest("Invalid id");

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}