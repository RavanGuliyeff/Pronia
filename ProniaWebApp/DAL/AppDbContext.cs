using Microsoft.EntityFrameworkCore;

namespace ProniaWebApp.DAL
{
	public class AppDbContext:DbContext
	{
        public AppDbContext(DbContextOptions options):base(options)
        {
        }
    }
}
