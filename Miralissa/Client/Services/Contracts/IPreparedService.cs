using Miralissa.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miralissa.Client.Services
{
	public interface IPreparedService
	{
		Task<ICollection<PreparedSubscript>> GetAsync();
		Task MarkAsync(int[] IDs);
		Task DeleteAsync(PreparedSubscript subscript);
		Task CreateAsync(PreparedSubscript subscript);
	}
}
