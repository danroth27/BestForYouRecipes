using System.Collections.Concurrent;
using System.Globalization;
using System.Text.Json;

namespace BestForYouRecipes.Data;

public class RecipesStore : IRecipesStore
{
    private readonly Dictionary<string, Recipe> recipes;
    private readonly ConcurrentDictionary<string, byte[]> images = new();
    private readonly InMemorySearchProvider searchProvider;

    public RecipesStore(IHostEnvironment hostEnvironment)
    {
        var jsonFileInfo = hostEnvironment.ContentRootFileProvider.GetFileInfo(Path.Combine("Data", "recipes.json"));
        using var jsonStream = jsonFileInfo.CreateReadStream();
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };
        recipes = JsonSerializer.Deserialize<Dictionary<string, Recipe>>(jsonStream, jsonOptions)!;
        searchProvider = new InMemorySearchProvider(recipes);
    }

    public async Task<IEnumerable<Recipe>> GetRecipes(string? query)
    {
        // Simulate DB query
        await Task.Delay(1000);

        return string.IsNullOrWhiteSpace(query)
            ? recipes.Values
            : searchProvider.Search(query);
    }

    public Task<Recipe?> GetRecipe(string id)
    {
        recipes.TryGetValue(id, out var recipe);
        return Task.FromResult(recipe);
    }

    public Task<Recipe> UpdateRecipe(Recipe recipe)
    {
        recipes[recipe.Id] = recipe;
        return Task.FromResult(recipe);
    }

    public Task<string> AddRecipe(Recipe recipe)
    {
        recipe.Id = recipes.Count.ToString(CultureInfo.InvariantCulture);
        recipes.Add(recipe.Id, recipe);
        searchProvider = new InMemorySearchProvider(recipes);
        return Task.FromResult(recipe.Id);
    }

    public async Task<string> AddImage(Stream imageData)
    {
        using var ms = new MemoryStream();
        await imageData.CopyToAsync(ms);

        var filename = Guid.NewGuid().ToString();
        images[filename] = ms.ToArray();
        return $"images/uploaded/{filename}";
    }

    public Task DownloadImage(string filename, Stream stream)
    {
        return stream.WriteAsync(images[filename].AsMemory()).AsTask();
    }
}
