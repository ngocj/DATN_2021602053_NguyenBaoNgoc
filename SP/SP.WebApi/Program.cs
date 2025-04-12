using Microsoft.EntityFrameworkCore;
using SP.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// cau hinh connectString

builder.Services.AddDbContext<SPContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SuperConnection"));
});

//dang ky


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
