namespace ChatBackend.Models;

public record AuthenticateRequest(string UserName, string Password);