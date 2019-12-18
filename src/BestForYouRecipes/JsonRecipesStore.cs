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

        public JsonRecipesStore()
        {
            var json = File.ReadAllText("recipes.json");
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true
            };
            recipes = JsonSerializer.Deserialize<Dictionary<string, Recipe>>(json, jsonOptions);
        }

        public Task<IEnumerable<Recipe>> GetRecipes()
        {
            return Task.FromResult<IEnumerable<Recipe>>(recipes.Values);
        }
    }
}
