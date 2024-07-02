using WebApiPokemon.Data;
using WebApiPokemon.Interfaces;
using WebApiPokemon.Models;

namespace WebApiPokemon.Repository
{
    public class CountryRepository : ICountryRepository
    {

        private readonly DataContext _context;

       
        public CountryRepository(DataContext context)
        {
            _context = context;
            
        }
        public ICollection<Country> GetCountries()
        {
            return _context.Countries.ToList();
        }

        public Country GetCountry(int id)
        {
          return _context.Countries.FirstOrDefault(p => p.Id == id);
        }

        public Country GetCountryByOwner(int OwnerId)
        {
            return _context.Owners.Where(o=>o.Id ==OwnerId).Select(c=>c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFromACountry(int countryId)
        {
           return _context.Owners.Where(o => o.Country.Id == countryId).ToList();
        }

        public bool CountryExists(int id)
        {
           return _context.Countries.Any(p => p.Id == id);
        }

        public bool CreateCountry(Country country)
        {
            _context.Countries.Add(country);
            return Save();
        }

        public bool Save()
        {
            var saved= _context.SaveChanges();
            return saved>0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            _context.Update(country);
            return Save();
        }
    }
}
