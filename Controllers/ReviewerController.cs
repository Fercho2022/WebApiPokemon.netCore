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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewDtos = _mapper.Map<List<ReviewDto>>(_reviewerRepository.GetReviewsByReviewer(reviewerId));

            return Ok(reviewDtos);

        }

        
    }
}
