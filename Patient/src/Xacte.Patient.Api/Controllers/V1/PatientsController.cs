using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Xacte.Common.Responses;
using Xacte.Patient.Api.Filters;
using Xacte.Patient.Business.Services.Interfaces;
using Xacte.Patient.Dto.Api.Patient;
using Xacte.Patient.Dto.Business;

namespace Xacte.Patient.Api.Controllers.V1
{
    /// <summary>
    /// Patients controller API
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [PatientExceptionFilter]
    public sealed class PatientsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPatientService _patientService;
        private readonly ILogger<PatientsController> _logger;

        /// <summary>
        /// Patients controller class.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="patientService"></param>
        /// <param name="logger"></param>
        public PatientsController(IMapper mapper, IPatientService patientService, ILogger<PatientsController> logger)
        {
            _mapper = mapper;
            _patientService = patientService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a patient
        /// </summary>
        /// <returns>Created patient</returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CreatePatientResponse>> Create([FromBody] CreatePatientRequest request)
        {
            var result = await _patientService.CreateAsync(_mapper.Map<CreatePatientRequestModel>(request));
            return Created($"/patients/{result.Data.First().Guid}", result);
        }

        /// <summary>
        /// Deletes a patient
        /// </summary>
        /// <returns>Nothing</returns>
        [HttpDelete("{guid:guid}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<GetPatientResponse>> Delete([FromRoute] Guid guid)
        {
            var request = new DeletePatientRequest(guid);
            return Ok(await _patientService.DeleteAsync(_mapper.Map<DeletePatientRequestModel>(request)));
        }

        /// <summary>
        /// Gets a demo string
        /// </summary>
        /// <returns>Demo string</returns>
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetPatientsResponse>> Get()
        {
            return Ok(await _patientService.GetAsync());
        }

        /// <summary>
        /// Gets a demo string with id
        /// </summary>
        /// <returns>Demo string</returns>
        [HttpGet("{guid:guid}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(XacteErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetPatientResponse>> Get([FromRoute] Guid guid)
        {
            var request = new GetPatientRequest(guid);
            return Ok(await _patientService.GetAsync(_mapper.Map<GetPatientRequestModel>(request)));
        }



        /// <summary>
        /// Gets a demo string with id
        /// </summary>
        /// <returns>Demo string</returns>
        [HttpGet("{id:int}/billings")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetBillings([FromRoute] int id)
        {
            return Ok($"Ok V1 Billing id:{id}");
        }
    }
}