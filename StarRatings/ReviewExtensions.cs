using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarRatings
{
    public static class ReviewExtensions
    {
        public static double AverageRating(this IList<Review> reviews)
            => Math.Round(reviews.Select(review => review.Rating).DefaultIfEmpty(0).Average(), 1);
    }
}
