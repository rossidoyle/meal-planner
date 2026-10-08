using MealPlanner.ViewModels;

namespace MealPlanner.Views;

public partial class RecipeListPage : ContentPage
{
	readonly RecipeListViewModel viewModel;

	public RecipeListPage(RecipeListViewModel viewModel)
	{
		InitializeComponent();

		this.viewModel = viewModel;
		BindingContext = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		await viewModel.AppearingAsync();
	}
}