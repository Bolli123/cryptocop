using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly ITokenRepository _tokenRepo;

        public JwtTokenService(ITokenRepository tokenRepo)
        {
            _tokenRepo = tokenRepo;
        }

        public bool IsTokenBlacklisted(int tokenId)
        {
            return _tokenRepo.IsTokenBlacklisted(tokenId);
        }
    }
}