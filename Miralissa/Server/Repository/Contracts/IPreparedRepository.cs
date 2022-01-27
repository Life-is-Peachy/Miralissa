using Miralissa.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miralissa.Server.Repository
{
	public interface IPreparedRepository
	{
		ICollection<PreparedSubscript> GetPrepared();
		Task ToOrderQueueAsync(int[] IDs);
		Task DeletePreparedAsync(PreparedSubscript subscript);
		Task CreateCadastralAsync(PreparedSubscript subscript);
	}
}
