using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MealPlanner.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
	public AppDbContext CreateDbContext(string[] args)
	{
		var options = new DbContextOptionsBuilder<AppDbContext>()
			.UseSqlite("Data Source=mealplanner-design.db")
			.Options;

		return new AppDbContext(options);
	}
}