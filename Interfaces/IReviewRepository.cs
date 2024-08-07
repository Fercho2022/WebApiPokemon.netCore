﻿using WebApiPokemon.Models;

namespace WebApiPokemon.Interfaces
{
    public interface IReviewRepository
    {

        ICollection<Review> GetReviews();

        Review GetReview(int reviewId);

        bool ReviewExists(int reviewId);
        ICollection<Review> GetReviewsByPokemon(int pokeId);
        ICollection<Review> GetReviewsByReviewer(int reviewerId);

        bool CreateReview(Review review);

        bool UpdateReview(Review review);   

        bool DeleteReview(Review review);

        bool DeleteReviews(List<Review> reviews);

        bool Save();
    }
}
