using AutoMapper;


using Microsoft.AspNetCore.Mvc;


using WebApiPokemon.Dto;
using WebApiPokemon.Interfaces;
using WebApiPokemon.Models;
using WebApiPokemon.Repository;



namespace WebApiPokemon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller

    {
        // Inyecto las dpendencias de PokemonRepository y Mapper, ambas como Interface
        // ya que la clase PokemonRepository implementa la interfaz
        // IPokemonRepsoitory y Mapper implementa implicitamente a
        // IMapper

        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;
        private readonly IReviewRepository _reviewRepository;


        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper, IReviewRepository reviewRepository)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
            _reviewRepository = reviewRepository;

        }
            [HttpGet]
            [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
            public IActionResult GetPokemons()
            {

                //ModelState.IsValid es una propiedad que indica si el
                //modelo recibido y validado en el controlador es válido
                //o no.

                //La validez se determina según las reglas de validación
                //que se hayan aplicado a las propiedades del modelo en
                //cuestión. Estas reglas pueden ser atributos de
                //validación como Required, StringLength, Range, etc.,
                //que se aplican a propiedades del modelo.

                //Si ModelState.IsValid es false, significa que al menos una
                //regla de validación ha fallado para los datos recibidos.

                // BadRequest(ModelState) devuelve una respuesta HTTP 400 Bad Request
                // junto con los detalles de los errores de validación contenidos en
                // ModelState. Esto permite al cliente de la API entender por qué la
                // solicitud no pudo ser procesada debido a datos incorrectos o faltantes.

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // AutoMapper: Es una biblioteca que se utiliza para simplificar
                // la conversión de un objeto de un tipo a otro. Esto es especialmente
                // útil cuando trabajamos con DTOs(Data Transfer Objects) y entidades
                // del dominio, permitiendo evitar la escritura manual de código de
                // conversión.

                //_mapper.Map<List<PokemonDto>>():
                //Utiliza un objeto _mapper, que es una instancia de la clase Mapper de la biblioteca AutoMapper.

                //Map<List<PokemonDto>> indica que queremos mapear los objetos obtenidos
                //en una lista de objetos de tipo PokemonDto.
                //.

                var pokemonsDto = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());
                return Ok(pokemonsDto);
            }

            [HttpGet("{pokeId:int}")]
            [ProducesResponseType(200, Type = typeof(Pokemon))]
            [ProducesResponseType(400)]
            public IActionResult GetPokemon(int pokeId)

            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);

                }

                if (!_pokemonRepository.PokemonExists(pokeId))
                {

                    return NotFound();
                }

                var pokemonDto = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId));


                return Ok(pokemonDto);



            }

            [HttpGet("{pokeId}/rating")]
            [ProducesResponseType(200, Type = typeof(decimal))] // 
            [ProducesResponseType(400)]

            public IActionResult GetPokemonRating(int pokeId)

            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);

                }

                if (!_pokemonRepository.PokemonExists(pokeId))
                {

                    return NotFound();
                }
                var rating = _pokemonRepository.GetPokemonRating(pokeId);
                return Ok(rating);
            }

            [HttpPost]
            [ProducesResponseType(204)]
            [ProducesResponseType(400)]

            public IActionResult CreatePokemon([FromQuery] int categoryId, [FromQuery] int ownerId, [FromBody] PokemonDto pokemonCreate)
            {
                if (pokemonCreate == null)
                    return BadRequest(ModelState);

                var pokemon = _pokemonRepository.GetPokemons().Where(c => c.Name.Trim().ToUpper() == pokemonCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

                if (pokemon != null)
                {
                    ModelState.AddModelError("", "Pokemon already exist");
                    return StatusCode(422, ModelState);
                }

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);


                if (!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
                {
                    ModelState.AddModelError("", "Samething went wrong while savin");
                    return StatusCode(500, ModelState);
                }

                return Ok("Successfully created");
            }

        [HttpPut("{pokeId}")]
        [ProducesResponseType(400)]   // 400 es un Bad Request
        [ProducesResponseType(204)]    // 204 es un No Content
        [ProducesResponseType(404)]     // 404 es un not found
        public IActionResult UpdatePokemon([FromBody] PokemonDto pokemonUpdate, 
            int pokeId)
        {
            if (pokemonUpdate == null)
                return BadRequest(ModelState);

            if (pokeId != pokemonUpdate.Id)
                return BadRequest(ModelState);

            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(pokemonUpdate);

            if (!_pokemonRepository.UpdatePokemon(pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);

            }

            return Ok(pokemonUpdate);

        }

        [HttpDelete("{pokeId}")]
        [ProducesResponseType(400)]   // 400 es un Bad Request
        [ProducesResponseType(204)]    // 204 es un No Content
        [ProducesResponseType(404)]     // 404 es un not found

        public IActionResult DeletePokemon(int pokeId)
        {

            if (!_pokemonRepository.PokemonExists(pokeId))
            {
                return NotFound();


            }
            var reviewsToDelete = _reviewRepository.GetReviewsByPokemon(pokeId);
            var pokemonToDelete = _pokemonRepository.GetPokemon(pokeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReviews(reviewsToDelete.ToList())){

                ModelState.AddModelError("", "Something went wrong deleting reviews");
                return StatusCode(500, ModelState);
            }

           

            if (!_pokemonRepository.DeletePokemon(pokemonToDelete))
            {

                ModelState.AddModelError("", "Something went wrong deleting owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
    }
    }
