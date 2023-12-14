using System.Globalization;

namespace BestForYouRecipes.Data;

public class InMemorySearchProvider
{
    readonly Dictionary<string, Recipe> recipes;
    readonly Dictionary<string, ICollection<(string RecipeId, int Count)>> searchIndex;

    public InMemorySearchProvider(Dictionary<string, Recipe> recipes)
    {
        this.recipes = recipes;

        // Build search index based on name, tags, and ingredients
        searchIndex = new Dictionary<string, ICollection<(string, int)>>();
        foreach (var recipe in recipes.Values)
        {
            var terms = recipe.Name.ToLower(CultureInfo.CurrentCulture).Split()
                .Concat(recipe.Tags.Select(tag => tag.ToLower(CultureInfo.CurrentCulture)))
                .Concat(recipe.Ingredients.SelectMany(ingredient => ingredient.ToLower(CultureInfo.CurrentCulture).Split()))
                .GroupBy(term => term)
                .Select<IGrouping<string, string>, (string Term, int TermCount)>(termGroup => (termGroup.Key, termGroup.Count()));
            foreach (var term in terms)
            {
                if (!searchIndex.ContainsKey(term.Term))
                {
                    searchIndex[term.Term] = new List<(string, int)>();
                }
                searchIndex[term.Term].Add((recipe.Id, term.TermCount));
            }
        }
    }

    public IEnumerable<Recipe> Search(string query)
    {
        return query.ToLower(CultureInfo.CurrentCulture).Split()
            .Where(term => !string.IsNullOrWhiteSpace(term))
            .SelectMany(term => searchIndex.Keys
                .Where(key => key.StartsWith(term, StringComparison.CurrentCultureIgnoreCase))
                .SelectMany(key => searchIndex[key]))
            .GroupBy(termCount => termCount.RecipeId, termCount => termCount.Count)
            .OrderByDescending(termCounts => termCounts.Sum())
            .Select(termCounts => recipes[termCounts.Key]);
    }
}
