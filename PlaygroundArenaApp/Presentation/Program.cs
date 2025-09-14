using Microsoft.EntityFrameworkCore;
using PlaygroundArenaApp.Application.Mapping;
using PlaygroundArenaApp.Application.Services;
using PlaygroundArenaApp.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


//Added a DB Context service to the container
builder.Services.AddDbContext<PlaygroundArenaDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


//Registring a AdminService & ArenInfoService
builder.Services.AddScoped<AdminArenaService>();
builder.Services.AddScoped<ArenaInformationService>();

//registered Auto Mapper
builder.Services.AddAutoMapper(typeof(AutoMapping));


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
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
