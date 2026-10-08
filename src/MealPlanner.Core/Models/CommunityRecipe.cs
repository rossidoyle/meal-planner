namespace MealPlanner.Models;

public class CommunityRecipe
{
	public int Id { get; set; }

	public string Name { get; set; } = string.Empty;

	public string? Description { get; set; }

	public string Author { get; set; } = string.Empty;

	public int Servings { get; set; } = 2;

	public int PrepMinutes { get; set; }

	public int CookMinutes { get; set; }

	public List<string> Tags { get; set; } = [];

	public List<string> IngredientLines { get; set; } = [];

	public static string ToSourceId(int communityRecipeId) => $"community:{communityRecipeId}";
}