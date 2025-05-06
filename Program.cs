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
    option.Password = Environment.GetEnvironmentVariable("Gmail-Matelat"); // Contraseña del Servidor de Correo.
    //option.Password = "cmpazrwvngjpmbhw";
});

builder.Services.AddTransient<IEmailSender, EmailSender>(); // Servicio de Correo.
builder.Services.AddTransient<ITranslator, Translator>(); // Servicio de Traducción de Texto e HTML.

//var conn = builder.Configuration.GetConnectionString("conn"); // Conexión con la Base de Datos de Usuarios/Productos/Articulos/Servicios.
//var pass = Environment.GetEnvironmentVariable("SQL-SERVER"); // Contraseña de la Base de Datos de Usuarios/Productos/Articulos/Servicios.
//var fullConn = $"{conn};Password={pass}";

builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("conn"))); // Conexión con la Base de Datos de Usuarios/Productos/Articulos/Servicios.

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
        options.CallbackPath = "/signin-google";
    })
    .AddMicrosoftAccount(microsoftOptions =>
    {
        microsoftOptions.ClientId = Environment.GetEnvironmentVariable("Microsoft-Client-Id")!;
        microsoftOptions.ClientSecret = Environment.GetEnvironmentVariable("Microsoft-Client-Secret")!;
        microsoftOptions.CallbackPath = "/signin-microsoft";
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("WithOriginsPolicy", policy =>
    {
        policy.WithOrigins("https://nexus-astralis-2.vercel.app") // Puerto de React: 5173.
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("WithOriginsPolicy");

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

app.Run();