using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokemonReview.Migrations;
using PokemonReview.Models;

namespace PokemonReview.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int id);
        Country GetCountryByOwner(int ownerId);
        //ICollection<Owner> GetOwnersFromACountry(int countryId);
        bool CountryExists(int countryId);
        bool CreateCountry(Country country);
        bool Save();
        
    }
}