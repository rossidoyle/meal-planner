using MealPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace MealPlanner.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
	public DbSet<Recipe> Recipes => Set<Recipe>();

	public DbSet<Ingredient> Ingredients => Set<Ingredient>();

	public DbSet<MealPlanEntry> MealPlanEntries => Set<MealPlanEntry>();

	public DbSet<CommunityRecipe> CommunityRecipes => Set<CommunityRecipe>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Recipe>(entity =>
		{
			entity.HasIndex(e => e.Name);
			entity.HasIndex(e => e.SourceId);

			entity.HasMany(e => e.Ingredients)
				.WithOne(e => e.Recipe!)
				.HasForeignKey(e => e.RecipeId)
				.OnDelete(DeleteBehavior.Cascade);
		});

		modelBuilder.Entity<Ingredient>(entity =>
		{
			entity.HasIndex(e => new { e.RecipeId, e.SortOrder }).IsUnique();
		});

		modelBuilder.Entity<MealPlanEntry>(entity =>
		{
			entity.HasIndex(e => new { e.Date, e.Slot }).IsUnique();

			entity.HasOne(e => e.Recipe)
				.WithMany()
				.HasForeignKey(e => e.RecipeId)
				.OnDelete(DeleteBehavior.SetNull);
		});

		modelBuilder.Entity<CommunityRecipe>(entity =>
		{
			entity.HasIndex(e => e.Name);
			entity.HasIndex(e => e.Author);
		});
	}
}