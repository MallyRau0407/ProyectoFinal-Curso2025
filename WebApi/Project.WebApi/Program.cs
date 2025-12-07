using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Prometheus;
using Project.WebApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =====================
// Controllers & Swagger
// =====================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Project.WebApi",
        Version = "v1"
    });

    // JWT en Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa el token JWT así: Bearer {tu_token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ============
// HealthCheck
// ============
builder.Services.AddHealthChecks();

// ==============
// Custom services
// ==============
builder.Services.AddServices();

// ===================
// JWT CONFIGURACIÓN
// ===================
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];
var jwtKey = builder.Configuration["Jwt:Key"];

// 🔴 Validaciones críticas
if (string.IsNullOrWhiteSpace(issuer))
    throw new Exception("JWT Issuer no configurado");
if (string.IsNullOrWhiteSpace(audience))
    throw new Exception("JWT Audience no configurado");
if (string.IsNullOrWhiteSpace(jwtKey))
    throw new Exception("JWT Key no configurada");

// Autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // true en producción
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = issuer,
        ValidAudience = audience,

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey)
        ),

        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// =======
// Swagger
// =======
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// =========
// Prometheus
// =========
app.UseHttpMetrics();

// =====================
// Seguridad (ORDEN VITAL)
// =====================
app.UseAuthentication();
app.UseAuthorization();

// =================
// Endpoints extra
// =================
app.MapGet("/api/env", (IHostEnvironment env) =>
{
    return Results.Ok(new
    {
        environment = env.EnvironmentName,
        application = env.ApplicationName
    });
});

app.MapGet("/api/version", () =>
{
    var path = Path.Combine(AppContext.BaseDirectory, "VERSION");

    return File.Exists(path)
        ? Results.Ok(new { version = File.ReadAllText(path).Trim() })
        : Results.NotFound(new { error = "VERSION file not found" });
});

app.MapHealthChecks("/api/health");
app.MapMetrics("/api/metrics");

// ===========
// Controllers
// ===========
app.MapControllers();

app.Run();
