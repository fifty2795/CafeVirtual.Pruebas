using CafeVirtual.Pruebas.Business.API.Interfaces;
using CafeVirtual.Pruebas.Business.API.Servicios;
using CafeVirtual.Pruebas.Business.Interfaces;
using CafeVirtual.Pruebas.Business.Servicios;
using CafeVirtual.Pruebas.Business.Settings;
using CafeVirtual.Pruebas.Data.Interfaces;
using CafeVirtual.Pruebas.Data.Models;
using CafeVirtual.Pruebas.Data.UnitOfWork;
using CafeVirtual.Pruebas.Utilidades.Email;
using CafeVirtual.Pruebas.Utilidades.Interfaces;
using CafeVirtual.Pruebas.Utilidades.Model;
using CafeVirtual.Pruebas.Utilidades.Utilidades;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configurar contexto de base de datos
builder.Services.AddDbContext<MvcContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")));

// Ignorar ciclos de referencia en respuestas JSON
builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Configurar Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configurar autenticación basada en Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;

        // Rutas de login y logout
        options.LoginPath = "/Login/Index";
        options.LogoutPath = "/Login/Logout";
        options.AccessDeniedPath = "/Login/AccessDenied";
    });

// Servicios de aplicación
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<IClienteService, ClienteService>();

// Servicio Email
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

// Servicio Log
builder.Services.Configure<LogSettings>(builder.Configuration.GetSection("LogSettings"));
builder.Services.AddSingleton<ILogService, LogService>();

// Servicios API
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.AddHttpClient<IApiAuthService, ApiAuthService>();
builder.Services.AddHttpClient<IApiProductoService, ApiProductoService>();


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
