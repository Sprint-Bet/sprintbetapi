using SprintBetApi.Dtos;

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
    }
}
