using ChatBackend.Data;
using ChatBackend.Models;
using ChatService;
using ChatService.Hub;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
 
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost/")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        
    });
});

var connectionString = builder.Configuration.GetConnectionString("Conn");
builder.Services.AddDbContext<ChatAppDbContext>(options => 
    options.UseSqlite(connectionString));
builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ChatAppDbContext>();

builder.Services.AddSingleton<IDictionary<string, UserConnection>>(opts => new Dictionary<string, UserConnection>());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await SeedData();
app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoint => { endpoint.MapHub<ChatHub>("/Chat"); });
app.MapPost("/token", async (AuthenticateRequest request, UserManager<User> userManager) =>
{
    // Verificamos credenciales con Identity
    var user = await userManager.FindByNameAsync(request.UserName);

    if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
    {
        return Results.Forbid();
    }

    var roles = await userManager.GetRolesAsync(user);

    // Generamos un token según los claims
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Sid, user.Id),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.GivenName, $"{user.NormalizedUserName}")
    };

    foreach (var role in roles)
    {
        claims.Add(new Claim(ClaimTypes.Role, role));
    }

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
    var tokenDescriptor = new JwtSecurityToken(
        issuer: builder.Configuration["Jwt:Issuer"],
        audience: builder.Configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(720),
        signingCredentials: credentials);

    var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

    return Results.Ok(new
    {
        AccessToken = jwt
    });
});
async Task SeedData()
{
    var scopeFactory = app!.Services.GetRequiredService<IServiceScopeFactory>();
    using var scope = scopeFactory.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<ChatAppDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    context.Database.EnsureCreated();

    if (!userManager.Users.Any())
    {
        logger.LogInformation("Creating a test User");

        var newUser = new User
        {
            Email = "test@demo.com",
            UserName = "Test",
        };

        await userManager.CreateAsync(newUser, "P@sword");
        await roleManager.CreateAsync(new IdentityRole
        {
            Name = "Admin"
        });
        await roleManager.CreateAsync(new IdentityRole
        {
            Name = "AnotherRole"
        });

        await userManager.AddToRoleAsync(newUser, "Admin");
        await userManager.AddToRoleAsync(newUser, "AnotherRole");
    }
}
app.Run();