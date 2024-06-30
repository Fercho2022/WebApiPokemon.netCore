using Microsoft.EntityFrameworkCore;
using WebApiPokemon.Data;
using WebApiPokemon.Interfaces;
using WebApiPokemon.Models;

namespace WebApiPokemon.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _dataContext;

        public OwnerRepository(DataContext context)

        {
            _dataContext = context;
        }

        public ICollection<Owner> GetOwners()
        {
            return _dataContext.Owners.ToList();

        }

        public Owner GetOwner(int ownerId)
        {
            return _dataContext.Owners.FirstOrDefault(o => o.Id == ownerId);
        }

        public ICollection<Owner> GetOwnersOfAPokemon(int pokeId)
        {
            return _dataContext.PokemonOwners
                .Where(o => o.Pokemon.Id == pokeId) // Filtra los registros donde el Pokémon tiene el ID especificado
                .Select(o => o.Owner)               // Selecciona los propietarios de esos registros
                .ToList();                          // Convierte el resultado en una lista
        }

        public ICollection<Pokemon> GetPokemonByAnOwner(int ownerId)
        {
           return _dataContext.PokemonOwners
                
                .Where(o=>o.Owner.Id==ownerId)  // Filtra por el ID del propietario
                .Select(o => o.Pokemon)     // Selecciona los Pokémon asociados
                .ToList();              // Convierte el resultado en una lista
        }

        public bool OwnerExists(int ownerId)
        {
            return _dataContext.Owners.Any(o => o.Id == ownerId);
        }

        public bool CreateOwner(Owner owner)
        {
            _dataContext.Add(owner);
            return Save();
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
