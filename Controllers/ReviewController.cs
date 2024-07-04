using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using WebApiPokemon.Dto;
using WebApiPokemon.Interfaces;
using WebApiPokemon.Models;


namespace WebApiPokemon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IPokemonRepository _pokemonRepository;

        public ReviewController(IReviewRepository reviewRepository, IMapper mapper, IReviewerRepository reviewerRepository, IPokemonRepository pokemonRepository)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _reviewerRepository = reviewerRepository;
            _pokemonRepository = pokemonRepository;
        }

        // GET: api/Review
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviews()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
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

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviews = _reviewRepository.GetReviewsByReviewer(reviewerId);

            var reviewsDto = _mapper.Map<ICollection<ReviewDto>>(reviews);

            return Ok(reviewsDto);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateReview([FromQuery] int pokeId, [FromQuery] int reviewerId, [FromBody] ReviewDto reviewCreate)
        {
            if (reviewCreate == null)
                return BadRequest(ModelState);

            var reviews = _reviewRepository.GetReviews().Where(r => r.Title.Trim().ToUpper() == reviewCreate.Title.TrimEnd().ToUpper()).FirstOrDefault();

            if (reviews != null)
            {
                ModelState.AddModelError("", "Review already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(reviewCreate);

            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);
            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokeId);

            if (!_reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Samething went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]   // 400 es un Bad Request
        [ProducesResponseType(204)]    // 204 es un No Content
        [ProducesResponseType(404)]     // 404 es un not found
        public IActionResult UpdateReview([FromBody] ReviewDto reviewUpdate, int reviewId)
        {
            if (reviewUpdate == null)
                return BadRequest(ModelState);

            if (reviewId != reviewUpdate.Id)
                return BadRequest(ModelState);

            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(reviewUpdate);

            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong updating review");
                return StatusCode(500, ModelState);

            }

            return Ok();

        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(400)]   // 400 es un Bad Request
        [ProducesResponseType(204)]    // 204 es un No Content
        [ProducesResponseType(404)]     // 404 es un not found

        public IActionResult DeleteReview(int reviewId)
        {

            if (!_reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();


            }
          
            var reviewToDelete = _reviewRepository.GetReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReview(reviewToDelete))
            {

                ModelState.AddModelError("", "Something went wrong deleting review");
                return StatusCode(500, ModelState);
            }
                      

            return NoContent();

        }

    }
}
