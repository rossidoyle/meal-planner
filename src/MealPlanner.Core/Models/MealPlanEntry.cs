namespace MealPlanner.Models;

public enum MealSlot
{
	Breakfast,
	Lunch,
	Dinner
}

public class MealPlanEntry
{
	public int Id { get; set; }

	public DateOnly Date { get; set; }

	public MealSlot Slot { get; set; }

	public int? RecipeId { get; set; }

	public Recipe? Recipe { get; set; }

	public int Servings { get; set; }
}