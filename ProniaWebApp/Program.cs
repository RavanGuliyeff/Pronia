using Microsoft.EntityFrameworkCore;
using ProniaWebApp.DAL;

namespace ProniaWebApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddControllersWithViews();
			builder.Services.AddDbContext<AppDbContext>(opt =>
			{
				opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
			});
			var app = builder.Build();

			app.MapControllerRoute(
				name: "areas",
				pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
				);

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}"
				);

			app.UseStaticFiles();

			app.Run();
		}
	}
}
