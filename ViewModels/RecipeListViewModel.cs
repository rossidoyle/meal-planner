using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MealPlanner.Models;
using MealPlanner.Services;

namespace MealPlanner.ViewModels;

public partial class RecipeListViewModel(
	IRecipeService recipeService,
	IDatabaseInitializer databaseInitializer,
	IUserFeedback feedback) : ObservableObject
{
	bool isInitialized;

	public ObservableCollection<Recipe> Recipes { get; } = [];

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(Title))]
	public partial string? SearchText { get; set; }

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(LoadRecipesCommand))]
	[NotifyCanExecuteChangedFor(nameof(OpenRecipeCommand))]
	public partial bool IsBusy { get; set; }

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(Title))]
	public partial bool SharedOnly { get; set; }

	public bool IsEmpty => !IsBusy && Recipes.Count == 0;

	public string Title
	{
		get
		{
			var term = string.IsNullOrWhiteSpace(SearchText)
				? SharedOnly ? "Shared recipes" : "Recipes"
				: $"\"{SearchText.Trim()}\"";

			return term;
		}
	}

	public async Task AppearingAsync()
	{
		if (!isInitialized)
		{
			isInitialized = true;
			await databaseInitializer.InitializeAsync();
		}

		await LoadRecipesAsync();
	}

	[RelayCommand]
	private async Task LoadRecipesAsync()
	{
		if (IsBusy)
		{
			return;
		}

		try
		{
			IsBusy = true;

			var recipes = await recipeService.GetAllAsync(SearchText, SharedOnly);

			Recipes.Clear();
			foreach (var recipe in recipes)
			{
				Recipes.Add(recipe);
			}
		}
		catch (Exception ex)
		{
			await feedback.ShowErrorAsync("Couldn't load your recipes.", ex);
		}
		finally
		{
			IsBusy = false;
			OnPropertyChanged(nameof(IsEmpty));
		}
	}

	[RelayCommand]
	private async Task OpenRecipeAsync(Recipe? recipe)
	{
		var id = recipe?.Id ?? 0;
		await Shell.Current.GoToAsync($"{Routes.RecipeEdit}?recipeId={id}");
	}
}