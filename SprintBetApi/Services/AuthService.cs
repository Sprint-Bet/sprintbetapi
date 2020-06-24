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
			var mySecret = "I am the most absolute unit of all time and i can't believe it it's absolutely crazy i swear";
			var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

			var myIssuer = "https://localhost:5001";
			var myAudience = "http://localhost:8888";

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
