using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task3_WebApiCore.BooksDB.Entities.DbEntity;

namespace Task3_WebApiCore.BooksDB.Entities.WebEntity
{
    public class WebPublisher
    {
        public int PublisherId { get; set; }
        public string Title { get; set; }
        public IEnumerable<int> BooksId { get; set; }
        public IEnumerable<int> AddressesId { get; set; }

        public WebPublisher()
        {
            AddressesId = new List<int>();
            BooksId = new List<int>();
        }

        public WebPublisher(Publisher publisher)
        {
            if (publisher != null)
            {
                this.PublisherId = publisher.PublisherId;
                this.Title = publisher.Title;
                this.BooksId = publisher.Books.Select((Book b) => b.BookId).ToList();
                this.AddressesId = publisher.Addresses.Select((Address a) => a.AddressId).ToList();
                return;
            }
            throw new ArgumentNullException("входной параметр publisher равен null");
        }

        public override string ToString()
        {
            return string.Format("id = {0,-3} {1}", PublisherId, Title);
        }

        public string GetFullString()
        {
            return string.Format("-PublisherId: {0} -Title: {1} -BooksId: {2} -AddressesId: {3}", PublisherId, Title, String.Join(",", BooksId), String.Join(",", AddressesId));
        }

        public static string[] GetStringHeaders()
        {
            return new string[] { "PublisherId", "Title", "BooksId", "AddressesId" };
        }

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
