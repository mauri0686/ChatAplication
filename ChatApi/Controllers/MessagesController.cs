
using ChatDomain.Models;
using ChatInfrastruncture.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatBackend.Controllers;


[Route("api/[controller]")] // api/teams
[ApiController]
public class MessagesController : ControllerBase
{
    private static MessageService? _messageService;
    public MessagesController(MessageService messageservice)
    {
        _messageService = messageservice;
    }
 
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
 
        var message = await _messageService.Get(id);
        if (message == null)
            return BadRequest("Invalid Id");

        return Ok(message);
    }
   
    [HttpGet]
    public async Task<IActionResult> Get()
    {
 
        var message =  await _messageService.GetRoomMessageLimit(1,50).ToListAsync();
        if (message == null)
            return BadRequest("Invalid Id");

        return Ok(message);
    }
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost]
    public async Task<IActionResult> Post(Message message)
    {
        await _messageService.Add(message);
        await _messageService.SaveAsync();
        
        return CreatedAtAction("Get", message.id, message);
    }
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
       
        var message = await _messageService.Get(id);

        if (message == null)
            return BadRequest("Invalid id");

        await _messageService.Delete(message);
        await _messageService.SaveAsync();

        return NoContent();
    }
}