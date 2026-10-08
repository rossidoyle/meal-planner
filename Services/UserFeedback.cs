using System.Diagnostics;
using CommunityToolkit.Maui.Alerts;

namespace MealPlanner.Services;

public interface IUserFeedback
{
	Task ShowAsync(string message);

	Task ShowErrorAsync(string message, Exception exception);
}

public class UserFeedback : IUserFeedback
{
	public async Task ShowAsync(string message)
	{
		try
		{
			await Snackbar.Make(message).Show();
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Snackbar failed, falling back to an alert: {ex}");

			var page = Application.Current?.Windows.FirstOrDefault()?.Page;

			if (page is not null)
			{
				await page.DisplayAlertAsync("Meal Planner", message, "OK");
			}
		}
	}

	public async Task ShowErrorAsync(string message, Exception exception)
	{
		Debug.WriteLine($"{message} {exception}");

		await ShowAsync(message);
	}
}