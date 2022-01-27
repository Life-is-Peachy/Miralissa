using Microsoft.AspNetCore.Mvc;
using Miralissa.Server.Repository;
using Miralissa.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miralissa.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PreparedController : ControllerBase
	{
		private readonly IPreparedRepository _repository;

		public PreparedController(IPreparedRepository repository) => _repository = repository;


		[HttpGet("[action]")]
		public ICollection<PreparedSubscript> Get()
			=> _repository.GetPrepared();


		[HttpPost("[action]")]
		public async Task<IActionResult> MarkAsOrderable(int[] subscripts)
		{
			await _repository.ToOrderQueueAsync(subscripts);
			return Ok();
		}


		[HttpPost("[action]")]
		public async Task<IActionResult> DeletePrepared(PreparedSubscript subscript)
		{
			await _repository.DeletePreparedAsync(subscript);
			return Ok();
		}


		[HttpPost("[action]")]
		public async Task<IActionResult> CreateCadastralAsync(PreparedSubscript subscript)
		{
			await _repository.CreateCadastralAsync(subscript);
			return Ok();
		}
	}
}
