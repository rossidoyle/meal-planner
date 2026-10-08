using MealPlanner.Views;

namespace MealPlanner;

public partial class App : Application
{
	public App(RecipeListPage recipesPage, CommunityPage communityPage)
	{
		InitializeComponent();

		RecipesPage = recipesPage;
		CommunityPage = communityPage;

		AppDomain.CurrentDomain.UnhandledException += (_, args) =>
			System.Diagnostics.Debug.WriteLine($"Unhandled exception: {args.ExceptionObject}");

		TaskScheduler.UnobservedTaskException += (_, args) =>
			System.Diagnostics.Debug.WriteLine($"Unobserved task exception: {args.Exception}");
	}

	RecipeListPage RecipesPage { get; }

	CommunityPage CommunityPage { get; }

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell(RecipesPage, CommunityPage));
	}
}