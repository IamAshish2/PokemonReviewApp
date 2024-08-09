using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReview.Dtos;
using PokemonReview.Interfaces;
using PokemonReview.Models;

namespace PokemonReview.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository,IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            var countries =  _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(countries);
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        public IActionResult GetCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }
            var country = _mapper.Map<Country>(_countryRepository.GetCountry(countryId));
            return Ok(country);
        }

        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        public IActionResult GetCountryByOwner(int ownerId)
        {
            var country = _countryRepository.GetCountryByOwner(ownerId);
            return Ok(country);
        }

        //[HttpGet("country/{Id}")]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        //public IActionResult GetOwnersFromACountry(int id)
        //{
        //    if (!_countryRepository.CountryExists(id))
        //    {
        //        return NotFound();
        //    }

        //    var owners = _countryRepository.GetOwnersFromACountry(id);
        //    return Ok(owners);
        //}


    }
}
