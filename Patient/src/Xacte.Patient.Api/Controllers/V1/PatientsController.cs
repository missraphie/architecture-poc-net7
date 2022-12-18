using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
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
    public sealed class PatientsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPatientService _patientService;
        private readonly ILogger<PatientsController> _logger;

        /// <summary>
        /// Patients controller class.
        /// </summary>
        /// <param name="logger"></param>
        public PatientsController(IMapper mapper, IPatientService patientService, ILogger<PatientsController> logger)
        {
            _mapper = mapper;
            _patientService = patientService;
            _logger = logger;
        }

        /// <summary>
        /// Gets a demo string
        /// </summary>
        /// <returns>Demo string</returns>
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Get()
        {
            return Ok("Ok V1");
        }

        /// <summary>
        /// Gets a demo string with id
        /// </summary>
        /// <returns>Demo string</returns>
        [HttpGet("{guid:guid}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetPatientResponse>> Get([FromRoute] Guid guid)
        {
            var request = new GetPatientRequest(guid);
            return Ok(await _patientService.GetPatientAsync(_mapper.Map<GetPatientRequestModel>(request)));
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