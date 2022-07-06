using Meli.Common.Models;
using Meli.ServiceLayer.Contract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Meli.WepApi.Controllers
{
    [ApiController]
    
    public class MutantController : ControllerBase
    {
        private readonly IIndividualService _mutantService;

        public MutantController(IIndividualService mutantService)
        {
            _mutantService = mutantService;
        }

        [Route("[controller]")]
        [HttpPost]        
        public async Task<IActionResult> Post([FromBody]IndividualModel individual)
        {
            try
            {
                var result = await _mutantService.ValidateIndividual(individual);
                return StatusCode(result);
            }
            catch (System.Exception)
            {
                return StatusCode(500);
            }                    
        }

        [Route("stats")]
        [HttpGet]
        public async Task<IActionResult> Stats()
        {
            try
            {
                return Ok(await _mutantService.GetStats());
            }
            catch (System.Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
