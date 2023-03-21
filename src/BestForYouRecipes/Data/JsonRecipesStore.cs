using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace BestForYouRecipes
{
    public class JsonRecipesStore : IRecipesStore
    {
        IDictionary<string, Recipe> recipes;
        InMemorySearchProvider searchProvider;

        public JsonRecipesStore(IDictionary<string, Recipe> recipes)
        {
            this.recipes = recipes;
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

        public Task<Recipe?> GetRecipe(string id)
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
