namespace MealPlanner;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
	}

	public AppShell(Page recipesPage, Page communityPage) : this()
	{
		Items.Add(new TabBar
		{
			Items =
			{
				new ShellContent
				{
					Title = "My recipes",
					Route = Routes.Recipes,
					Content = recipesPage
				},
				new ShellContent
				{
					Title = "Community",
					Route = Routes.Community,
					Content = communityPage
				}
			}
		});
	}
}