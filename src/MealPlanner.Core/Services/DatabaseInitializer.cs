using MealPlanner.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MealPlanner.Services;

public interface IDatabaseInitializer
{
	Task InitializeAsync(CancellationToken cancellationToken = default);
}

public class DatabaseInitializer(
	IDbContextFactory<AppDbContext> contextFactory,
	ICommunitySeeder communitySeeder,
	ILogger<DatabaseInitializer> logger) : IDatabaseInitializer
{
	public async Task InitializeAsync(CancellationToken cancellationToken = default)
	{
		await using var db = await contextFactory.CreateDbContextAsync(cancellationToken);

		var pending = (await db.Database.GetPendingMigrationsAsync(cancellationToken)).ToList();

		if (pending.Count > 0)
		{
			logger.LogInformation("Applying migrations: {Migrations}", string.Join(", ", pending));
		}

		await db.Database.MigrateAsync(cancellationToken);

		var seeded = await communitySeeder.SeedIfEmptyAsync(cancellationToken);

		if (seeded > 0)
		{
			logger.LogInformation("Seeded community recipes: {Count}", seeded);
		}
	}
}