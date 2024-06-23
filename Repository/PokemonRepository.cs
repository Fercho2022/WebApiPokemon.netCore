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
    }
}
