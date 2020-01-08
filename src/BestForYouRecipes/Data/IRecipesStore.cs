using System.Collections.Generic;
using System.Threading.Tasks;

namespace BestForYouRecipes
{
    public interface IRecipesStore
    {
        Task<IEnumerable<Recipe>> GetRecipes(string query = "");

        Task<Recipe> GetRecipe(string id);

        Task<Recipe> UpdateRecipe(Recipe recipe);
    }
}