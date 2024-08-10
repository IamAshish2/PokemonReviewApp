
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.Dtos;
using PokemonReview.Interfaces;
using PokemonReview.Models;

namespace PokemonReview.Controllers
{
    [ApiController]
    [Route("Api/[Controller]")]
    public class ReviewController:Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;

        public ReviewController(IReviewRepository reviewRepository,IMapper mapper,IPokemonRepository pokemonRepository,IReviewerRepository reviewerRepository)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int id)
        {
            if (!_reviewRepository.HasReview(id))
            {
                return NotFound(0);
            }
            var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(id));
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(review);
        }

        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewOfPokemon(int pokeId)
        {
            
            var reviewPoke = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewOfPokemon(pokeId));
            return Ok(reviewPoke);
        }

        [HttpPost]
        [ProducesResponseType(402)]
        [ProducesResponseType(500)]
        public IActionResult CreateReview([FromQuery] int pokemonId, [FromQuery] int reviewerId, [FromBody] ReviewDto reviewdto)
        {
            if(reviewdto == null) return BadRequest(ModelState);

            var review = _reviewRepository.GetReviews().Where(r => r.Title == reviewdto.Title).FirstOrDefault();
            if(review != null)
            {
                ModelState.AddModelError("", "The review already exists.");
                return StatusCode(402,ModelState);
            }

            var newreview = _mapper.Map<Review>(reviewdto);
            newreview.Pokemon = _pokemonRepository.GetPokemon(pokemonId);
            newreview.Reviewer = _reviewerRepository.GetReviewer(reviewerId);
            if (!_reviewRepository.CreateReview(newreview))
            {
                ModelState.AddModelError("","The model could not be created.");
                return StatusCode(502,ModelState);
            }
            return Ok("Successfully created new review.");
        }


        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReiewer(int reviewId,[FromBody] ReviewDto reviewDto)
        {
            if (reviewDto == null) return BadRequest(ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (reviewId != reviewDto.Id) return BadRequest(ModelState);

            if (!_reviewRepository.HasReview(reviewId)) return NotFound();

            var updatedReview = _mapper.Map<Review>(reviewDto);

            if (!_reviewRepository.UpdateReview(updatedReview))
            {
                ModelState.AddModelError("", "Unsuccessful operation. Try again!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
