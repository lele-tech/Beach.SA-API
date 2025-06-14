using ApiWebHotelBeachSA.Models.Custom;
using ApiWebHotelBeachSA.Models;
using ApiWebHotelBeachSA.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace ApiWebHotelBeachSA.Services
{
    public class AutorizacionServices : IAutorizacionServices
    {
        private readonly IConfiguration _configuration;

        private readonly DbContextHotelBeachSA _context;

        public AutorizacionServices(IConfiguration configuration,DbContextHotelBeachSA dbContext)
        {
            _configuration = configuration;
            _context = dbContext;
        }


        public async Task<AutorizacionResponse> DevolverToken(Usuario autorizacion)
        {
            var temp = await _context.Usuarios.FirstOrDefaultAsync(u =>
            u.Email.Equals(autorizacion.Email) && u.Password.Equals(autorizacion.Password));

            if (temp == null)
            {
                return await Task.FromResult<AutorizacionResponse>(null);
            }
            else
            {
                string tokenCreado = GenerarToken(autorizacion.Email.ToString());

                return new AutorizacionResponse() { Token = tokenCreado, Resultado = true, Msj = "Ok" };
            }

        }



        private string GenerarToken(string IDUsuario)
        {
            var key = _configuration.GetValue<string>("JwtSettings:key");

            var KeyBytes = Encoding.ASCII.GetBytes(key);

            var claims = new ClaimsIdentity();

            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, IDUsuario));

            var credencialesToken = new SigningCredentials(
                new SymmetricSecurityKey(KeyBytes),
                SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(3),
                SigningCredentials = credencialesToken
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            var tokenCreado = tokenHandler.WriteToken(tokenConfig);

            return tokenCreado;

        }
    }
}
