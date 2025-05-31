using CafeVirtual.Pruebas.API.AutoMapper;
using CafeVirtual.Pruebas.API.Data.Interfaces;
using CafeVirtual.Pruebas.API.Data.UnitOfWork;
using CafeVirtual.Pruebas.API.Interfaces;
using CafeVirtual.Pruebas.API.Services;
using CafeVirtual.Pruebas.API.Services.Interfaces;
using CafeVirtual.Pruebas.API.Services.Services;
using CafeVirtual.Pruebas.API.Services.Settings;
using CafeVirtual.Pruebas.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using CafeVirtual.Pruebas.API.Validators;
using CafeVirtual.Pruebas.API.Utilidades.Settings;
using CafeVirtual.Pruebas.API.Utilidades.Interfaces;
using CafeVirtual.Pruebas.API.Utilidades.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<MvcContext>(options => options.UseSqlServer(builder.Configuration.
GetConnectionString("defaultConnection")));

// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingProfile));  // Registra el perfil de AutoMapper

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Servicios
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IProductoService, ProductoService>();

// Servicios de validacion de FluentValidation
builder.Services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<ProductoViewModelValidator>()); // Esta linea incluye las demas clases validator

builder.Services.AddFluentValidationAutoValidation();


// Servicio Log
builder.Services.Configure<LogSettings>(builder.Configuration.GetSection("LogSettings"));
builder.Services.AddSingleton<ILogService, LogService>();

// Configuracion de JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped<IJwtService, JwtService>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sales API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });
});


//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sales API v1");
        c.RoutePrefix = string.Empty;
    });
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
