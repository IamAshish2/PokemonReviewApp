
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

        public ReviewController(IReviewRepository reviewRepository,IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
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
    }
}
