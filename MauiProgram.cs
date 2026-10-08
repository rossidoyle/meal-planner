using CommunityToolkit.Maui;
using MealPlanner.Data;
using MealPlanner.Services;
using MealPlanner.ViewModels;
using MealPlanner.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MealPlanner;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit(options => options.SetShouldEnableSnackbarOnWindows(true))
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddDbContextFactory<AppDbContext>(options =>
			options.UseSqlite($"Data Source={Path.Combine(FileSystem.AppDataDirectory, "mealplanner.db")}"));

builder.Services.AddSingleton<IDatabaseInitializer, DatabaseInitializer>();
		builder.Services.AddSingleton<ICommunitySeeder, CommunitySeeder>();
		builder.Services.AddSingleton<IRecipeService, RecipeService>();
		builder.Services.AddSingleton<ICommunityRecipeService, LocalCommunityRecipeService>();
		builder.Services.AddSingleton<IUserFeedback, UserFeedback>();

	builder.Services.AddSingleton<RecipeListViewModel>();
	builder.Services.AddTransient<RecipeListPage>();
	builder.Services.AddSingleton<CommunityViewModel>();
	builder.Services.AddTransient<CommunityPage>();
	builder.Services.AddTransient<RecipeEditViewModel>();
	builder.Services.AddTransient<RecipeEditPage>();

		Routing.RegisterRoute(Routes.RecipeEdit, typeof(RecipeEditPage));

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}