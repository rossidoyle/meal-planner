using MealPlanner.Models;

namespace MealPlanner.Services;

public interface IRecipeService
{
	Task<List<Recipe>> GetAllAsync(string? search = null, bool sharedOnly = false, CancellationToken cancellationToken = default);

	Task<Recipe?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

	Task<int> SaveAsync(Recipe recipe, CancellationToken cancellationToken = default);

	Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}