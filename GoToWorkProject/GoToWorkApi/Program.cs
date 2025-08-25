using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GoToWorkApi;
using GoToWorkDatabase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

using var loggerFactory = new LoggerFactory();
loggerFactory.AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger());
builder.Services.AddSingleton(loggerFactory.CreateLogger("Any"));

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
            ValidateIssuerSigningKey = true,
        };
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

if (app.Environment.IsProduction())
{
    var dbContext = app.Services.GetRequiredService<GoToWorkDbContext>();
    if (dbContext.Database.CanConnect())
    {
        dbContext.Database.EnsureCreated();
        dbContext.Database.Migrate();
    }
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Map("/login/{username}", (string username) =>
    new JwtSecurityTokenHandler()
        .WriteToken(new JwtSecurityToken(
            AuthOptions.Issuer,
            AuthOptions.Audience,
            [new Claim(ClaimTypes.Name, username)],
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(20)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256))));

app.MapControllers();

app.Run();