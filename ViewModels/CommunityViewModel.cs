using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MealPlanner.Models;
using MealPlanner.Services;

namespace MealPlanner.ViewModels;

public partial class CommunityViewModel(
	ICommunityRecipeService communityRecipeService,
	IDatabaseInitializer databaseInitializer,
	IUserFeedback feedback) : ObservableObject
{
	bool isInitialized;

	public ObservableCollection<CommunityRecipeItem> Recipes { get; } = [];

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(Title))]
	public partial string? SearchText { get; set; }

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(LoadRecipesCommand))]
	public partial bool IsBusy { get; set; }

	public bool IsEmpty => !IsBusy && Recipes.Count == 0;

	public string Title => string.IsNullOrWhiteSpace(SearchText)
		? "Community"
		: $"\"{SearchText.Trim()}\"";

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

			var recipes = await communityRecipeService.BrowseAsync(SearchText);
			var importedSourceIds = await communityRecipeService.GetImportedSourceIdsAsync();

			Recipes.Clear();
			foreach (var recipe in recipes)
			{
				Recipes.Add(new CommunityRecipeItem(recipe, importedSourceIds.Contains(CommunityRecipe.ToSourceId(recipe.Id))));
			}
		}
		catch (Exception ex)
		{
			await feedback.ShowErrorAsync("Couldn't load community recipes.", ex);
		}
		finally
		{
			IsBusy = false;
			OnPropertyChanged(nameof(IsEmpty));
		}
	}

	[RelayCommand]
	private async Task ImportAsync(CommunityRecipeItem? item)
	{
		if (item is null || item.IsInLibrary)
		{
			return;
		}

		try
		{
			var recipe = await communityRecipeService.ImportAsync(item.Recipe.Id);

			item.IsInLibrary = true;

			await feedback.ShowAsync($"\"{recipe.Name}\" added to your library.");
		}
		catch (Exception ex)
		{
			await feedback.ShowErrorAsync($"Couldn't add \"{item.Recipe.Name}\" to your library.", ex);
		}
	}
}