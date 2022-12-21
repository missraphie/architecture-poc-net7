using AutoMapper;
using Moq;
using Xacte.Patient.Business.Services;
using Xacte.Patient.Business.Tests.MockData;
using Xacte.Patient.Data.Repositories.Interfaces;
using Xacte.Patient.Dto.Api.Patient;
using Xunit;

namespace Xacte.Patient.Business.Tests
{
    public sealed class PatientServiceCreateTests
    {
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PatientService _sut;

        public PatientServiceCreateTests()
        {
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _mapperMock = new Mock<IMapper>();
            _sut = new PatientService(_mapperMock.Object, _patientRepositoryMock.Object);
        }

        [Fact]
        public async Task ShouldReturnData_WhenPatientIsCreatedSuccessfully()
        {
            // Arrange
            var requestModel = PatientMock.GetCreatePatientRequestModel();
            var patient = PatientMock.GetPatient(x =>
            {
                x.FirstName = requestModel.FirstName;
                x.LastName = requestModel.LastName;
            });
            var patientResponse = PatientMock.GetPatientResponse(x => x.Guid = patient.Guid);

            _patientRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Data.Entities.Patient>())).ReturnsAsync(patient);
            _mapperMock.Setup(x => x.Map<PatientResponse>(patient)).Returns(patientResponse);

            // Act
            var result = await _sut.CreateAsync(requestModel);

            // Assert
            Assert.True(result.Meta.IsSuccessStatusCode);
            Assert.NotEmpty(result.Data);
            Assert.Single(result.Data);
            Assert.Equal("Patient created Successful", result.Meta.Messages.First().Message);

            _patientRepositoryMock.Verify(v => v.CreateAsync(
                It.Is<Data.Entities.Patient>(p =>
                    p.FirstName.Equals(requestModel.FirstName)
                    && p.LastName.Equals(requestModel.LastName))), Times.Once);
            _patientRepositoryMock.VerifyNoOtherCalls();
        }
    }
}