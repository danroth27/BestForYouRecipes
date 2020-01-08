using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BestForYouRecipes
{
    public class JsonRecipesStore : IRecipesStore
    {
        IDictionary<string, Recipe> recipes;
        InMemorySearchProvider searchProvider;

        public JsonRecipesStore()
        {
            var json = File.ReadAllText("recipes.json");
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true
            };
            recipes = JsonSerializer.Deserialize<Dictionary<string, Recipe>>(json, jsonOptions);
            searchProvider = new InMemorySearchProvider(recipes);
        }

        public Task<IEnumerable<Recipe>> GetRecipes(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Task.FromResult<IEnumerable<Recipe>>(recipes.Values);
            }
            return Task.FromResult(searchProvider.Search(query));
        }

        public Task<Recipe> GetRecipe(string id)
        {
            recipes.TryGetValue(id, out var recipe);
            return Task.FromResult(recipe);
        }

        public Task<Recipe> UpdateRecipe(Recipe recipe)
        {
            return Task.FromResult(recipe);
        }
    }
}
