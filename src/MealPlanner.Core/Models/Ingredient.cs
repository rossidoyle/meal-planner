namespace MealPlanner.Models;

public class Ingredient
{
	public int Id { get; set; }

	public int RecipeId { get; set; }

	public Recipe? Recipe { get; set; }

	public int SortOrder { get; set; }

	public string Name { get; set; } = string.Empty;

	public double Quantity { get; set; }

	public string Unit { get; set; } = string.Empty;
}