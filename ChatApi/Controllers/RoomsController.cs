
using ChatDomain.Models;
using ChatInfrastruncture.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatBackend.Controllers;

[Route("api/[controller]")] // api/teams
[ApiController]
public class RoomsController : ControllerBase
{
    private static RoomService? _roomService;
    public RoomsController(RoomService roomService)
    {
        _roomService = roomService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var rooms = await _roomService.Get();
        return Ok(rooms);
    }
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
 
        var room = await _roomService.Get(id);
        if (room == null)
            return BadRequest("Invalid Id");

        return Ok(room);
    }
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost]
    public async Task<IActionResult> Post(Room room)
    {
        await _roomService.Add(room);
        await _roomService.SaveAsync();
        
        return CreatedAtAction("Get", room.id, room);
    }
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPatch]
    public async Task<IActionResult> Patch(int id, string name)
    {
        var room = await _roomService.Get(id);

        if (room == null)
            return BadRequest("Invalid id");

        room.name = name;

        await _roomService.SaveAsync();

        return NoContent();
    }
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
       
        var room = await _roomService.Get(id);

        if (room == null)
            return BadRequest("Invalid id");

        await _roomService.Delete(room);
        await _roomService.SaveAsync();

        return NoContent();
    }
}