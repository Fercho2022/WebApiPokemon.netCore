using Microsoft.EntityFrameworkCore;
using WebApiPokemon.Data;
using WebApiPokemon.Interfaces;
using WebApiPokemon.Models;

namespace WebApiPokemon.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {

        private readonly DataContext _dataContext;

        public ReviewerRepository(DataContext context) { 
                    
            _dataContext = context;
        }
        public Reviewer GetReviewer(int reviewerId)
        {
           return _dataContext.Reviewers.Where(r=>r.Id == reviewerId).Include(r=>r.Reviews).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
           return  _dataContext.Reviewers.ToList();

        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
           return _dataContext.Reviews.Where(r=>r.Reviewer.Id==reviewerId).ToList();
        }

        public bool ReviewerExists(int reviewerId)
        {
            return _dataContext.Reviewers.Any(r=>r.Id==reviewerId);
            
        }
    }
}
