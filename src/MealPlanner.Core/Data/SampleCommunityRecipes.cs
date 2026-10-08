using MealPlanner.Models;

namespace MealPlanner.Data;

public static class SampleCommunityRecipes
{
	public static IReadOnlyList<CommunityRecipe> Create() =>
	[
		new CommunityRecipe
		{
			Name = "Spaghetti carbonara",
			Author = "lucia_k",
			Description = "Rich, peppery and surprisingly quick. No cream needed.",
			Servings = 2,
			PrepMinutes = 5,
			CookMinutes = 12,
			Tags = ["italian", "quick", "pasta"],
			IngredientLines = ["200 g spaghetti", "100 g pancetta", "2 eggs", "50 g pecorino", "Black pepper"]
		},
		new CommunityRecipe
		{
			Name = "Chickpea and spinach curry",
			Author = "dev_sharma",
			Description = "Store-cupboard curry that tastes like it simmered all afternoon.",
			Servings = 4,
			PrepMinutes = 10,
			CookMinutes = 25,
			Tags = ["vegan", "indian", "one-pot"],
			IngredientLines = ["400 g chickpeas", "200 g spinach", "1 onion", "400 ml coconut milk", "2 tbsp curry powder"]
		},
		new CommunityRecipe
		{
			Name = "Sheet pan lemon chicken",
			Author = "marta_p",
			Description = "Everything roasts together, so cleanup is basically one tray.",
			Servings = 4,
			PrepMinutes = 15,
			CookMinutes = 40,
			Tags = ["sheet-pan", "chicken", "weeknight"],
			IngredientLines = ["4 chicken thighs", "600 g potatoes", "2 lemons", "Olive oil", "Rosemary"]
		},
		new CommunityRecipe
		{
			Name = "Miso salmon rice bowl",
			Author = "kenji_w",
			Description = "Glazed salmon over rice with cucumber. Fifteen minutes, start to finish.",
			Servings = 2,
			PrepMinutes = 8,
			CookMinutes = 10,
			Tags = ["japanese", "quick", "seafood"],
			IngredientLines = ["2 salmon fillets", "2 tbsp white miso", "300 g rice", "1 cucumber", "Sesame seeds"]
		},
		new CommunityRecipe
		{
			Name = "Mushroom risotto",
			Author = "sofia_r",
			Description = "Worth the stirring. Very good with a hard cheese on top.",
			Servings = 4,
			PrepMinutes = 10,
			CookMinutes = 35,
			Tags = ["vegetarian", "italian", "comfort"],
			IngredientLines = ["300 g risotto rice", "400 g mushrooms", "1 l stock", "Parmesan", "White wine"]
		},
		new CommunityRecipe
		{
			Name = "Shakshuka",
			Author = "amir_h",
			Description = "Eggs poached in a spiced tomato sauce. Good for lazy brunches.",
			Servings = 3,
			PrepMinutes = 10,
			CookMinutes = 25,
			Tags = ["vegetarian", "brunch", "one-pan"],
			IngredientLines = ["6 eggs", "800 g tomatoes", "2 peppers", "1 onion", "Cumin and paprika"]
		}
	];
}