using WebApiPokemon.Models;

namespace WebApiPokemon.Interfaces
{
    public interface IOwnerRepository
    {

        ICollection<Owner> GetOwners();

        Owner GetOwner(int ownerId);

        ICollection<Pokemon> GetPokemonByAnOwner(int ownerId);

        ICollection<Owner> GetOwnersOfAPokemon(int pokeIdId);

        bool OwnerExists(int ownerId);

        bool CreateOwner(Owner owner);

        bool Save();


    }
}
