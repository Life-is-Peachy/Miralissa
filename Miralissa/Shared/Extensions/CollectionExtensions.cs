using System.Collections.Generic;

namespace Miralissa.Shared
{
	public static class CollectionExtensions
	{
		public static void AddOrRemoveIfContains<T>(this ICollection<T> @this, T item)
		{
			if (@this.Contains(item))
				@this.Remove(item);
			else
				@this.Add(item);
		}

		public static ICollection<T> AsCollection<T>(this IEnumerable<T> @this)
			=> (ICollection<T>)@this;

		public static IList<T> AsList<T>(this IEnumerable<T> @this)
			=> (IList<T>)@this;
	}
}
