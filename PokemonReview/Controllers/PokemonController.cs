using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PokemonReview.Dtos;
using PokemonReview.Interfaces;
using PokemonReview.Models;
using PokemonReview.Repository;
using System.Collections.Generic;

namespace PokemonReview.Controllers
{
    [ApiController] // Added attribute to indicate this is an API controller
    [Route("api/[controller]")] // Route attribute to define the route pattern
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(pokemons);
        }


        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200,Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            var rating = _pokemonRepository.GetPokemonRating(pokeId);
            if(!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(rating);
        }


        [HttpGet("{pokeId:int}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
            {
                return NotFound();
            }

            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemon);
        }


        [HttpGet("{name}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(string name)
        {
            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(name));
            if (pokemon == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromQuery] int ownerId,[FromQuery] int categoryId, [FromBody] PokemonDto pokemonDto)
        {
            if (pokemonDto == null) return BadRequest(ModelState);
            var pokemon = _pokemonRepository.GetPokemons().Where(p => p.Name.Trim().ToUpper() == pokemonDto.Name.Trim().ToUpper()).FirstOrDefault();

            if (pokemon != null)
            {
                ModelState.AddModelError("", "Pokemon already exists.");
                return StatusCode(422, ModelState);
            }


            if (!ModelState.IsValid) return BadRequest(ModelState);

            var pokeMap = _mapper.Map<Pokemon>(pokemonDto);
             //pokeMap.Reviews = _review.GetReviews(countryId);
            if (!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokeMap))
            {
                ModelState.AddModelError("", "Something went wrong.Please Try again later.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created Pokemon.");
        }

        [HttpPut("{pokemonId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon(int pokemonId, [FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDto pokemonDto)
        {
            if(pokemonDto == null) return BadRequest(ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if(pokemonId != pokemonDto.Id) return BadRequest(ModelState);
            if (!_pokemonRepository.PokemonExists(pokemonId)) return NotFound();

            var updatedPokemon = _mapper.Map<Pokemon>(pokemonDto);
            if(!_pokemonRepository.UpdatePokemon(ownerId, categoryId,updatedPokemon))
            {
                ModelState.AddModelError("", "Unsuccessful operation. Try again!");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
