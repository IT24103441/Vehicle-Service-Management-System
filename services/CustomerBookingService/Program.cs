using System.Text;
using CustomerBookingService.Data;
using CustomerBookingService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// DATABASE
// ======================================================

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "ConnectionStrings:DefaultConnection is missing."
    );
}

builder.Services.AddDbContext<CustomerBookingDbContext>(
    options =>
        options.UseMySQL(connectionString)
);

// ======================================================
// AUTHENTICATION SERVICES
// ======================================================

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddSingleton<IBookingEventPublisher, BookingEventPublisher>();
builder.Services.AddSingleton<ICheckInEventPublisher, CheckInEventPublisher>();

var jwtKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException(
        "Jwt:Key is missing."
    );
}

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

                ValidAudience =
                    builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)
                    ),

                ClockSkew = TimeSpan.Zero
            };
    });

builder.Services.AddAuthorization();

// ======================================================
// CONTROLLERS / OPENAPI
// ======================================================

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// ======================================================
// CORS FOR REACT
// ======================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowReactFrontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173", "http://144.24.106.68:8080")
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    );
});

// ======================================================
// APPLICATION
// ======================================================

var app = builder.Build();

// Environment check එක නැතුව Production වලත් OpenAPI ක්‍රියාත්මක වීමට:
app.MapOpenApi();

// Root URL එකට එන විට OpenAPI JSON වෙත redirect කිරීම සඳහා (නිවැරදි ක්‍රමය):
app.MapGet("/", () => Results.Redirect("/openapi/v1.json"));

// app.UseHttpsRedirection(); // Disabled for HTTP development — HTTPS redirect breaks frontend HTTP requests

app.UseCors("AllowReactFrontend");

// Must be in this order
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();