using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SprintBetApi.Auth;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SprintBetApi.Services
{
    /// <inheritdoc/>
    public class AuthService : IAuthService
    {
		private readonly JwtConfig _jwtConfig;

		public AuthService(IOptions<JwtConfig> jwtConfig)
        {
			_jwtConfig = jwtConfig.Value ?? throw new ArgumentException(nameof(jwtConfig));
        }

        /// <inheritdoc/>
        public string GenerateToken(string voterId, string roomId)
        {
			var mySecret = _jwtConfig.JwtSecret;
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
