using MealPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace MealPlanner.Data;

public static class RecipeSearch
{
	public static IQueryable<Recipe> Apply(IQueryable<Recipe> query, string? search)
	{
		if (string.IsNullOrWhiteSpace(search))
		{
			return query;
		}

		var term = search.Trim();

		return query.Where(e =>
			EF.Functions.Like(e.Name, $"%{term}%") ||
			(e.Description != null && EF.Functions.Like(e.Description, $"%{term}%")) ||
			(e.Author != null && EF.Functions.Like(e.Author, $"%{term}%")));
	}

	public static IQueryable<CommunityRecipe> Apply(IQueryable<CommunityRecipe> query, string? search)
	{
		if (string.IsNullOrWhiteSpace(search))
		{
			return query;
		}

		var term = search.Trim();

		return query.Where(e =>
			EF.Functions.Like(e.Name, $"%{term}%") ||
			EF.Functions.Like(e.Author, $"%{term}%") ||
			(e.Description != null && EF.Functions.Like(e.Description, $"%{term}%")));
	}
}