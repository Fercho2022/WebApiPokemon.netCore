using System.Security.Cryptography.Xml;
using WebApiPokemon.Data;
using WebApiPokemon.Interfaces;
using WebApiPokemon.Models;

namespace WebApiPokemon.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;


        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();


        }

        public Category GetCategory(int id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == id);
        }

        public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
        {
            //Filtrado con Where: Primero, usamos Where para obtener solo las filas
            //de PokemonCategory que tienen el CategoryId igual al categoryId
            //proporcionado

            //Proyección con Select: Luego, usamos Select para proyectar(transformar)
            //estos objetos PokemonCategory a los objetos Pokemon correspondientes

            var pokemons = _context.PokemonCategories
                .Where(pc => pc.CategoryId == categoryId)  // Filtrar por el ID de la categoría
                .Select(pc => pc.Pokemon)   // Proyectar a objetos Pokemon
                .ToList();   // Convertir el resultado a una lista

            return pokemons;   // Retornar la lista de Pokémon

        }
        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public bool CreateCategory(Category category)
        {
            _context.Add(category);

            return Save();
        }

        public bool Save()
        {
            var saved= _context.SaveChanges();

            return saved > 0 ? true : false;
        }
    }
}
