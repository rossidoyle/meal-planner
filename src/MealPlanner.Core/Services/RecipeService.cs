using MealPlanner.Data;
using MealPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace MealPlanner.Services;

public class RecipeService(IDbContextFactory<AppDbContext> contextFactory) : IRecipeService
{
	public async Task<List<Recipe>> GetAllAsync(
		string? search = null,
		bool sharedOnly = false,
		CancellationToken cancellationToken = default)
	{
		await using var db = await contextFactory.CreateDbContextAsync(cancellationToken);

		var query = db.Recipes
			.Include(e => e.Ingredients)
			.AsNoTracking();

		if (sharedOnly)
		{
			query = query.Where(e => e.IsShared);
		}

		query = RecipeSearch.Apply(query, search);

		return await query
			.OrderBy(e => e.Name)
			.ToListAsync(cancellationToken);
	}

	public async Task<Recipe?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
	{
		await using var db = await contextFactory.CreateDbContextAsync(cancellationToken);

		return await db.Recipes
			.AsNoTracking()
			.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
	}

	public async Task<int> SaveAsync(Recipe recipe, CancellationToken cancellationToken = default)
	{
		recipe.Name = recipe.Name.Trim();
		recipe.Tags = NormalizeTags(recipe.Tags);

		await using var db = await contextFactory.CreateDbContextAsync(cancellationToken);

		if (recipe.Id == 0)
		{
			recipe.CreatedAt = DateTimeOffset.Now;
			db.Recipes.Add(recipe);
		}
		else
		{
			db.Recipes.Update(recipe);
		}

		await db.SaveChangesAsync(cancellationToken);

		return recipe.Id;
	}

	public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
	{
		await using var db = await contextFactory.CreateDbContextAsync(cancellationToken);

		var recipe = await db.Recipes.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);

		if (recipe is not null)
		{
			db.Recipes.Remove(recipe);
			await db.SaveChangesAsync(cancellationToken);
		}
	}

	static List<string> NormalizeTags(IEnumerable<string> tags) => tags
		.Where(t => !string.IsNullOrWhiteSpace(t))
		.Select(t => t.Trim())
		.Distinct(StringComparer.OrdinalIgnoreCase)
		.OrderBy(t => t)
		.ToList();
}