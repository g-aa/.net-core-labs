using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Task2_AspNetCore.WebUI.Models.Data.Entities;
using Task2_AspNetCore.WebUI.Models.Data.Enumeration;

namespace Task2_AspNetCore.WebUI.Models.Data.Abstraction
{
    public interface ICountryRepository
    {
        public List<Country> GetCountrys();
        public Country GetCountry(Int32 country_id);
        public Country GetCountry(String name);

        public Boolean Add(Country country);
        public Boolean Update(Country country);
        public Boolean Delete(Country country);

        public Int32 GetG20CountryCount();
        public Int32 GetCountryCount(Continent continent);
        public Country GetCountryMaxSize();
        public Country GetCountryMaxPopulation();

        public List<KeyValuePair<string, string>> GetStatistic();
        public List<Country> SortBy(String sortString);
    }
}
