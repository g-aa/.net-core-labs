using System;
using System.Collections.Generic;
using System.Text;
using Task3_WebApiCore.BooksDB.Entities.WebEntity;

namespace Task3_WebApiCore.BooksDB.Entities.DbEntity
{
    public class Address
    {
        public int AddressId { get; set; }     
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        public override string ToString() => string.Format("{0}, {1}, {2}", Country, City, Street);

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                switch (obj)
                {
                    case Address address:
                        return Country.Equals(address.Country) && City.Equals(address.City) && Street.Equals(address.Street);
                    case WebAddress webAddress:
                        return Country.Equals(webAddress.Country) && City.Equals(webAddress.City) && Street.Equals(webAddress.Street);
                    default:
                        break;
                }
            }
            return false;
        }
    }
}
