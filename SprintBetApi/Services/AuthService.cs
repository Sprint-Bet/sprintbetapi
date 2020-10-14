using Microsoft.IdentityModel.Tokens;
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
					new Claim(Constants.Constants.VoterIdClaimType, voterId),
					new Claim(Constants.Constants.RoomIdClaimType, roomId)
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

		/// <inheritdoc/>
		public JwtSecurityToken ReadToken(string authHeader)
        {
			var token = authHeader.Replace("Bearer ", "");
			var tokenHandler = new JwtSecurityTokenHandler();
			return tokenHandler.ReadJwtToken(token);
		}
    }
}
