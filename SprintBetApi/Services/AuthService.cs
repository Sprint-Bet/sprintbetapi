using Microsoft.IdentityModel.Tokens;
using SprintBetApi.Constants;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SprintBetApi.Services
{
    /// <inheritdoc/>
    public class AuthService : IAuthService
    {
        /// <inheritdoc/>
        public string GenerateToken(string voterId, string roomId)
        {
			var mySecret = "placeholder secret";
			var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.NameIdentifier, voterId),
					new Claim(CustomClaims.RoomId, roomId)
				}),
				Expires = DateTime.UtcNow.AddHours(4),
				Issuer = "https://sprintbetapi.herokuapp.com/",
				Audience = "https://sprintbet.herokuapp.com/",
				SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
    }
}
