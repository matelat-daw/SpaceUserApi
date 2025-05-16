using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using SpaceUserAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SpaceUserAPI.Interface;
using SpaceUserAPI.Models.User;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration; // Configuración de la Aplicación.

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp")); // Configuración del Servidor de Correo.

builder.Services.Configure<SmtpSettings>(option =>
{
    option.Password = Environment.GetEnvironmentVariable("Gmail-Nexus"); // Contraseña del Servidor de Correo.
});

builder.Services.AddTransient<IEmailSender, EmailSender>(); // Servicio de Correo.

var conn = builder.Configuration.GetConnectionString("conn"); // Conexión con la Base de Datos de Usuarios/Productos/Articulos/Servicios.
var pass = Environment.GetEnvironmentVariable("SQL-SERVER"); // Contraseña de la Base de Datos de Usuarios/Productos/Articulos/Servicios.
var fullConn = $"{conn};Password={pass}";

// builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(fullConn)); // Conexión con la Base de Datos de Usuarios/Productos/Articulos/Servicios.
builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(conn));

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); // Para Evitar que los JSON Hagan un Bucle Infinito.

builder.Services.AddIdentity<SpaceUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddDefaultTokenProviders()
.AddEntityFrameworkStores<UserContext>(); // AddIdentity Agrega todos los servicios, Roles, SingInManager, Etc. para la Autenticación de Usuarios.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    }) // Método para la Autenticación por Token.
    .AddGoogle(options =>
    {
        options.ClientId = Environment.GetEnvironmentVariable("Google-Client-Id")!;
        options.ClientSecret = Environment.GetEnvironmentVariable("Google-Client-Secret")!;
    })
    .AddMicrosoftAccount(microsoftOptions =>
    {
        microsoftOptions.ClientId = Environment.GetEnvironmentVariable("Microsoft-Client-Id")!;
        microsoftOptions.ClientSecret = Environment.GetEnvironmentVariable("Microsoft-Client-Secret")!;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Pega el Token del Usuario Logueado",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
}); // Este Método Habilita Swagger para Hacer la Pruebas de la API Autenticando Usuarios con el Token.

var AllowCors = "AllowCors";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowCors, policy =>
    {
        policy.AllowAnyOrigin() // Permitir cualquier origen.
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Logging.ClearProviders(); // Limpia los proveedores de registro predeterminados.
builder.Logging.AddConsole(); // Agrega el registro en la consola.
builder.Logging.AddDebug(); // Agrega el registro en la ventana de depuración.


var app = builder.Build();

app.UseCors(AllowCors); // Habilita CORS para la API.

app.Use(async (context, next) =>
{
context.Response.Headers.Remove("Cross-Origin-Opener-Policy");
context.Response.Headers.Remove("Cross-Origin-Embedder-Policy");
// Solo para desarrollo, puedes agregar:
// context.Response.Headers["Cross-Origin-Opener-Policy"] = "unsafe-none";
await next();
});

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

app.Run();