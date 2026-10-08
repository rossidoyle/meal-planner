using MealPlanner.Data;
using MealPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MealPlanner.Services;

public interface ICommunitySeeder
{
	Task<int> SeedIfEmptyAsync(CancellationToken cancellationToken = default);
}

public class CommunitySeeder(
	IDbContextFactory<AppDbContext> contextFactory,
	ILogger<CommunitySeeder> logger) : ICommunitySeeder
{
	public async Task<int> SeedIfEmptyAsync(CancellationToken cancellationToken = default)
	{
		await using var db = await contextFactory.CreateDbContextAsync(cancellationToken);

		if (await db.CommunityRecipes.AnyAsync(cancellationToken))
		{
			return 0;
		}

		var samples = SampleCommunityRecipes.Create();
		db.CommunityRecipes.AddRange(samples);
		await db.SaveChangesAsync(cancellationToken);

		logger.LogInformation("Seeded {Count} community recipes.", samples.Count);

		return samples.Count;
	}
}