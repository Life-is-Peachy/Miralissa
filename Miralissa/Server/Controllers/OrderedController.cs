using Microsoft.AspNetCore.Mvc;
using Miralissa.Server.Repository;
using Miralissa.Shared;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Miralissa.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderedController : ControllerBase
	{
		private readonly IOrderedRepository _repository;

		public OrderedController(IOrderedRepository repo)
		{
			_repository = repo;
		}

		[HttpGet("[action]")]
		public ICollection<OrderedSubscript> Get()
			=> _repository.GetOrdered();

		[HttpPost("[action]")]
		public async Task<IActionResult> MarkAsync([FromBody]int ID)
		{
			await _repository.MarkAsync(ID);
			return Ok();
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> DeleteAsync(OrderedSubscript subscript)
		{
			await _repository.DeleteAsync(subscript);
			return Ok();
		}

		[HttpGet("[action]/{id:int}")]
		public string GetHtml(int id)
		{
			var res = _repository.GetHtmlBytes(id);
			return Encoding.UTF8.GetString(res);
		}
	}
}
