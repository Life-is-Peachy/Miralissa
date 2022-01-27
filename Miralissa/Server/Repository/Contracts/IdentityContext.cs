using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Miralissa.Server.Models;

namespace Miralissa.Server.Repository
{
	public class IdentityContext : IdentityDbContext<User, Role, int>
	{
		public IdentityContext(DbContextOptions<IdentityContext> options)
			: base(options) { }


		protected override void OnModelCreating(ModelBuilder builder)
		=> base.OnModelCreating(builder);
	}
}
