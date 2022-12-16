using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Security.Claims;

namespace Xacte.Common.Services
{

    public sealed class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private const string TOKEN_HEADER = "Authorization";

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid Id
        {
            get
            {
                if (_httpContextAccessor.HttpContext?.User.Identity!.IsAuthenticated ?? false)
                {
                    ClaimsPrincipal user = _httpContextAccessor.HttpContext.User;
                    return Guid.Parse(user.FindFirst("guid")!.Value);
                }
                return Guid.Empty;
            }
        }

        public string Name
        {
            get
            {
                if (_httpContextAccessor.HttpContext?.User.Identity!.IsAuthenticated ?? false)
                {
                    ClaimsPrincipal user = _httpContextAccessor.HttpContext.User;
                    return $"{user.FindFirst(ClaimTypes.GivenName)!.Value} {user.FindFirst(ClaimTypes.Name)!.Value}";
                }
                return string.Empty;
            }
        }

        public string Token
        {
            get
            {
                if (_httpContextAccessor.HttpContext?.Request.Headers.ContainsKey(TOKEN_HEADER) ?? false)
                {
                    return _httpContextAccessor.HttpContext.Request.Headers[TOKEN_HEADER].ToString().Replace("Bearer ", "");
                }
                return string.Empty;
            }
        }

        public string FirstName
        {
            get
            {
                if (_httpContextAccessor.HttpContext?.User.Identity!.IsAuthenticated ?? false)
                {
                    ClaimsPrincipal user = _httpContextAccessor.HttpContext.User;
                    return user.FindFirst(ClaimTypes.GivenName)!.Value;
                }
                return string.Empty;
            }
        }

        public string LastName
        {
            get
            {
                if (_httpContextAccessor.HttpContext?.User.Identity!.IsAuthenticated ?? false)
                {
                    ClaimsPrincipal user = _httpContextAccessor.HttpContext.User;
                    return user.FindFirst(ClaimTypes.Name)!.Value;
                }
                return string.Empty;
            }
        }

        public string Locale
        {
            get
            {
                if (_httpContextAccessor.HttpContext?.User.Identity!.IsAuthenticated ?? false)
                {
                    return _httpContextAccessor.HttpContext.User.FindFirst("locale")!.Value;
                }
                return string.Empty;
            }
        }

        public CultureInfo Culture => string.IsNullOrWhiteSpace(Locale) ? new CultureInfo(string.Empty) : new CultureInfo(Locale);

        public string UserRole
        {
            get
            {
                if (_httpContextAccessor.HttpContext?.User.Identity!.IsAuthenticated ?? false)
                {
                    return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)!.Value;
                }
                return string.Empty;
            }
        }
    }
}
