using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiPokemon.Dto;
using WebApiPokemon.Interfaces;

namespace WebApiPokemon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        // GET: api/Review
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviews()
        {
          
            var reviewsDto = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());
            return Ok(reviewsDto);
        }

        // GET: api/Review/5
        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(ReviewDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var review = _reviewRepository.GetReview(reviewId);
            var reviewDto = _mapper.Map<ReviewDto>(review);
            return Ok(reviewDto);
        }

        // GET: api/Reviews/Pokemon/5
        [HttpGet("Pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsByPokemon(int pokeId)
        {
            var reviews = _reviewRepository.GetReviewsByPokemon(pokeId);
            var reviewsDto = _mapper.Map<ICollection<ReviewDto>>(reviews);
            return Ok(reviewsDto);
        }

        // GET: api/Reviews/Reviewer/5
        [HttpGet("Reviewer/{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            var reviews = _reviewRepository.GetReviewsByReviewer(reviewerId);
            var reviewsDto = _mapper.Map<ICollection<ReviewDto>>(reviews);
            return Ok(reviewsDto);
        }
    }
}
