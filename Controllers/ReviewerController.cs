using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiPokemon.Dto;
using WebApiPokemon.Interfaces;
using WebApiPokemon.Models;
using WebApiPokemon.Repository;

namespace WebApiPokemon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)

        {
            _mapper = mapper;
            _reviewerRepository = reviewerRepository;
        }

        // GET: api/Reviewer
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult GetReviewers()
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var reviewersDto = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers());
            return Ok(reviewersDto);
        }

        // GET: api/Reviewer/{reviewerId}
        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int reviewerId)

        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }

            var reviewerDto = _mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewer(reviewerId));


            return Ok(reviewerDto);

        }
        // GET: api/Reviewer/{reviewerId}/reviews
        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsByReviewer(int reviewerId)
        {

            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }

            var reviewDtos = _mapper.Map<List<ReviewDto>>(_reviewerRepository.GetReviewsByReviewer(reviewerId));
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           

            return Ok(reviewDtos);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateReviewer([FromBody] ReviewerDto reviewerCreate)
        {
            if (reviewerCreate == null)
                return BadRequest(ModelState);

            var reviewer = _reviewerRepository.GetReviewers().Where(c => c.LastName.Trim().ToUpper() == reviewerCreate.LastName.TrimEnd().ToUpper()).FirstOrDefault();

            if (reviewer != null)
            {
                ModelState.AddModelError("", "Reviewer already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);

            if (!_reviewerRepository.CreateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Samething went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(400)]   // 400 es un Bad Request
        [ProducesResponseType(204)]    // 204 es un No Content
        [ProducesResponseType(404)]     // 404 es un not found
        public IActionResult UpdateReviewerr([FromBody] ReviewerDto reviewerUpdate, int reviewerId)
        {
            if (reviewerUpdate == null)
                return BadRequest(ModelState);

            if (reviewerId != reviewerUpdate.Id)
                return BadRequest(ModelState);

            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerMap = _mapper.Map<Reviewer>(reviewerUpdate);

            if (!_reviewerRepository.UpdateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);

            }

            return Ok();

        }

        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(400)]   // 400 es un Bad Request
        [ProducesResponseType(204)]    // 204 es un No Content
        [ProducesResponseType(404)]     // 404 es un not found

        public IActionResult DeleteReview(int reviewerId)
        {

            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();

            }

            var reviewerToDelete = _reviewerRepository.GetReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewerRepository.DeleteReviewer(reviewerToDelete))
            {

                ModelState.AddModelError("", "Something went wrong deleting reviewer");
                return StatusCode(500, ModelState);
            }


            return NoContent();

        }
    }
}
