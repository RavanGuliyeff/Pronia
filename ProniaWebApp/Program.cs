
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

			builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
			{
				opt.User.RequireUniqueEmail = true;

				opt.Password.RequiredLength = 8;
				opt.Password.RequireUppercase = false;
				opt.Password.RequireLowercase = false;
				opt.Password.RequireDigit = false;
				opt.Password.RequireNonAlphanumeric = false;
				
				opt.Lockout.AllowedForNewUsers = true;
				opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
				opt.Lockout.MaxFailedAccessAttempts = 3;


			}).AddEntityFrameworkStores<AppDbContext>()
			.AddDefaultTokenProviders();

			builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
			builder.Services.AddTransient<IMailService, MailService>();

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
