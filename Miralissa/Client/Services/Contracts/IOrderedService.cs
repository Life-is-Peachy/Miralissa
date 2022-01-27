using Miralissa.Shared;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Miralissa.Client.Services
{
	public interface IOrderedService
	{
		Task<ICollection<OrderedSubscript>> GetAsync();
		Task MarkAsync(int ID);
		Task DeleteAsync(OrderedSubscript subscript);
		Task<string> GetHtml(int ID);
	}
}
