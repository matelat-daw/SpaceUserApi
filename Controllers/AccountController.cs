using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SpaceUserAPI.Interface;
using SpaceUserAPI.Models.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;

namespace SpaceUserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IEmailSender emailSender, SignInManager<SpaceUser> signInManager, UserManager<SpaceUser> userManager, IConfiguration configuration) : ControllerBase
    {
        private readonly string GoogleClientId = Environment.GetEnvironmentVariable("Google-Client-Id")!; // Reemplaza con tu Client ID.
        private readonly string MicrosoftClientId = Environment.GetEnvironmentVariable("Microsoft-Client-Id")!; // Reemplaza con tu Client ID.
        private readonly string MicrosoftClientSecret = Environment.GetEnvironmentVariable("Microsoft-Client-Secret")!; // Reemplaza con tu Client Secret.

        //[HttpGet("GoogleLogin/{token}")]
        //public async Task<IActionResult> GoogleLogin(string token)
        [HttpPost("GoogleLogin")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            var token = request.Token;

            try
            {
                // Verificar el token de Google
                var payload = await GoogleJsonWebSignature.ValidateAsync(token, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = [GoogleClientId] // Validar contra tu Client ID
                });

                var user = await userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new SpaceUser
                    {
                        UserName = payload.Email,
                        Email = payload.Email,
                        Name = payload.Name,
                        EmailConfirmed = true,
                        ProfileImage = payload.Picture
                    };
                    await userManager.CreateAsync(user);
                    await userManager.AddToRoleAsync(user, "Basic");
                }

                var localToken = await GenerateToken(user);

                // Aquí puedes usar los datos del usuario
                //var email = payload.Email;
                //var name = payload.Name;
                //var picture = payload.Picture;

                // Lógica adicional: Crear o autenticar al usuario en tu sistema
                return Ok(new
                {
                    Message = "Inicio de Sesión Exitoso",
                    Token = new JwtSecurityTokenHandler().WriteToken(localToken),
                    Email = user.Email,
                    Name = user.Name,
                    Picture = user.ProfileImage
                });
            }
            catch (InvalidJwtException ex)
            {
                // Token inválido
                return BadRequest(new { Message = "Token Inválido", Error = ex.Message });
            }
        }

        [HttpGet("MicrosoftLogin/{token}")]
        public async Task<IActionResult> MicrosoftLogin(string token)
        {
            try
            {
                // Configurar los parámetros de validación
                var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                    "https://login.microsoftonline.com/common/v2.0/.well-known/openid-configuration",
                    new OpenIdConnectConfigurationRetriever());

                var config = await configManager.GetConfigurationAsync();
                var tokenHandler = new JwtSecurityTokenHandler();

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuers = ["https://login.microsoftonline.com/" + MicrosoftClientSecret + "/v2.0"],
                    ValidateAudience = true,
                    ValidAudiences = [MicrosoftClientId],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKeys = config.SigningKeys,
                    ValidateLifetime = true
                };

                // Validar el token
                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                // Obtener información del usuario
                string? userEmail = claimsPrincipal.FindFirst("preferred_username")?.Value;
                string? name = claimsPrincipal.FindFirst("name")?.Value;

                return Ok(new
                {
                    message = "Login Exitoso",
                    email = userEmail,
                    name,
                    picture = claimsPrincipal.FindFirst("picture")?.Value
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Token Inválido", Error = ex.Message });
            }
        }

        [HttpGet("Users")]
        public async Task<IActionResult> Users()
        {
            return Ok(await userManager.Users.ToListAsync());
        }

        [HttpGet("GetUser/{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("ERROR: Ese Usuario no Existe.");
            }
            return Ok(user);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(Login model)
        {
            SpaceUser? user = await userManager.FindByEmailAsync(model.Email!);

            if (user == null)
            {
                return NotFound("ERROR: No Existe un Usuario Registrado con ese E-mail.");
            }

            if (!user.EmailConfirmed)
            {
                return BadRequest("ERROR: El E-mail no Está Confirmado, Por Favor Confirma tu Registro.");
            }
            
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password!))
            {
                //IList<string> roles = await userManager.GetRolesAsync(user);
                //List<Claim> claims =
                //[
                //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString() ),
                //    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                //    new Claim(JwtRegisteredClaimNames.Sub, user.UserName!)
                //];

                //foreach (var role in roles)
                //    claims.Add(new Claim(ClaimTypes.Role, role));

                //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!));
                //var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                //var token = new JwtSecurityToken
                //    (
                //        configuration["JWT:Issuer"],
                //        configuration["JWT:Audience"],
                //        claims,
                //        expires: DateTime.UtcNow.AddMinutes(120),
                //        signingCredentials: credentials
                //    );
                //return Ok(new JwtSecurityTokenHandler().WriteToken(token));

                var token = await GenerateToken(user);
                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }

            return Unauthorized();
        }

        private async Task<JwtSecurityToken> GenerateToken(SpaceUser user)
        {
            IList<string> roles = await userManager.GetRolesAsync(user);
            if (roles.Count == 0)
            {
                await userManager.AddToRoleAsync(user, "Basic");
                roles = await userManager.GetRolesAsync(user);
            }
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            return new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPassword model)
        {
            var user = await userManager.FindByEmailAsync(model.Email!);
            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var resetLink = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);
                await emailSender.SendEmailAsync(model.Email!, "Reestablecer Contraseña", $"Por Favor Reestablece tu Contraseña Haciendo Click en Este Enlace: <a href='{resetLink}'>Reestablecer Contraseña</a>");
                return Ok("Por Favor Revisa tu E-mail Para Cambiar la Contraseña.");
            }

            return NotFound("ERROR: No Existe un Usuario Registrado con ese E-mail.");
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            var user = await userManager.FindByEmailAsync(model.Email!);
            if (user == null)
            {
                return NotFound("ERROR: No Existe un Usuario Registrado con ese E-mail.");
            }

            var result = await userManager.ResetPasswordAsync(user, model.Token!, model.Password!);
            if (result.Succeeded)
            {
                return Ok("Contraseña Cambiada Correctamente.");
            }

            return BadRequest("ERROR: La Contraseña Tiene que Tener al Menos una Letra Mayúscula, una Minúscula, un Dígito, un Caracer Especial y 8 Caracteres de Longitud.");
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] Register model)
        {
            SpaceUser? existingUser = await userManager.FindByEmailAsync(model.Email!);
            if (existingUser != null)
            {
                return BadRequest("ERROR: Ya Existe un Usuario Registrado con ese E-mail.");
            }

            var profileImagePath = await SaveProfileImageAsync(model.ProfileImageFile, model.Email!);

            if (model.Surname2 == "")
            {
                model.Surname2 = null;
            }

            var user = new SpaceUser
            {
                Name = model.Name,
                Surname1 = model.Surname1,
                Surname2 = model.Surname2,
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                ProfileImage = profileImagePath,
                Bday = model.Bday
            };

            try
            {
                IdentityResult result = await userManager.CreateAsync(user, model.Password!);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Basic");
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token }, Request.Scheme);

                    await emailSender.SendEmailAsync(user.Email!, "Confirma tu Registro", $"Por Favor Confirma tu Cuenta Haciendo Click en Este Enlace: <a href='{confirmationLink}'>Confirmar Registro</a>");

                    return Ok("Confirma tu Registro.");
                }
            }
            catch (Exception)
            {
                return BadRequest("ERROR: La Contraseña Tiene que Tener al Menos una Letra Mayúscula, una Minúscula, un Dígito, un Caracer Especial y 8 Caracteres de Longitud.");
            }

            return BadRequest("ERROR: La Contraseña Tiene que Tener al Menos una Letra Mayúscula, una Minúscula, un Dígito, un Caracer Especial y 8 Caracteres de Longitud.");
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null || userId == "" || token == "")
            {
                return BadRequest("ERROR: La Id y el Token no Pueden Estar Vacios.");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("ERROR: No Existe un Usuario con esa ID.");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                // return Ok("Registro Confirmado.");
                return Redirect("https://nexus-astralis-2.vercel.app");
            }

            return BadRequest("ERROR: El E-mail de Confirmación no Está Registrado, ¿Estás Seguro que no Eliminaste tu Cuenta?.");
        }

        [HttpPatch("Update/{id}")]
        public async Task<IActionResult> Update(string id, [FromForm] Register model)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("ERROR: Ese Usuario no Existe.");
            }

            if (model.Surname2 == "")
            {
                model.Surname2 = null;
            }

            user.Name = model.Name;
            user.Surname1 = model.Surname1;
            user.Surname2 = model.Surname2;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Bday = model.Bday;

            if (model.ProfileImageFile != null)
            {
                user.ProfileImage = await SaveProfileImageAsync(model.ProfileImageFile, model.Email!);
            }

            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.Password))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordChangeResult = await userManager.ResetPasswordAsync(user, token, model.Password);
                    if (!passwordChangeResult.Succeeded)
                    {
                        foreach (var error in passwordChangeResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return BadRequest("ERROR: La Contraseña Tiene que Tener al Menos una Letra Mayúscula, una Minúscula, un Dígito, un Caracer Especial y 8 Caracteres de Longitud.");
                    }
                }

                await Logout();

                return Ok("Datos Actualizados.");
            }

            return BadRequest("ERROR: La Contraseña Tiene que Tener al Menos una Letra Mayúscula, una Minúscula, un Dígito, un Caracer Especial y 8 Caracteres de Longitud.");
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return Ok("Loged Out.");
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("ERROR: Ese Usuario no Existe.");
            }

            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                await Logout();

                return Ok("Usuario Eliminado.");
            }

            return BadRequest("ERROR: No se Pudo Eliminar el Usuario.");
        }

        private static async Task<string> SaveProfileImageAsync(IFormFile? profileImageFile, string email)
        {
            if (profileImageFile is null)
            {
                return "/imgs/default-profile.jpg";
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imgs/profile/" + email);
            Directory.CreateDirectory(uploadsFolder);
            var uniqueFileName = profileImageFile.FileName;
            var filename = uniqueFileName.Split('.');
            var index = 0;
            while (index < filename.Length - 1) // Para estar seguros de que el nombre de la imagen no tenga más "." (puntos) antes de la extensión.
            {
                index++;
            }
            var lastName = "Profile." + filename[index];
            var filePath = Path.Combine(uploadsFolder, lastName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await profileImageFile.CopyToAsync(fileStream);
            }

            return "/imgs/profile/" + email + "/" + lastName;
        }
    }
}