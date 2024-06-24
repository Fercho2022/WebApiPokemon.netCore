using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiPokemon.Dto;
using WebApiPokemon.Interfaces;
using WebApiPokemon.Models;

namespace WebApiPokemon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {

        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }

        // GET: api/Owner
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]  // Especifica que este endpoint puede devolver un 200 con un IEnumerable<OwnerDto>
        [ProducesResponseType(400)] // Especifica que este endpoint puede devolver un 400
        public IActionResult GetOwners()
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var owners = _ownerRepository.GetOwners();  // Obtiene la lista de propietarios del repositorio
            var ownersDto = _mapper.Map<List<OwnerDto>>(owners); // Mapea la lista de propietarios a una lista de OwnerDto
            return Ok(ownersDto);   // Retorna un Ok con la lista de OwnerDto
        }

        // GET: api/Owner/5
        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))] // Especifica que este endpoint puede devolver un 200 con un OwnerDto
        [ProducesResponseType(400)] // Especifica que este endpoint puede devolver un 400
        [ProducesResponseType(404)] // Especifica que este endpoint puede devolver un 404
        public IActionResult GetOwner(int ownerId)
        {
            if (!ModelState.IsValid) // Verifica si el estado del modelo es válido
                return BadRequest(ModelState); // Si no es válido, retorna un BadRequest con el estado del modelo

            if (!_ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();  // Si no existe, retorna un NotFound
            }

            var owner = _ownerRepository.GetOwner(ownerId); // Obtiene la lista de Pokémon del propietario con el ID dado
            var ownerDto = _mapper.Map<OwnerDto>(owner);    // Mapea la lista de Pokémon a una lista de PokemonDto
            return Ok(ownerDto);    // Retorna un Ok con la lista de PokemonDto
        }

        // GET: api/Owner/Pokemon/5
        [HttpGet("Pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnersOfAPokemon(int pokeId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var owners = _ownerRepository.GetOwnersOfAPokemon(pokeId);
            var ownersDto = _mapper.Map<List<OwnerDto>>(owners);
            return Ok(ownersDto);
        }

        // GET: api/Owner/5/Pokemon
        [HttpGet("{ownerId}/Pokemon")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPokemonByAnOwner(int ownerId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_ownerRepository.OwnerExists(ownerId)) // Verifica si el propietario con el ID dado existe en el repositorio
            {
                return NotFound(); // Retorna un NotFound si el propietario no existe
            }

            var pokemons = _ownerRepository.GetPokemonByAnOwner(ownerId); // Obtiene la lista de Pokémon del propietario con el ID dado
            var pokemonsDto = _mapper.Map<List<PokemonDto>>(pokemons); // Mapea la lista de Pokémon a una colección de PokemonDto
            return Ok(pokemonsDto); // Retorna un Ok con la colección de PokemonDto
        }
    }
}

    

    

