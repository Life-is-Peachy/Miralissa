using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Miralissa.Server.Classes;
using Miralissa.Shared;

namespace Miralissa.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PersonalDataController : ControllerBase
	{
		[HttpGet("[action]/{fio}&{address}")]
		public IEnumerable<PDelement> Get(string fio, string address)
		{
			//	¯\_(ツ)_/¯
			if (fio == "привет") fio = null;
			if (address == "я костыль") address = null;
			if (fio == null && address == null) return Enumerable.Empty<PDelement>();

			return PDhandler.GetFulltextSearchResult(new PDmodel(fio, address))
				.OrderByDescending(x => x.Rank)
				.Take(10);
		}
	}
}
