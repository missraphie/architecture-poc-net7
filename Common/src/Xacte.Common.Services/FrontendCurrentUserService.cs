using Blazored.LocalStorage;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

namespace Xacte.Common.Services
{
    public sealed class FrontendCurrentUserService : ICurrentUserService
    {
        private const string LocalStorageKey = "session";

        private readonly ISyncLocalStorageService _localStorageService;
        private readonly JwtSecurityTokenHandler _jwtTokenHandler;
        private readonly SessionType _jwtToken;

        public FrontendCurrentUserService(ISyncLocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;

            _jwtTokenHandler = new JwtSecurityTokenHandler();
            _jwtToken = GetToken();
        }

        public Guid Id => _jwtToken == null ? Guid.Empty : Guid.Parse(GetClaim(XacteClaimTypes.Guid));
        public string Name => _jwtToken == null ? string.Empty : $"{FirstName} {LastName}";
        public string FirstName => _jwtToken == null ? string.Empty : GetClaim(XacteClaimTypes.GivenName);
        public string LastName => _jwtToken == null ? string.Empty : GetClaim(XacteClaimTypes.UniqueName);
        public string Token => _jwtToken == null ? string.Empty : _jwtToken.Token;
        public string UserRole => _jwtToken == null ? string.Empty : GetClaim(XacteClaimTypes.Role);
        public string Locale => _jwtToken == null ? string.Empty : GetClaim(XacteClaimTypes.Locale);
        public CultureInfo Culture => string.IsNullOrWhiteSpace(Locale) ? CultureInfo.CurrentUICulture : new CultureInfo(Locale);

        private SessionType GetToken()
        {
            return _localStorageService.GetItem<SessionType>(LocalStorageKey);
        }

        private string? GetClaim(string claimType)
        {
            var token = _jwtTokenHandler.ReadJwtToken(_jwtToken.Token)!;
            return token.Claims.FirstOrDefault(f => f.Type == claimType)?.Value;
        }
    }

    internal sealed class SessionType
    {
        public DateTime Expiry { get; set; }
        public string Token { get; set; } = string.Empty;
    }

    internal static class XacteClaimTypes
    {
        public const string Guid = "guid";
        public const string Locale = "locale";
        public const string GivenName = "given_name";
        public const string UniqueName = "unique_name";
        public const string Role = "role";
    }
}
