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


        [HttpPost]
        [ProducesResponseType(402)]
        [ProducesResponseType(502)]
        public IActionResult CreateCountry(CountryDto countryDto)
        {
            if(countryDto == null) return BadRequest(ModelState);

            var countries = _countryRepository.GetCountries().Where(C => C.Name.Trim().ToUpper() == countryDto.Name.Trim().ToUpper()).FirstOrDefault();
            
            if(countries != null)
            {
                ModelState.AddModelError("","Cannot create two countries with same name.");
                return StatusCode(402,ModelState);
            }

            var countryMap = _mapper.Map<Country>(countryDto);
            if (countryMap == null) return BadRequest(ModelState);

            if (!_countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Unsuccessfull operation. Try again!");
                return StatusCode(502, ModelState);
            }
            return Ok("Successfully Created Country.");
        }

        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto countryDto)
        {

            if (countryDto == null) return BadRequest(ModelState);

            if (countryId != countryDto.Id) return BadRequest(ModelState);

            if (!_countryRepository.CountryExists(countryId)) return NotFound();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedCountry = _mapper.Map<Country>(countryDto);
            if (!_countryRepository.UpdateCountry(updatedCountry))
            {
                ModelState.AddModelError("", "Unsuccessfull operation. Try again!");
                return StatusCode(500, ModelState);
            }
            return Ok("success");
        }
    }
}
