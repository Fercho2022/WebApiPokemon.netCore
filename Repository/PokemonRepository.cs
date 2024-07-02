using WebApiPokemon.Data;
using WebApiPokemon.Interfaces;
using WebApiPokemon.Models;

namespace WebApiPokemon.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;
        public PokemonRepository(DataContext context) { 
        
            _context = context;
        }

        

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemon.OrderBy(p => p.Id).ToList();
        }


        public Pokemon GetPokemon(int id)
        {
            return _context.Pokemon.Find(id);

        }
        public Pokemon GetPokemon(string name)
        {
            return _context.Pokemon.FirstOrDefault(p => p.Name == name);
        }

        public decimal GetPokemonRating(int pokeId)
        {
            // Filtra las reviews asociadas al Pokémon con el ID dado
            var reviews = _context.Reviews.Where(r => r.Pokemon.Id == pokeId);

            // Si existen reviews, calcula y retorna el promedio del rating
            if (reviews.Any())
            {
                return (decimal)reviews.Average(r => r.Rating);
            }

            // Si no hay reviews, retorna 0
            return 0;
        }
        public bool PokemonExists(int pokeId)
        {
            return _context.Pokemon.Any(p => p.Id == pokeId);
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
           var OwnerEntity= _context.Owners.Where(o=>o.Id==ownerId).FirstOrDefault();
            var CategoryEntity=_context.Categories.Where(c=>c.Id==categoryId).FirstOrDefault();


            // Verificar si el propietario y la categoría existen
            if (OwnerEntity == null || CategoryEntity == null)
            {
                return false; // Retornar falso si no se encuentran
            }

            // Crear la relación entre Pokémon y Propietario
            var pokemonOwner = new PokemonOwner()
            {

                Owner = OwnerEntity,
                Pokemon = pokemon,
            };

            // rastrear esta nueva entidad pokemonOwner para que pueda ser insertada en la base de datos
            // cuando se llame a SaveChanges() en el contexto _context.

            _context.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Pokemon = pokemon,
                Category = CategoryEntity
            };

            // rastrear esta nueva entidad pokemonCategory para que pueda ser insertada en la base de datos
            // cuando se llame a SaveChanges() en el contexto _context.
            _context.Add(pokemonCategory);

            // rastrear esta nueva entidad pokemon para que pueda ser insertada en la base de datos
            // cuando se llame a SaveChanges() en el contexto _context.

            _context.Add(pokemon);

            return Save();
        }

        public bool Save()
        {
            var saved=_context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdatePokemon(Pokemon pokemon)
        {
            _context.Update(pokemon);
            return Save();
        }
    }
}
