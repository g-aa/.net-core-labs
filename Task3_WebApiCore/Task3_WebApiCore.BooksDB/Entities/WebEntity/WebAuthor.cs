using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task3_WebApiCore.BooksDB.Entities.DbEntity;

namespace Task3_WebApiCore.BooksDB.Entities.WebEntity
{
    public class WebAuthor
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<int> BooksId { get; set; }

        public WebAuthor()
        {
            this.BooksId = new List<int>();
        }

        public WebAuthor(Author author)
        {
            if (author != null)
            {
                this.AuthorId = author.AuthorId;
                this.FirstName = author.FirstName;
                this.LastName = author.LastName;
                this.BooksId = author.Books.Select((AuthorBook b) => b.BookId).ToList();
                return;
            }
            throw new ArgumentNullException("входной параметр author равен null");
        }

        public override string ToString()
        {
            return string.Format("id = {0,-3} {1} {2}", AuthorId, FirstName, LastName);
        }

        public string GetFullString()
        {
            return string.Format("-AutorId: {0} -FirstName: {1} -LastName: {2} -BookId: {3}", AuthorId, FirstName, LastName, String.Join(",", BooksId));
        }

        public static string[] GetStringHeaders()
        {
            return new string[] { "AutorId", "FirstName", "LastName", "BookId" };
        }

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                switch (obj)
                {
                    case Author author:
                        return this.FirstName.Equals(author.FirstName) && this.LastName.Equals(author.LastName);
                    case WebAuthor webAuthor:
                        return this.FirstName.Equals(webAuthor.FirstName) && this.LastName.Equals(webAuthor.LastName);
                    default:
                        break;
                }
            }
            return false;
        }
    }
}
