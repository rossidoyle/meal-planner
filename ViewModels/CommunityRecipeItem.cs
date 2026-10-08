using CommunityToolkit.Mvvm.ComponentModel;
using MealPlanner.Models;

namespace MealPlanner.ViewModels;

public partial class CommunityRecipeItem(CommunityRecipe recipe, bool isInLibrary) : ObservableObject
{
	public CommunityRecipe Recipe { get; } = recipe;

	[ObservableProperty]
	public partial bool IsInLibrary { get; set; } = isInLibrary;
}