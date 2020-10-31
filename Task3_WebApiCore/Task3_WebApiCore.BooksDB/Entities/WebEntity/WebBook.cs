using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task3_WebApiCore.BooksDB.Entities.DbEntity;

namespace Task3_WebApiCore.BooksDB.Entities.WebEntity
{
    public class WebBook
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public IEnumerable<int> AuthorsId { get; set; }
        public int PublisherId { get; set; }

        public WebBook()
        {
            this.AuthorsId = new List<int>();
        }

        public WebBook(Book book)
        {
            if(book != null)
            {
                this.BookId = book.BookId;
                this.Title = book.Title;
                this.Year = book.Year;
                this.PublisherId = book.PublisherId;
                this.AuthorsId = book.Authors.Select(a => a.AuthorId).ToList();
                return;
            }
            throw new ArgumentNullException("входной параметр book равен null");
        }

        public override string ToString()
        {
            return string.Format("id = {0,-3} {1} - {2}", BookId, Title, Year);
        }

        public string GetFullString()
        {
            return string.Format("-BookId: {0} -Title: {1} -Year: {2} -AuthorsId: {3} -PublisherId: {4}", BookId, Title, Year, String.Join(",", AuthorsId), PublisherId);
        }

        public static string[] GetStringHeaders()
        {
            return new string[] { "BookId", "Title", "Year", "AuthorsId", "PublisherId" };
        }

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                switch (obj)
                {
                    case Book book:
                        return this.Title.Equals(book.Title) && this.Year.Equals(book.Year);
                    case WebBook webBook:
                        return this.Title.Equals(webBook.Title) && this.Year.Equals(webBook.Year);
                    default:
                        break;
                }
            }
            return false;
        }
    }
}
