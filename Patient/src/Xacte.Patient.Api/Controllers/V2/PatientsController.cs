using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Xacte.Patient.Api.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public sealed class PatientsController : ControllerBase
    {
        private readonly ILogger<PatientsController> _logger;

        public PatientsController(ILogger<PatientsController> logger)
        {
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
        public async Task<ActionResult> Get()
        {
            return Ok("Ok V2");
        }

        /// <summary>
        /// Gets a demo string with id
        /// </summary>
        /// <returns>Demo string</returns>
        [HttpGet("{id:int}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get([FromRoute] int id)
        {
            return Ok($"Ok V2 id:{id}");
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
        public async Task<ActionResult> GetBillings([FromRoute] int id)
        {
            return Ok($"Ok V2 Billing id:{id}");
        }
    }
}