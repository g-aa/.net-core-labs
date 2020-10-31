using System;
using System.Collections.Generic;
using System.Text;
using Task3_WebApiCore.BooksDB.Entities.DbEntity;

namespace Task3_WebApiCore.BooksDB.Entities.WebEntity
{
    public class WebAddress
    {
        public int AddressId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int PublisherId { get; set; }

        public WebAddress()
        {
            
        }

        public WebAddress(Address address)
        {
            if (address != null)
            {
                this.AddressId = address.AddressId;
                this.Country = address.Country;
                this.City = address.City;
                this.Street = address.Street;
                this.PublisherId = address.PublisherId;
                return;
            }
            throw new ArgumentNullException("входной параметр address равен null");
        }

        public override string ToString()
        {
            return string.Format("id = {0,-3} {1}, {2}, {3}", AddressId, Country, City, Street);
        }

        public string GetFullString()
        {
            return string.Format("-AddressId: {0} -Country: {1} -City: {2} -Street: {3} -PublisherId: {4}", AddressId, Country, City, Street, PublisherId);
        }

        public static string[] GetStringHeaders()
        {
            return new string[] { "AddressId", "Country", "City", "Street", "PublisherId" };
        }

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
