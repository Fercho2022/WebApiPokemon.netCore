using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.ConstrainedExecution;
using WebApiPokemon.Dto;
using WebApiPokemon.Interfaces;
using WebApiPokemon.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
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

            var pokemonDto= _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId));
            

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
    }
}