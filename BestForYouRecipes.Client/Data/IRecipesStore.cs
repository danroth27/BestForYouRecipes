namespace BestForYouRecipes;

public interface IRecipesStore
{
    Task<IEnumerable<Recipe>> GetRecipes(string? query);

    Task<Recipe?> GetRecipe(string id);

    Task<Recipe> UpdateRecipe(Recipe recipe);

    Task DownloadImage(string filename, Stream stream);

    Task<string> AddRecipe(Recipe recipe);

    Task<string> AddImage(Stream imageData);
}
