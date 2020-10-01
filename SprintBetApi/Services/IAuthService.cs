using SprintBetApi.Dtos;
using System.IdentityModel.Tokens.Jwt;

namespace SprintBetApi.Services
{
    // <summary>
    ///     Interface for AuthService that performs actions related to Auth
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        ///     Generate jwt token
        /// </summary>
        /// <param name="voterId"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public string GenerateToken(string voterId, string roomId);

        /// <summary>
        ///     Read jwt token
        /// </summary>
        /// <param name="authHeader"></param>
        /// <returns></returns>
        public JwtSecurityToken ReadToken(string authHeader);
    }
}
