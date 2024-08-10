using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.Dtos;
using PokemonReview.Interfaces;
using PokemonReview.Models;

namespace PokemonReview.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewers()
        {
            var reviewers = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers());
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(reviewers);
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int reviewerId)
        {
            if(!_reviewerRepository.ReviewerExists(reviewerId)) return BadRequest(ModelState);  

            var reviewer = _mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewer(reviewerId));

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(reviewer);
        }


        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId)) return BadRequest("no reviewer with the reviewerId found!");

            var reviews = _mapper.Map<List<ReviewDto>>(_reviewerRepository.GetReviewsByReviewer(reviewerId));

            //if(!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(402)]
        [ProducesResponseType(502)]
        public IActionResult CreateReviewer(ReviewerDto reviewerDto)
        {
            if(reviewerDto == null) return BadRequest(ModelState);  

            var reviewers = _reviewerRepository.GetReviewers().Where(r => r.Id == reviewerDto.Id).FirstOrDefault();
            if (reviewers != null)
            {
                ModelState.AddModelError("","The reviewer already exists.");
                return StatusCode(402,ModelState);
            }

            var newReviewer = _mapper.Map<Reviewer>(reviewerDto);
            if (!_reviewerRepository.CreateReviewer(newReviewer))
            {
                ModelState.AddModelError("", "Operation Failed. Please Try again later.");
                return StatusCode(402, ModelState);
            }

            return Ok("Operation Successfull.");
        }


        [HttpPut("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReiewer(int reviewerId, [FromBody] ReviewerDto reviewerDto)
        {
            if (reviewerDto == null) return BadRequest(ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (reviewerId != reviewerDto.Id) return BadRequest(ModelState);

            if (!_reviewerRepository.ReviewerExists(reviewerId)) return NotFound();

            var updatedReviewer = _mapper.Map<Reviewer>(reviewerDto);
            if (!_reviewerRepository.UpdateReviewer(updatedReviewer))
            {
                ModelState.AddModelError("", "Unsuccessful operation. Try again!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
