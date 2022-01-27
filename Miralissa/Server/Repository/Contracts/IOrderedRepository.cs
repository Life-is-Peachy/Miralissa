using Miralissa.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miralissa.Server.Repository
{
	public interface IOrderedRepository
	{
		ICollection<OrderedSubscript> GetOrdered();
		Task MarkAsync(int ID);
		Task DeleteAsync(OrderedSubscript subscript);
		byte[] GetHtmlBytes(int ID);
	}
}