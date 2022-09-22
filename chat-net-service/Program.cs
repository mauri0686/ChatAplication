using ChatService;
using ChatService.Hub;
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
builder.Services.AddSingleton<IDictionary<string, UserConnection>>(opts => new Dictionary<string, UserConnection>());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

 
app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoint => { endpoint.MapHub<ChatHub>("/Chat"); });
app.Run();