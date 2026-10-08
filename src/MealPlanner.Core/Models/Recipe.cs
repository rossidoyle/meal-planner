namespace MealPlanner.Models;

public enum RecipeOrigin
{
	MyRecipes,
	Community
}

public class Recipe
{
	public int Id { get; set; }

	public string Name { get; set; } = string.Empty;

	public string? Description { get; set; }

	public int Servings { get; set; } = 2;

	public int PrepMinutes { get; set; }

	public int CookMinutes { get; set; }

	public string? PhotoFileName { get; set; }

	public bool IsShared { get; set; }

	public RecipeOrigin Origin { get; set; } = RecipeOrigin.MyRecipes;

	public string? Author { get; set; }

	public string? SourceId { get; set; }

	public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

	public List<Ingredient> Ingredients { get; set; } = [];

	public List<string> Tags { get; set; } = [];
}