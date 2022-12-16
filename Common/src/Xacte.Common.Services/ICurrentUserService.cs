using System.Globalization;

namespace Xacte.Common.Services
{
    public interface ICurrentUserService
    {
        Guid Id { get; }
        string Name { get; }
        string FirstName { get; }
        string LastName { get; }
        string Token { get; }
        string Locale { get; }
        CultureInfo Culture { get; }
        string UserRole { get; }
    }
}
