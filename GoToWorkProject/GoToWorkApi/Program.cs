using GoToWorkApi;
using GoToWorkApi.Adapters;
using GoToWorkApi.Infrastructure;
using GoToWorkBusinessLogic.Implementations;
using GoToWorkContracts.AdapterContracts;
using GoToWorkContracts.BusinessLogicContracts;
using GoToWorkContracts.Infrastructure;
using GoToWorkContracts.StoragesContracts;
using GoToWorkDatabase;
using GoToWorkDatabase.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Logging
using var loggerFactory = new LoggerFactory();
loggerFactory.AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger());
builder.Services.AddSingleton(loggerFactory.CreateLogger("Any"));

// DbContext
builder.Services.AddSingleton<IConfigurationDatabase, ConfigurationDatabase>();
builder.Services.AddDbContext<GoToWorkDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection
// Storages
builder.Services.AddScoped<IDetailStorageContract, DetailStorageContract>();
builder.Services.AddScoped<IEmployeeStorageContract, EmployeeStorageContract>();
builder.Services.AddScoped<IMachineStorageContract, MachineStorageContract>();
builder.Services.AddScoped<IProductStorageContract, ProductStorageContract>();
builder.Services.AddScoped<IProductionStorageContract, ProductionStorageContract>();
builder.Services.AddScoped<IUserStorageContract, UserStorageContract>();
builder.Services.AddScoped<IWorkshopStorageContract, WorkshopStorageContract>();

// Business Logic
builder.Services.AddScoped<IDetailBusinessLogicContract, DetailBusinessLogicContract>();
builder.Services.AddScoped<IEmployeeBusinessLogicContract, EmployeeBusinessLogicContract>();
builder.Services.AddScoped<IMachineBusinessLogicContract, MachineBusinessLogicContract>();
builder.Services.AddScoped<IProductBusinessLogicContract, ProductBusinessLogicContract>();
builder.Services.AddScoped<IProductionBusinessLogicContract, ProductionBusinessLogicContract>();
builder.Services.AddScoped<IUserBusinessLogicContract, UserBusinessLogicContract>();
builder.Services.AddScoped<IWorkshopBusinessLogicContract, WorkshopBusinessLogicContract>();
builder.Services.AddScoped<IReportContract, ReportContract>();

// Adapters
builder.Services.AddScoped<IDetailAdapter, DetailAdapter>();
builder.Services.AddScoped<IEmployeeAdapter, EmployeeAdapter>();
builder.Services.AddScoped<IMachineAdapter, MachineAdapter>();
builder.Services.AddScoped<IProductAdapter, ProductAdapter>();
builder.Services.AddScoped<IProductionAdapter, ProductionAdapter>();
builder.Services.AddScoped<IUserAdapter, UserAdapter>();
builder.Services.AddScoped<IWorkshopAdapter, WorkshopAdapter>();
builder.Services.AddScoped<IReportAdapter, ReportAdapter>();

builder.Services.AddControllers();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = AuthOptions.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true
        };
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

if (app.Environment.IsProduction())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<GoToWorkDbContext>();
    if (dbContext.Database.CanConnect())
    {
        dbContext.Database.EnsureCreated();
        dbContext.Database.Migrate();
    }
}

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = app.Environment.IsProduction() ? CookieSecurePolicy.Always : CookieSecurePolicy.None
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();