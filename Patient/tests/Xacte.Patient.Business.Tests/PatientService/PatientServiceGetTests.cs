using AutoMapper;
using Moq;
using System.Net;
using Xacte.Patient.Business.Exceptions;
using Xacte.Patient.Business.Services;
using Xacte.Patient.Business.Tests.MockData;
using Xacte.Patient.Data.Repositories.Interfaces;
using Xacte.Patient.Dto.Api.Patient;
using Xunit;

namespace Xacte.Patient.Business.Tests
{
    public sealed class PatientServiceGetTests
    {
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PatientService _sut;

        public PatientServiceGetTests()
        {
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _mapperMock = new Mock<IMapper>();
            _sut = new PatientService(_mapperMock.Object, _patientRepositoryMock.Object);

            _patientRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Guid>())).ReturnsAsync(true);
        }

        [Fact]
        public async Task ShouldReturnData_WhenPatientIsRetrievedSuccessfully()
        {
            // Arrange
            var requestModel = PatientMock.GetGetPatientRequestModel();
            var patient = PatientMock.GetPatient();
            var patientResponse = PatientMock.GetPatientResponse(x => x.Guid = patient.Guid);

            _patientRepositoryMock.Setup(x => x.GetAsync(patient.Guid)).ReturnsAsync(patient);
            _mapperMock.Setup(x => x.Map<PatientResponse>(patient)).Returns(patientResponse);

            // Act
            var result = await _sut.GetAsync(requestModel);

            // Assert
            Assert.True(result.Meta.IsSuccessStatusCode);
            Assert.Single(result.Data);
            Assert.Equal("Get Successful", result.Meta.Messages.First().Message);

            _patientRepositoryMock.Verify(v => v.AnyAsync(It.Is<Guid>(x => x.Equals(requestModel.Guid))), Times.Once);
            _patientRepositoryMock.Verify(v => v.GetAsync(It.Is<Guid>(x => x.Equals(requestModel.Guid))), Times.Once);
            _patientRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowException_WhenPatientDoesNotExist()
        {
            // Arrange
            var requestModel = PatientMock.GetGetPatientRequestModel();
            _patientRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            // Act
            var result = await Assert.ThrowsAsync<PatientException>(() => _sut.GetAsync(requestModel));

            // Assert
            Assert.Equal(PatientException.Codes.PatientNotFound, Enum.Parse<PatientException.Codes>(result.Code.Code));
            Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
        }

        [Fact]
        public async Task ShouldReturnData_WhenPatientAreRetrievedSuccessfully()
        {
            // Arrange
            var patients = new List<Data.Entities.Patient>
            {
                PatientMock.GetPatient(),
                PatientMock.GetPatient()
            };
            var patientResponse = PatientMock.GetPatientResponse();

            _patientRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(patients);
            _mapperMock.Setup(x => x.Map<PatientResponse>(It.IsAny<Data.Entities.Patient>())).Returns(patientResponse);

            // Act
            var result = await _sut.GetAsync();

            // Assert
            Assert.True(result.Meta.IsSuccessStatusCode);
            Assert.Equal(2, result.Data.Count);
            Assert.Equal("Get Successful", result.Meta.Messages.First().Message);

            _patientRepositoryMock.Verify(v => v.GetAsync(), Times.Once);
            _patientRepositoryMock.VerifyNoOtherCalls();
        }
    }
}