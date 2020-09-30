using System;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using SprintBetApi.Dtos;

namespace SprintBetApi.Services
{
    /// <inheritdoc/>
    public class AuthService : IAuthService
    {
        /// <inheritdoc/>
        public string GenerateToken(AuthenticateVoterDto authenticateVoterDto)
        {
			var mySecret = "placeholder secret";
			var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

			var myIssuer = "https://sprintbetapi.herokuapp.com/";
			var myAudience = "https://sprintbet.herokuapp.com/";

			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.NameIdentifier, authenticateVoterDto.Id),
				}),
				Expires = DateTime.UtcNow.AddHours(4),
				Issuer = myIssuer,
				Audience = myAudience,
				SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
    }
}
