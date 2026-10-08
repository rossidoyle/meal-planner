using MealPlanner.ViewModels;

namespace MealPlanner.Views;

public partial class CommunityPage : ContentPage
{
	readonly CommunityViewModel viewModel;

	public CommunityPage(CommunityViewModel viewModel)
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