using Microsoft.EntityFrameworkCore;
using SocketChat.BLL.Logic;
using SocketChat.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddScoped<IUserLogic, UserLogic>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ChatContext>();//(options =>
  //  options.UseNpgsql(connection));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.MapHub<ChatHub>("/chat");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
