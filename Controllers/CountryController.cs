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
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;

        private readonly IMapper _mapper;
        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;

            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var countriesDto = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());
            return Ok(countriesDto);
        }

        [HttpGet("{countryId:int}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int countryId)

        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            if (!_countryRepository.CountryExists(countryId))
            {

                return NotFound();
            }

            var countryDto = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));


            return Ok(countryDto);

        }

        [HttpGet("/country/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryOfAnOwner(int ownerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            var countryDto = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(ownerId));

            return Ok(countryDto);

        }

        [HttpGet("/ownwers/{countryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnersOfAnCountry(int countryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            var ownerDto = _mapper.Map<List<OwnerDto>>(_countryRepository.GetOwnersFromACountry(countryId));

            return Ok(ownerDto);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateCountry([FromBody] CountryDto countryCreate)
        {
            if (countryCreate == null)
                return BadRequest(ModelState);

            var country = _countryRepository.GetCountries().Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", "Country already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryMap = _mapper.Map<Country>(countryCreate);

            if (!_countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Samething went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }
    }
}