using AutoMapper;
using Moq;
using Xacte.Patient.Business.Services;
using Xacte.Patient.Business.Tests.MockData;
using Xacte.Patient.Data.Repositories.Interfaces;
using Xunit;

namespace Xacte.Patient.Business.Tests
{
    public sealed class PatientServiceDeleteTests
    {
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PatientService _sut;

        public PatientServiceDeleteTests()
        {
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _mapperMock = new Mock<IMapper>();
            _sut = new PatientService(_mapperMock.Object, _patientRepositoryMock.Object);

            _patientRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Guid>())).ReturnsAsync(true);
        }

        [Fact]
        public async Task ShouldReturnDataEmpty_WhenPatientIsDeletedSuccessfully()
        {
            // Arrange
            var requestModel = PatientMock.GetDeletePatientRequestModel();

            // Act
            var result = await _sut.DeleteAsync(requestModel);

            // Assert
            Assert.True(result.Meta.IsSuccessStatusCode);
            Assert.Empty(result.Data);
            Assert.Equal("Patient deleted successfully", result.Meta.Messages.First().Message);

            _patientRepositoryMock.Verify(v => v.AnyAsync(It.Is<Guid>(x => x.Equals(requestModel.Guid))), Times.Once);
            _patientRepositoryMock.Verify(v => v.DeleteAsync(It.Is<Guid>(x => x.Equals(requestModel.Guid))), Times.Once);
            _patientRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnError_WhenPatientDoesNotExist()
        {
            // Arrange
            var requestModel = PatientMock.GetDeletePatientRequestModel();
            _patientRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            // Act
            var result = await _sut.DeleteAsync(requestModel);

            // Assert
            Assert.False(result.Meta.IsSuccessStatusCode);
            Assert.Empty(result.Data);
            Assert.Equal("Patient does not exist", result.Meta.Messages.First().Message);
        }
    }
}