using MealPlanner.Data;
using MealPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace MealPlanner.Services;

public class LocalCommunityRecipeService(IDbContextFactory<AppDbContext> contextFactory) : ICommunityRecipeService
{
	public async Task<List<CommunityRecipe>> BrowseAsync(string? search = null, CancellationToken cancellationToken = default)
	{
		await using var db = await contextFactory.CreateDbContextAsync(cancellationToken);

		var query = RecipeSearch.Apply(db.CommunityRecipes.AsNoTracking(), search);

		return await query
			.OrderBy(e => e.Name)
			.ToListAsync(cancellationToken);
	}

	public async Task<HashSet<string>> GetImportedSourceIdsAsync(CancellationToken cancellationToken = default)
	{
		await using var db = await contextFactory.CreateDbContextAsync(cancellationToken);

		var imported = await db.Recipes
			.Where(e => e.SourceId != null)
			.Select(e => e.SourceId!)
			.ToListAsync(cancellationToken);

		return [.. imported];
	}

	public async Task<Recipe> ImportAsync(int communityRecipeId, CancellationToken cancellationToken = default)
	{
		await using var db = await contextFactory.CreateDbContextAsync(cancellationToken);

		var sourceId = CommunityRecipe.ToSourceId(communityRecipeId);

		var alreadyInLibrary = await db.Recipes
			.SingleOrDefaultAsync(e => e.SourceId == sourceId, cancellationToken);

		if (alreadyInLibrary is not null)
		{
			return alreadyInLibrary;
		}

		var source = await db.CommunityRecipes
			.SingleOrDefaultAsync(e => e.Id == communityRecipeId, cancellationToken)
			?? throw new InvalidOperationException($"Community recipe {communityRecipeId} does not exist.");

		var recipe = new Recipe
		{
			Name = source.Name,
			Description = source.Description,
			Servings = source.Servings,
			PrepMinutes = source.PrepMinutes,
			CookMinutes = source.CookMinutes,
			Tags = [.. source.Tags],
			Origin = RecipeOrigin.Community,
			Author = source.Author,
			SourceId = sourceId,
			CreatedAt = DateTimeOffset.Now
		};

		db.Recipes.Add(recipe);
		await db.SaveChangesAsync(cancellationToken);

		return recipe;
	}
}