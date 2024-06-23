using Microsoft.Identity.Client;
using WebApiPokemon.Models;

namespace WebApiPokemon.Interfaces
{
    public interface ICountryRepository
    {

        ICollection<Country> GetCountries();

        Country GetCountry(int id);

        Country GetCountryByOwner(int OwnerId);

        ICollection<Owner> GetOwnersFromACountry(int countryId);

        bool CountryExists(int id);
    }
}
