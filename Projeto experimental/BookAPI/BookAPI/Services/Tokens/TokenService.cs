using BookAPI.Entities.Clientes;
using BookAPI.Services.Tokens;
using BookAPI.Token;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookAPI.Services.Token
{
    public static class TokenService
    {
        public static TokenResponse GenerateToken(Cliente cliente)
        {
            var key = Encoding.ASCII.GetBytes(Key.Secret);

            var tokenConfig = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Id", cliente.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfig);
            var tokenString = tokenHandler.WriteToken(token);

            return new TokenResponse
            {
                Token = tokenString  
            };
        }

        public static async Task<int?> GetClientIdFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jsonToken == null)
                    throw new Exception("Token inválido.");

                var claims = jsonToken?.Claims;
                var id = claims?.FirstOrDefault(claim => claim.Type == "Id")?.Value;

                if (id != null)
                {
                    return int.Parse(id);
                }
                else
                {
                    throw new Exception("ID não encontrado no token.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter informações do token: {ex.Message}");
            }

            return null;
        }
    }
}
