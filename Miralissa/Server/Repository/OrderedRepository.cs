using Miralissa.Server.Classes;
using Miralissa.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Miralissa.Server.Repository
{
	public class OrderedRepository : IOrderedRepository
	{
		public OrderedRepository(PipelineContext ctx) => _context = ctx;

		private readonly PipelineContext _context;

		public ICollection<OrderedSubscript> GetOrdered()
			=> _context.Pipeline.Where(x => (x.Result == CONSTS.HAS_OWNER_DATA || x.Result == CONSTS.NO_OWNER_DATA || x.Result == CONSTS.DIZO || x.Result == CONSTS.UL || x.Result == "Доли не обработаны") && x.HasXml && !x.IsResult)
				.Select(t => new OrderedSubscript
				{
					ID = t.ID,
					ID_Request = t.ID_Request,
					Source = t.Source,
					Address = t.Address,
					CadastralNumber = t.CadastralNumber,
					Result = t.Result,
					IsResult = t.IsResult
				}).ToList();

		public async Task DeleteAsync(OrderedSubscript subscript)
		{
			await _context.Pipeline.Where(x => x.ID_Request == subscript.ID_Request).DeleteAsync();

			await _context.Pipeline.SingleInsertAsync(
				new Subscript
				{
					ID_Request = subscript.ID_Request,
					Source = subscript.Source,
					AddressRecivedAt = DateTime.Now,
					CadastralNumber = subscript.CadastralNumber,
					Address = subscript.Address,
					Result = CONSTS.SBSCRPT_RJCTD,
					IsChecked = true,
					IsResult = true
				});
		}

		public async Task MarkAsync(int ID)
			=> await _context.Pipeline.Where(x => x.ID == ID)
				.UpdateAsync(_ => new Subscript { IsResult = true });

		// Плохо
		public byte[] GetHtmlBytes(int ID)
		{
			return _context.Xmls.Where(x => x.IdPipeline == ID).Select(t => PipelineContext.Decompress(t.HtmlData)).First();
		}
	}
}