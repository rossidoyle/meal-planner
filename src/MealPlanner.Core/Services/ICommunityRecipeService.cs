using MealPlanner.Models;

namespace MealPlanner.Services;

public interface ICommunityRecipeService
{
	Task<List<CommunityRecipe>> BrowseAsync(string? search = null, CancellationToken cancellationToken = default);

	Task<HashSet<string>> GetImportedSourceIdsAsync(CancellationToken cancellationToken = default);

	Task<Recipe> ImportAsync(int communityRecipeId, CancellationToken cancellationToken = default);
}