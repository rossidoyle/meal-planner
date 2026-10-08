using MealPlanner.ViewModels;

namespace MealPlanner.Views;

public partial class RecipeEditPage : ContentPage, IQueryAttributable
{
	readonly RecipeEditViewModel viewModel;

	public RecipeEditPage(RecipeEditViewModel viewModel)
	{
		InitializeComponent();

		this.viewModel = viewModel;
		BindingContext = viewModel;
	}

	public async void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		var id = query.TryGetValue("recipeId", out var value)
			&& int.TryParse(value?.ToString(), out var parsed)
				? parsed
				: 0;

		await viewModel.LoadAsync(id);
	}
}