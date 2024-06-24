using WebApiPokemon.Data;
using WebApiPokemon.Interfaces;
using WebApiPokemon.Models;

namespace WebApiPokemon.Repository
{
    public class ReviewRepository : IReviewRepository
    {

        private readonly DataContext _context;

        public ReviewRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public Review GetReview(int reviewId)
        {
            return _context.Reviews.FirstOrDefault(r => r.Id == reviewId);
        }

        public bool ReviewExists(int reviewId)
        {
            return _context.Reviews.Any(r => r.Id == reviewId);
        }

        public ICollection<Review> GetReviewsByPokemon(int pokeId)
        {
            return _context.Reviews.Where(r=>r.Pokemon.Id == pokeId).ToList();

        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
           return  _context.Reviews.Where(r=>r.Reviewer.Id == reviewerId).ToList();
        }
    }
}