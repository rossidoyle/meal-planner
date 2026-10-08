using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MealPlanner.Models;
using MealPlanner.Services;

namespace MealPlanner.ViewModels;

public partial class RecipeEditViewModel(IRecipeService recipeService, IUserFeedback feedback) : ObservableObject
{
	Recipe? loaded;

	[ObservableProperty]
	public partial string Name { get; set; } = string.Empty;

	[ObservableProperty]
	public partial string? Description { get; set; }

	[ObservableProperty]
	public partial int Servings { get; set; } = 2;

	[ObservableProperty]
	public partial int PrepMinutes { get; set; }

	[ObservableProperty]
	public partial int CookMinutes { get; set; }

	[ObservableProperty]
	public partial string? TagsText { get; set; }

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(Title))]
	public partial bool IsShared { get; set; }

	[ObservableProperty]
	public partial string? Author { get; set; }

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(Title))]
	[NotifyPropertyChangedFor(nameof(OriginText))]
	public partial bool IsExisting { get; set; }

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(SaveCommand))]
	public partial bool IsBusy { get; set; }

	public string Title => IsExisting ? "Edit recipe" : "New recipe";

	public string OriginText => loaded is { Origin: RecipeOrigin.Community }
		? $"From the community by {loaded.Author}"
		: "Your own recipe";

	public async Task LoadAsync(int id)
	{
		if (id == 0)
		{
			loaded = null;
			IsExisting = false;
			OnPropertyChanged(nameof(OriginText));
			return;
		}

		Recipe? recipe;

		try
		{
			recipe = await recipeService.GetByIdAsync(id);
		}
		catch (Exception ex)
		{
			await feedback.ShowErrorAsync("Couldn't load that recipe.", ex);
			await CloseAsync();
			return;
		}

		if (recipe is null)
		{
			await feedback.ShowAsync("That recipe no longer exists.");
			await CloseAsync();
			return;
		}

		loaded = recipe;

		Name = recipe.Name;
		Description = recipe.Description;
		Servings = recipe.Servings;
		PrepMinutes = recipe.PrepMinutes;
		CookMinutes = recipe.CookMinutes;
		TagsText = string.Join(", ", recipe.Tags);
		IsShared = recipe.IsShared;
		Author = recipe.Author;
		IsExisting = true;
		OnPropertyChanged(nameof(OriginText));
	}

	[RelayCommand]
	private async Task SaveAsync()
	{
		if (IsBusy)
		{
			return;
		}

		if (string.IsNullOrWhiteSpace(Name))
		{
			await feedback.ShowAsync("Give the recipe a name first.");
			return;
		}

		var recipe = loaded ?? new Recipe();

		recipe.Name = Name;
		recipe.Description = Description;
		recipe.Servings = Servings;
		recipe.PrepMinutes = PrepMinutes;
		recipe.CookMinutes = CookMinutes;
		recipe.Tags = ParseTags(TagsText);
		recipe.IsShared = IsShared;
		recipe.Author = string.IsNullOrWhiteSpace(Author) ? null : Author.Trim();

		try
		{
			IsBusy = true;

			await recipeService.SaveAsync(recipe);

			await feedback.ShowAsync(IsExisting ? "Recipe updated." : "Recipe added.");
			await CloseAsync();
		}
		catch (Exception ex)
		{
			await feedback.ShowErrorAsync("Couldn't save that recipe.", ex);
		}
		finally
		{
			IsBusy = false;
		}
	}

	[RelayCommand]
	private Task CloseAsync() => Shell.Current.GoToAsync("..");

	static List<string> ParseTags(string? tagsText) => (tagsText ?? string.Empty)
		.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
		.ToList();
}