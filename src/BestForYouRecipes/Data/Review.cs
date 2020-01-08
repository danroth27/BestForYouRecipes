using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BestForYouRecipes
{
    public class Review
    {
        [Range(0, 5)]
        public double Rating { get; set; }

        public string Text { get; set; }
    }
}
