using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Task2_AspNetCore.WebUI.Models.Data.Abstraction;
using Task2_AspNetCore.WebUI.Models.Data.Entities;
using Task2_AspNetCore.WebUI.Models.Data.Enumeration;

namespace Task2_AspNetCore.WebUI.Models.Data.Realization
{
    public class CountryRepository : ICountryRepository
    {
        private List<Country> m_countries;
        private Int32 m_maxId;

        public CountryRepository()
        {
            this.m_countries = CountryRepository.Initialize();
            this.m_maxId = m_countries.Max(c => c.CountryId);
        }

        public List<Country> GetCountrys()
        {
            return new List<Country>(m_countries);
        }

        public Country GetCountry(Int32 country_id)
        {
            return m_countries.FirstOrDefault(c => c.CountryId == country_id);
        }

        public Country GetCountry(String name)
        {
            if (name != null)
            {
                return m_countries.FirstOrDefault(c => c.Name == name);
            }
            throw new ArgumentNullException("входной парамтр name равен null");
        }

        public Boolean Add(Country country)
        {
            if (country != null)
            {
                if (m_countries.FirstOrDefault(c => c.Name == country.Name) == null)
                {
                    country.CountryId = m_countries.Count != 0 ? this.m_maxId + 1 : 1;
                    m_countries.Add(country);
                    return true;
                }
                return false;
            }
            throw new ArgumentNullException("входной парамтр country равен null");
        }

        public Boolean Update(Country country)
        {
            if (country != null)
            {
                Int32 idx = m_countries.FindIndex(c => c.CountryId == country.CountryId);
                if (idx < 0)
                {
                    return false;
                }
                m_countries[idx] = country;
                return true;
            }
            throw new ArgumentNullException("входной парамтр country равен null");
        }

        public Boolean Delete(Country country)
        {
            if (country != null)
            {
                Country dbCountry = this.m_countries.FirstOrDefault(c => c.CountryId.Equals(country.CountryId));
                if (dbCountry != null)
                {
                    this.m_countries.Remove(dbCountry);
                    return true;
                }
                return false;
            }
            throw new ArgumentNullException("входной парамтр country равен null");
        }

        public Int32 GetG20CountryCount()
        {
            return m_countries.Count(c => c.IsG20Member);
        }

        public Int32 GetCountryCount(Continent continent)
        {
            if (continent != Continent.None)
            {
                return m_countries.Count(c => c.Continent == continent);
            }
            return m_countries.Count();
        }

        public Country GetCountryMaxSize()
        {
            return m_countries.FirstOrDefault(c => c.Size == m_countries.Max(sc => sc.Size));
        }

        public Country GetCountryMaxPopulation()
        {
            return m_countries.FirstOrDefault(c => c.Population == m_countries.Max(sc => sc.Population));
        }

        private static List<Country> Initialize()
        {
            List<Country> repository = new List<Country>();

            repository.Add(new Country
            {
                CountryId = 1,
                Name = "Российская Федерация",
                Population = 142856536,
                Size = 17125191,
                IsG20Member = true,
                Continent = Continent.Eurasia
            });

            repository.Add(new Country
            {
                CountryId = 2,
                Name = "Китайская Народная Республика",
                Population = 1404328611,
                Size = 9598962,
                IsG20Member = true,
                Continent = Continent.Eurasia
            });

            repository.Add(new Country
            {
                CountryId = 3,
                Name = "Республика Корея",
                Population = 51732586,
                Size = 100210,
                IsG20Member = true,
                Continent = Continent.Eurasia
            });

            repository.Add(new Country
            {
                CountryId = 4,
                Name = "Корейская Народно-Демократическая Республика",
                Population = 25713726,
                Size = 120540,
                IsG20Member = false,
                Continent = Continent.Eurasia
            });

            repository.Add(new Country
            {
                CountryId = 5,
                Name = "Государство Япония",
                Population = 126225000,
                Size = 377944,
                IsG20Member = true,
                Continent = Continent.Eurasia
            });

            repository.Add(new Country
            {
                CountryId = 6,
                Name = "Соединённые Штаты Америки",
                Population = 328915700,
                Size = 9519431,
                IsG20Member = true,
                Continent = Continent.NorthAmerica
            });

            repository.Add(new Country
            {
                CountryId = 7,
                Name = "Федеративная Республика Бразилия",
                Population = 207353391,
                Size = 8515767,
                IsG20Member = true,
                Continent = Continent.SouthAmerica
            });

            repository.Add(new Country
            {
                CountryId = 8,
                Name = "Южно-Африканская Республика",
                Population = 54956900,
                Size = 1219912,
                IsG20Member = true,
                Continent = Continent.Africa
            });

            return repository;
        }

        public List<KeyValuePair<string, string>> GetStatistic()
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            result.Add(new KeyValuePair<string, string>("Количество стран в базе:", this.GetCountryCount(Continent.None).ToString()));
            result.Add(new KeyValuePair<string, string>("Количество стран в Африке:", this.GetCountryCount(Continent.Africa).ToString()));
            result.Add(new KeyValuePair<string, string>("Количество стран в Антарктике:", this.GetCountryCount(Continent.Antarctica).ToString()));
            result.Add(new KeyValuePair<string, string>("Количество стран в Австралии:", this.GetCountryCount(Continent.Australia).ToString()));
            result.Add(new KeyValuePair<string, string>("Количество стран в Евразии:", this.GetCountryCount(Continent.Eurasia).ToString()));
            result.Add(new KeyValuePair<string, string>("Количество стран в Северной Америке:", this.GetCountryCount(Continent.NorthAmerica).ToString()));
            result.Add(new KeyValuePair<string, string>("Количество стран в Южной Америке:", this.GetCountryCount(Continent.SouthAmerica).ToString()));
            result.Add(new KeyValuePair<string, string>("Количество участников G20:", this.GetG20CountryCount().ToString()));

            var cMaxSize = this.GetCountryMaxSize();
            if (cMaxSize != null)
            {
                result.Add(new KeyValuePair<string, string>("Страна с максимальной площадью:", string.Format("{0} ({1} км^2)", cMaxSize.Name, cMaxSize.Size)));
            }
            else
            {
                result.Add(new KeyValuePair<string, string>("Страна с максимальной площадью:", "нет данных!"));
            }

            var cMaxPopulation = this.GetCountryMaxPopulation();
            if (cMaxPopulation != null)
            {
                result.Add(new KeyValuePair<string, string>("Страна с максимальным населением:", string.Format("{0} ({1} чел.)", cMaxPopulation.Name, cMaxPopulation.Population)));
            }
            else
            {
                result.Add(new KeyValuePair<string, string>("Страна с максимальным населением:", "нет данных!"));
            }

            return result;
        }

        // name:
        // isG20:
        // continent:
        public List<Country> SortBy(String sortString)
        {
            if (sortString != null)
            {
                string[] sh = sortString.Trim().Split(':');

                if (sh.Length == 2)
                {
                    if ("name".Equals(sh[0]))
                    {
                        return m_countries.Where(c => c.Name.Contains(sh[1])).ToList();
                    }
                    else if ("isG20".Equals(sh[0]))
                    {
                        if (bool.TryParse(sh[1], out bool tf))
                        {
                            return m_countries.Where(c => c.IsG20Member == tf).ToList();
                        }
                    }
                    else if ("continent".Equals(sh[0]))
                    {
                        if (Enum.TryParse(typeof(Continent), sh[1], out object cObj))
                        {
                            var continet = (Continent)cObj;
                            return m_countries.Where(c => c.Continent == continet).ToList();
                        }
                    }
                }
            }
            return new List<Country>();
        }
    }
}
