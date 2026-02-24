using AutoMapper;
using ClosureServices.API.Middleware;
using ClosureServices.Application.Interfaces;
using ClosureServices.Application.Mapping;
using ClosureServices.Infrastructure.Data;
using ClosureServices.Infrastructure.External;
using ClosureServices.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        path: "Logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Message}{NewLine}",
        shared: true,
        flushToDiskInterval: TimeSpan.FromSeconds(1)
    )
    .CreateLogger();


Log.Information("Starting Loan API...");
QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbconn")));
builder.Services.AddScoped<IForeclosureRepo, ForClosureRepo>();
builder.Services.AddScoped<ILoanClosureRepo, LoanClosureRepo>();
builder.Services.AddAutoMapper(typeof(MapperConfig));

builder.Services.AddHttpClient<LoanAccountClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7164");
}
);
builder.Services.AddHttpClient<EmiClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7285");

}
);
builder.Services.AddHttpClient<LoanPaymentClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7137");
}
);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseGlobalExceptionHandler();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
