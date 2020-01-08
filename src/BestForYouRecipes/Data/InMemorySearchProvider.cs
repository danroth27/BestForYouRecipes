using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestForYouRecipes
{
    public class InMemorySearchProvider
    {
        readonly IDictionary<string, Recipe> recipes;
        IDictionary<string, ICollection<(string RecipeId, int Count)>> searchIndex;

        public InMemorySearchProvider(IDictionary<string, Recipe> recipes)
        {
            this.recipes = recipes;
            BuildSearchIndex();
        }

        void BuildSearchIndex()
        {
            // Build search index based on name, tags, and ingredients
            searchIndex = new Dictionary<string, ICollection<(string, int)>>();
            foreach (var recipe in recipes.Values)
            {
                var terms = recipe.Name.ToLower().Split()
                    .Concat(recipe.Tags.Select(tag => tag.ToLower()))
                    .Concat(recipe.Ingredients.SelectMany(ingredient => ingredient.ToLower().Split()))
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
            return query.ToLower().Split()
                .Where(term => !string.IsNullOrWhiteSpace(term))
                .SelectMany(term => searchIndex.Keys
                    .Where(key => key.StartsWith(term))
                    .SelectMany(key => searchIndex[key]))
                .GroupBy(termCount => termCount.RecipeId, termCount => termCount.Count)
                .OrderByDescending(termCounts => termCounts.Sum())
                .Select(termCounts => recipes[termCounts.Key]);
        }
    }
}
