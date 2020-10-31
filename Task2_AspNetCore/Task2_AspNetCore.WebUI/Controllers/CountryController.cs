using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Task2_AspNetCore.WebUI.Models.Data.Abstraction;
using Task2_AspNetCore.WebUI.Models.Data.Entities;
using Task2_AspNetCore.WebUI.Models.Data.Realization;

namespace Task2_AspNetCore.WebUI.Controllers
{
    public class CountryController : Controller
    {
        private ICountryRepository m_repository;

        public CountryController(CountryRepository repository)
        {
            m_repository = repository;
        }

        public ActionResult GetCountryList()
        {
            return View(m_repository.GetCountrys());
        }

        public ActionResult GetCountryBy(Int32? id)
        {
            if (id == null)
            {
                return this.RedirectToAction(nameof(this.GetCountryList));
            }
            return View(m_repository.GetCountry((Int32)id));
        }

        public ActionResult DeleteCountry(Int32? id)
        {
            if (id != null)
            {
                m_repository.Delete(this.m_repository.GetCountry((Int32)id));
            }
            return this.RedirectToAction(nameof(this.GetCountryList));
        }

        [HttpGet]
        public ActionResult AddNewCountry()
        {
            return View(new Country());
        }

        [HttpPost]
        public ActionResult AddNewCountry(Country country)
        {
            if (country != null)
            {
                m_repository.Add(country);
            }
            return this.RedirectToAction(nameof(this.GetCountryList));
        }

        [HttpGet]
        public ActionResult UpdateCountry(Int32? id)
        {
            if (id == null)
            {
                return this.RedirectToAction(nameof(this.GetCountryList));
            }
            return View(m_repository.GetCountry((Int32)id));
        }

        [HttpPost]
        public ActionResult UpdateCountry(Country country)
        {
            if (country != null)
            {
                m_repository.Update(country);
            }
            return this.RedirectToAction(nameof(this.GetCountryList));
        }

        [HttpGet]
        public ActionResult SearchBy()
        {
            return View(new List<Country>());
        }

        [HttpPost]
        public ActionResult SearchBy(String sortString)
        {
            if (sortString != null)
            {
                // поддерживаемые ключевые слова для поиска (примеры):
                // name:Федер
                // isG20:true
                // isG20:false
                // continent:Eurasia
                List<Country> searchResult = m_repository.SortBy(sortString);
                return View(searchResult);
            }
            return this.RedirectToAction(nameof(this.SearchBy));
        }

        public ActionResult GetStatistics()
        {
            List<KeyValuePair<string, string>> statistic = m_repository.GetStatistic();
            return View(statistic);
        }
    }
}
