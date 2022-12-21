using Moq;
using Xacte.Common.Services;

namespace Xacte.Patient.Business.Tests.MockData
{
    public static class CurrentUserServiceMock
    {
        public static ICurrentUserService GetCurrentUserService()
        {
            Mock<ICurrentUserService> mock = new();
            mock.Setup(s => s.Id).Returns(Guid.Parse("aaaaaaaa-0000-0000-0000-000000000000"));
            mock.Setup(s => s.Name).Returns("Xacte User");

            return mock.Object;
        }
    }
}
