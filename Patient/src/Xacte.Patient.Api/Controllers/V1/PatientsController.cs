using Microsoft.AspNetCore.Mvc;

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
        private readonly ILogger<PatientsController> _logger;

        /// <summary>
        /// Patients controller class.
        /// </summary>
        /// <param name="logger"></param>
        public PatientsController(ILogger<PatientsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets a demo string
        /// </summary>
        /// <returns>Demo string</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get()
        {
            return Ok("Ok V1");
        }

        /// <summary>
        /// Gets a demo string with id
        /// </summary>
        /// <returns>Demo string</returns>
        [HttpGet("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok($"Ok V1 {id}");
        }

        /// <summary>
        /// Gets a demo string with id
        /// </summary>
        /// <returns>Demo string</returns>
        [HttpGet("{id:int}/billings")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetBillings([FromRoute] int id)
        {
            return Ok($"Ok V1 Billing id:{id}");
        }
    }
}