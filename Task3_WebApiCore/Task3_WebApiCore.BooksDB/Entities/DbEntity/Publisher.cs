using System;
using System.Collections.Generic;
using System.Text;
using Task3_WebApiCore.BooksDB.Entities.WebEntity;

namespace Task3_WebApiCore.BooksDB.Entities.DbEntity
{
    public class Publisher
    {
        public int PublisherId { get; set; }
        public string Title { get; set; }

        public ICollection<Address> Addresses { get; set; }
        public ICollection<Book> Books { get; set; }

        public Publisher()
        {
            this.Addresses = new HashSet<Address>();
            this.Books = new HashSet<Book>();
        }

        public override string ToString() => this.Title;

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                switch (obj)
                {
                    case Publisher publisher:
                        return this.Title.Equals(publisher.Title);
                    case WebPublisher webPublisher:
                        return this.Title.Equals(webPublisher.Title);
                    default:
                        break;
                }
            }
            return false;
        }
    }
}
