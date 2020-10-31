using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Task2_AspNetCore.WebUI.Models.Data.Enumeration;

namespace Task2_AspNetCore.WebUI.Models.Data.Entities
{
    public class Country
    {
        public int CountryId { get; set; }

        public string Name { get; set; }
        public double Population { get; set; }
        public double Size { get; set; }
        public bool IsG20Member { get; set; }
        public Continent Continent { get; set; }
    }
}
