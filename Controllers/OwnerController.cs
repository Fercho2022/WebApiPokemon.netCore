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
    public class OwnerController : Controller
    {

        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository, IMapper mapper, ICountryRepository countryRepository)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
            _countryRepository = countryRepository;
        }

        // GET: api/Owner
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]  // Especifica que este endpoint puede devolver un 200 con un IEnumerable<OwnerDto>
        [ProducesResponseType(400)] // Especifica que este endpoint puede devolver un 400
        public IActionResult GetOwners()
        {
            var owners = _ownerRepository.GetOwners();  // Obtiene la lista de propietarios del repositorio
            var ownersDto = _mapper.Map<List<OwnerDto>>(owners); // Mapea la lista de propietarios a una lista de OwnerDto

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            return Ok(ownersDto);   // Retorna un Ok con la lista de OwnerDto
        }

        // GET: api/Owner/5
        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))] // Especifica que este endpoint puede devolver un 200 con un OwnerDto
        [ProducesResponseType(400)] // Especifica que este endpoint puede devolver un 400
        [ProducesResponseType(404)] // Especifica que este endpoint puede devolver un 404
        public IActionResult GetOwner(int ownerId)
        {


            if (!_ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();  // Si no existe, retorna un NotFound
            }

            var owner = _ownerRepository.GetOwner(ownerId); // Obtiene la lista de Pokémon del propietario con el ID dado

            var ownerDto = _mapper.Map<OwnerDto>(owner);    // Mapea la lista de Pokémon a una lista de PokemonDto

            if (!ModelState.IsValid) // Verifica si el estado del modelo es válido
                return BadRequest(ModelState); // Si no es válido, retorna un BadRequest con el estado del modelo

            return Ok(ownerDto);    // Retorna un Ok con la lista de PokemonDto
        }


        // GET: api/Owner/5/Pokemon
        [HttpGet("{ownerId}/Pokemon")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPokemonByAnOwner(int ownerId)
        {


            if (!_ownerRepository.OwnerExists(ownerId)) // Verifica si el propietario con el ID dado existe en el repositorio
            {
                return NotFound(); // Retorna un NotFound si el propietario no existe
            }

            var pokemons = _ownerRepository.GetPokemonByAnOwner(ownerId); // Obtiene la lista de Pokémon del propietario con el ID dado
            var pokemonsDto = _mapper.Map<List<PokemonDto>>(pokemons); // Mapea la lista de Pokémon a una colección de PokemonDto

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemonsDto); // Retorna un Ok con la colección de PokemonDto
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateCountry([FromQuery] int countryId, [FromBody] OwnerDto ownerCreate)
        {
            if (ownerCreate == null)
                return BadRequest(ModelState);

            var owner = _ownerRepository.GetOwners().Where(c => c.LastName.Trim().ToUpper() == ownerCreate.LastName.TrimEnd().ToUpper()).FirstOrDefault();

            if (owner != null)
            {
                ModelState.AddModelError("", "Owner already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownerMap = _mapper.Map<Owner>(ownerCreate);

           


            ownerMap.Country=_countryRepository.GetCountry(countryId);


            if (!_ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Samething went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }


    }





}