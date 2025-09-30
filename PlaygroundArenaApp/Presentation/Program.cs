using Microsoft.EntityFrameworkCore;
using PlaygroundArenaApp.Application.Mapping;
using PlaygroundArenaApp.Application.Middlewares.CustomGlobalExceptionHandler;
using PlaygroundArenaApp.Application.Middlewares.CustomSerilogLogging;
using PlaygroundArenaApp.Application.Services;
using PlaygroundArenaApp.Infrastructure.Data;
using PlaygroundArenaApp.Infrastructure.Repository.ArenaRepository;
using PlaygroundArenaApp.Infrastructure.Repository.BookingRepository;
using PlaygroundArenaApp.Infrastructure.Repository.CourtRepository;
using PlaygroundArenaApp.Infrastructure.Repository.UOW;
using Serilog;

var builder = WebApplication.CreateBuilder(args);



//Adding CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});


// Add services to the container.
builder.Services.AddControllers();


//COnfiguring Logging with Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

//Replacing the Default .Net logging with Serilog Logging
builder.Host.UseSerilog();


//Added a DB Context service to the container
builder.Services.AddDbContext<PlaygroundArenaDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


//Adding Exception Handler Globally
builder.Services.AddExceptionHandler<GlobalExceptionHandlerMiddleware>();
builder.Services.AddProblemDetails();


//Registring a AdminService & ArenInfoService
builder.Services.AddScoped<AdminArenaService>();
builder.Services.AddScoped<ArenaInformationService>();
builder.Services.AddScoped<IArenaRepository , ArenaRepository>();
builder.Services.AddScoped<ICourtRepository, CourtRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IUnitOfWork , UnitOfWork>();
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

app.UseCors("AllowReactApp");
app.UseHttpsRedirection();
app.UseMiddleware<SerilogLoggingMiddleware>();
app.UseExceptionHandler();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
