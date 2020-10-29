using System;
using System.Collections.Generic;
using System.Text;

namespace Task1_EFCore.BooksDB.Entities
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
    }
}
