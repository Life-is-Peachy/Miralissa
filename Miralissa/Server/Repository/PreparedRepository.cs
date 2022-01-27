using Miralissa.Server.Classes;
using Miralissa.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Miralissa.Server.Repository
{
	public class PreparedRepository : IPreparedRepository
	{
		private readonly PipelineContext _context;

		public PreparedRepository(PipelineContext ctx) => _context = ctx;

		public ICollection<PreparedSubscript> GetPrepared()
			=> _context.Pipeline.Where(x => x.Result == CONSTS.CDSTRL_CTCHD || x.Result == CONSTS.ADRS_NTFND || x.Result == CONSTS.CDSTRL_NCRCT || x.Result == CONSTS.ANNL)
					.Select(y => new PreparedSubscript
					{
						ID = y.ID,
						ID_Request = y.ID_Request,
						Source = y.Source,
						CadastralNumber = y.CadastralNumber,
						Address = y.Address,
						Square = y.Square,
						Result = y.Result,
						District = y.District,
						City = y.City,
						Town = y.Town,
						Street = y.Street,
						Home = y.Home,
						Corp = y.Corp,
						Flat = y.Flat,
						R_FullAddress = y.R_FullAddress,
						R_ObjType = y.R_ObjType,
						R_Square = y.R_Square,
						R_SteadCategory = y.R_SteadCategory,
						R_SteadKind = y.R_SteadKind,
						R_FuncName = y.R_FuncName,
						R_Status = y.R_Status,
						R_CadastralCost = y.R_CadastralCost,
						R_CadastralCostDate = y.R_CadastralCostDate,
						R_NumStoreys = y.R_NumStoreys,
						R_UpdateInfoDate = y.R_UpdateInfoDate,
						R_LiterBTI = y.R_LiterBTI,
						IsChecked = y.IsChecked,
					}).ToList();

		public async Task DeletePreparedAsync(PreparedSubscript subscript)
		{
			await _context.Pipeline.Where(x => x.ID_Request == subscript.ID_Request).DeleteAsync();

			await _context.Pipeline.SingleInsertAsync(
				new Subscript
				{
					ID_Request = subscript.ID_Request,
					Source = subscript.Source,
					CadastralNumber = subscript.CadastralNumber,
					Address = subscript.Address,
					Result = CONSTS.SBSCRPT_RJCTD,
					IsChecked = true,
					IsResult = true
				});
		}

		public async Task CreateCadastralAsync(PreparedSubscript subscript)
			=> await _context.Pipeline.SingleInsertAsync(
					new Subscript
					{
						ID_Request = subscript.ID_Request,
						Source = subscript.Source,
						AddressRecivedAt = DateTime.Now,
						CadastralNumber = subscript.CadastralNumber,
						Address = subscript.Address,
						Square = subscript.Square,
						Result = CONSTS.CDSTRL_CTCHD,
						IsChecked = true
					});
		public async Task ToOrderQueueAsync(int[] IDs) =>
			await _context.Pipeline.Where(x => IDs.Contains(x.ID))
				.UpdateAsync(t => new Subscript { IsChecked = true });
	}
}
