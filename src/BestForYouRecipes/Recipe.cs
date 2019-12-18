using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestForYouRecipes
{
    public class Recipe
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public int PrepTime { get; set; }
        public int WaitTime { get; set; }
        public int CookTime { get; set; }
        public int Servings { get; set; }
        public string Comments { get; set; }
        public string Instructions { get; set; }
        public string[] Ingredients { get; set; }
        public string[] Tags { get; set; }

        public Uri CardImageUrl
        {
            get
            {
                return new Uri($"images/cards/{Name}.png", UriKind.Relative);
            }
        }
    }
}
