using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

using Microsoft.EntityFrameworkCore;

using Task3_WebApiCore.BooksDB.Abstraction;
using Task3_WebApiCore.BooksDB.Entities.DbEntity;
using Task3_WebApiCore.BooksDB.Entities.WebEntity;
using Task3_WebApiCore.BooksDB.Enumeration;


namespace Task3_WebApiCore.BooksDB.Realization
{
    public class EFBooksRepository : IBooksRepository
    {
        private readonly BooksEFDbContext context;
        
        public EFBooksRepository(BooksEFDbContext dbContext)
        {
            if (dbContext != null)
            {
                this.context = dbContext;
                this.DefaultInitialization();
                return;
            }
            throw new ArgumentNullException("входной параметр BooksEFDbContext равен null");
        }

        public String Print(Entitie entitie)
        {
            StringBuilder sb = new StringBuilder();
            String format = "{0,-3} {1,-80}";
            switch (entitie)
            {
                case Entitie.None:
                    return this.PrintDataBase();
                case Entitie.Address:
                    sb.AppendLine(String.Format(format, "Id:", "Address:"));
                    foreach (Address a in context.Addresses)
                    {
                        sb.AppendLine(String.Format(format, a.AddressId, a.ToString()));
                    }
                    break;
                case Entitie.Author:
                    sb.AppendLine(String.Format(format, "Id:", "Author:"));
                    foreach (Author a in context.Authors)
                    {
                        sb.AppendLine(String.Format(format, a.AuthorId, a.ToString()));
                    }
                    break;
                case Entitie.Book:
                    sb.AppendLine(String.Format(format, "Id:", "Book:"));
                    foreach (Book b in context.Books)
                    {
                        sb.AppendLine(String.Format(format, b.BookId, b.ToString()));
                    }
                    break;
                case Entitie.Publisher:
                    sb.AppendLine(String.Format(format, "Id:", "Publisher:"));
                    foreach (Publisher p in context.Publishers)
                    {
                        sb.AppendLine(String.Format(format, p.PublisherId, p.ToString()));
                    }
                    break;
            }
            return sb.ToString();
        }

        public Object GetEntitie(Entitie entitie, Int32 entitie_id) 
        {
            switch (entitie)
            {
                case Entitie.Address:
                    Address address = context.Addresses.FirstOrDefault(a => a.AddressId.Equals(entitie_id));
                    if (address != null)
                    {
                        return new WebAddress(address);
                    }
                    break;
                case Entitie.Author:
                    Author author = context.Authors.FirstOrDefault(a => a.AuthorId.Equals(entitie_id));
                    if (author != null)
                    {
                        return new WebAuthor(author);
                    }
                    break;
                case Entitie.Book:
                    Book book = context.Books.FirstOrDefault(b => b.BookId.Equals(entitie_id));
                    if (book != null)
                    {
                        return new WebBook(book);
                    }
                    break;
                case Entitie.Publisher:
                    Publisher publisher = context.Publishers.FirstOrDefault(p => p.PublisherId.Equals(entitie_id));
                    if (publisher != null)
                    {
                        return new WebPublisher(publisher);
                    }
                    break;
                default:
                    throw new ArgumentException("параметр entitie может принимать значение Address, Author, Book, Publisher");
            }
            return new Object();
        }

        public List<Object> GetAllEntities(Entitie entitie) 
        {
            List<Object> result = new List<Object>();
            switch (entitie)
            {
                case Entitie.Address:
                    context.Addresses.ForEachAsync((Address a) => { result.Add(new WebAddress(a)); }).Wait();
                    break;
                case Entitie.Author:
                    context.Authors.ForEachAsync((Author a) => { result.Add(new WebAuthor(a)); }).Wait();
                    break;
                case Entitie.Book:
                    context.Books.ForEachAsync((Book b) => { result.Add(new WebBook(b)); }).Wait();
                    break;
                case Entitie.Publisher:
                    context.Publishers.ForEachAsync((Publisher p) => { result.Add(new WebPublisher(p)); }).Wait();
                    break;
                default:
                    throw new ArgumentException("параметр entitie может принимать значение Address, Author, Book, Publisher");
            }
            return result;
        }

        public Int32 GetEntitieCount(Entitie entitie)
        {
            switch (entitie)
            {
                case Entitie.Author:
                    return context.Authors.Count();
                case Entitie.Book:
                    return context.Books.Count();
                default:
                    throw new ArgumentException("параметр entitie может принимать только значения: Author, Book");
            }
        }

        public Int32 Add(Object obj)
        {
            if (obj != null)
            {
                Int32 return_id = 0;
                switch (obj)
                {
                    case WebAddress webAddress:
                        if (context.Addresses.FirstOrDefault((Address a) => a.Equals(webAddress)) == null)
                        {
                            Address address = new Address()
                            {
                                Country = webAddress.Country,
                                City = webAddress.City,
                                Street = webAddress.Street,
                                PublisherId = webAddress.PublisherId
                            };
                            context.Addresses.Add(address);
                            context.SaveChanges();
                            return_id = context.Addresses.FirstOrDefault((Address a) => a.Equals(webAddress)).AddressId;
                        }
                        break;
                    case WebAuthor webAuthor:
                        if (context.Authors.FirstOrDefault((Author a) => a.Equals(webAuthor)) == null)
                        {
                            Author author = new Author()
                            {
                                FirstName = webAuthor.FirstName,
                                LastName = webAuthor.LastName
                            };
                        
                            List<Book> books = context.Books.Where((Book b) => webAuthor.BooksId.Contains(b.BookId)).ToList();
                            if (books != null)
                            {
                                books.ForEach((Book b) => { author.Books.Add(new AuthorBook { Book = b, Author = author }); });
                            }
                            context.Authors.Add(author);
                            context.SaveChanges();
                            return_id = context.Authors.FirstOrDefault(a => a.Equals(author)).AuthorId;
                        }
                        break;
                    case WebBook webBook:
                        if (context.Books.FirstOrDefault((Book b) => b.Equals(webBook)) == null)
                        {
                            Book book = new Book()
                            {
                                Title = webBook.Title,
                                Year = webBook.Year,
                                Publisher = context.Publishers.FirstOrDefault(p => p.PublisherId == webBook.PublisherId)
                            };
                            List<Author> authors = context.Authors.Where(a => webBook.AuthorsId.Contains(a.AuthorId)).ToList();
                            if (authors != null)
                            {
                                authors.ForEach((Author a) => book.Authors.Add(new AuthorBook { Author = a, Book = book }));  
                            }
                            context.Books.Add(book);
                            context.SaveChanges();
                            return_id = context.Books.FirstOrDefault((Book b) => b.Equals(webBook)).BookId;
                        }
                        break;
                    case WebPublisher webPublisher:
                        if (context.Publishers.FirstOrDefault((Publisher p) => p.Equals(webPublisher)) == null)
                        {
                            Publisher publisher = new Publisher()
                            {
                                Title = webPublisher.Title,
                                Books = context.Books.Where(b => webPublisher.BooksId.Contains(b.BookId)).ToHashSet(),
                                Addresses = context.Addresses.Where(a => webPublisher.AddressesId.Contains(a.PublisherId)).ToHashSet()
                            };
                            context.Publishers.Add(publisher);
                            context.SaveChanges();
                            return_id = context.Publishers.FirstOrDefault((Publisher p) => p.Equals(publisher.Title)).PublisherId;
                        }
                        break;
                    default:
                        throw new ArgumentException("входной парметр obj не является типом: Address, Author, Book, Publisher");
                }
                return return_id;
            }
            throw new ArgumentNullException("входной параметр obj равен null");
        }
        
        public Boolean Remove(Object obj)
        {
            if (obj != null)
            {
                switch (obj)
                {
                    case WebAddress webAddress:
                        Address dbAddress = context.Addresses.FirstOrDefault((Address a) => a.Equals(webAddress));
                        if (dbAddress != null)
                        {
                            context.Remove(dbAddress);
                            context.SaveChanges();
                            return true;
                        }
                        break;
                    case WebAuthor webAuthor:
                        Author dbAuthor = context.Authors.FirstOrDefault((Author a) => a.Equals(webAuthor));
                        if (dbAuthor != null)
                        {
                            context.Remove(dbAuthor);
                            context.SaveChanges();
                            return true;
                        }
                        break;
                    case WebBook webBook:
                        Book dbBook = context.Books.FirstOrDefault((Book b) => b.Equals(webBook));
                        if (dbBook != null)
                        {
                            context.Remove(dbBook);
                            context.SaveChanges();
                            return true;
                        }
                        break;
                    case WebPublisher webPublisher:
                        Publisher dbPublisher = context.Publishers.FirstOrDefault((Publisher p) => p.Equals(webPublisher));
                        if (dbPublisher != null)
                        {
                            context.Remove(dbPublisher);
                            context.SaveChanges();
                            return true;
                        }
                        break;
                    default:
                        throw new ArgumentException("входной парметр obj не является типом: Address, Author, Book, Publisher");
                }
                return false;
            }
            throw new ArgumentNullException("входной параметр obj равен null");
        }
        
        public Int32 Refresh(Object obj)
        {
            if (obj != null)
            {
                Int32 return_id = 0;
                switch (obj)
                {
                    case WebAddress webAddress:
                        Address dbAddress = context.Addresses.FirstOrDefault(a => a.Equals(webAddress));
                        if (dbAddress != null)
                        {
                            dbAddress.Country = webAddress.Country;
                            dbAddress.City = webAddress.City;
                            dbAddress.Street = webAddress.Street;
                            dbAddress.Publisher = context.Publishers.FirstOrDefault(p => p.PublisherId == webAddress.PublisherId);
                            context.SaveChanges();
                            return_id = dbAddress.AddressId;
                        }
                        break;
                    case WebAuthor webAuthor:
                        Author dbAuthor = context.Authors.FirstOrDefault(a => a.Equals(webAuthor));
                        if (dbAuthor != null)
                        {
                            dbAuthor.FirstName = webAuthor.FirstName;
                            dbAuthor.LastName = webAuthor.LastName;

                            var books = context.Books.Where(b => webAuthor.BooksId.Contains(b.BookId));
                            if (books != null)
                            {
                                books.ForEachAsync((Book b) => context.AuthorBooks.Add(new AuthorBook { Author = dbAuthor, Book = b }));
                            }

                            var removeABs = context.AuthorBooks.Where(ab => ab.AuthorId == webAuthor.AuthorId && !webAuthor.BooksId.Contains(ab.BookId));
                            if (removeABs != null)
                            {
                                removeABs.ForEachAsync((AuthorBook r) => context.AuthorBooks.Remove(r));
                            }
                            context.SaveChanges();
                            return_id = dbAuthor.AuthorId;
                        }
                        break;
                    case WebBook webBook:
                        Book dbBook = context.Books.FirstOrDefault(b => b.Equals(webBook));
                        if (dbBook != null)
                        {
                            dbBook.Title = webBook.Title;
                            dbBook.Year = webBook.Year;
                            dbBook.Publisher = context.Publishers.FirstOrDefault(p => p.PublisherId == webBook.PublisherId);

                            var authors = context.Authors.Where(a => webBook.AuthorsId.Contains(a.AuthorId));
                            if (authors != null)
                            {
                                authors.ForEachAsync((Author a) => context.AuthorBooks.Add(new AuthorBook { Author = a, Book = dbBook }));
                            }

                            var removeABs = context.AuthorBooks.Where(ab => ab.BookId == webBook.BookId && !webBook.AuthorsId.Contains(ab.AuthorId));
                            if (removeABs != null)
                            {
                                removeABs.ForEachAsync(r => context.AuthorBooks.Remove(r));
                            }
                            context.SaveChanges();
                            return_id = dbBook.BookId;
                        }
                        break;
                    case WebPublisher webPublisher:
                        Publisher dbPublisher = context.Publishers.FirstOrDefault(p => p.Equals(webPublisher));
                        if (dbPublisher != null)
                        {
                            dbPublisher.Title = webPublisher.Title;
                            dbPublisher.Addresses = context.Addresses.Where(a => webPublisher.AddressesId.Contains(a.PublisherId)).ToHashSet();
                            dbPublisher.Books = context.Books.Where(b => webPublisher.BooksId.Contains(b.BookId)).ToHashSet();
                            context.SaveChanges();
                            return_id = dbPublisher.PublisherId;
                        }
                        break;
                    default:
                        throw new ArgumentException("входной парметр obj не является типом: Address, Author, Book, Publisher");
                }
                return return_id;
            }
            throw new ArgumentNullException("входной параметр obj равен null");
        }

        public IEnumerable<KeyValuePair<string, string>> GetStatistics()
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            int bCount = context.Books.Count();
            int aCount = context.Authors.Count();
            int pCount = context.Publishers.Count();

            result.Add(new KeyValuePair<string, string>("Число книг в коллекции:", bCount.ToString()));
            result.Add(new KeyValuePair<string, string>("Число авторов в коллекции:", aCount.ToString()));

            if (aCount != 0)
            {
                result.Add(new KeyValuePair<string, string>("Среднее число книг на одного автора:", (bCount / aCount).ToString()));
            }
            else
            {
                result.Add(new KeyValuePair<string, string>("Среднее число книг на одного автора:", "нет данных!"));
            }

            if (pCount != 0)
            {
                result.Add(new KeyValuePair<string, string>("Среднее число книг на одно издательство:", (bCount / pCount).ToString()));
            }
            else
            {
                result.Add(new KeyValuePair<string, string>("Среднее число книг на одно издательство:", "нет данных!"));
            }

            var author = context.Authors.Include(a => a.Books).OrderBy(a => -a.Books.Count).FirstOrDefault();
            if (author != null)
            {
                result.Add(new KeyValuePair<string, string>("Часто встречаемый автор:", author.ToString()));
            }
            else
            {
                result.Add(new KeyValuePair<string, string>("Часто встречаемый автор:", "нет данных!"));
            }
            return result;
        }

        private String PrintDataBase()
        {
            var q = from b in context.Books
                    join ab in context.AuthorBooks on b.BookId equals ab.BookId into abe
                    from ab in abe.DefaultIfEmpty()
                    join a in context.Authors on ab.AuthorId equals a.AuthorId into ae
                    from a in ae.DefaultIfEmpty()
                    join p in context.Publishers on b.PublisherId equals p.PublisherId into pe
                    from p in pe.DefaultIfEmpty()
                    join ad in context.Addresses on p.PublisherId equals ad.PublisherId into ade
                    from ad in ade.DefaultIfEmpty()
                    select new
                    {
                        FullName = a != null ? a.ToString() : "---",
                        BookTitle = b.ToString(),
                        PublisherTitle = p != null ? p.ToString() : "---",
                        PublisherAddresses = ad != null ? ad.ToString() : "---"
                    };

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("{0,-80} {1,-80} {2,-80} {3,-80}", "Author:", "Book:", "Publisher:", "Address:"));
            foreach (var e in q)
            {
                sb.AppendLine(String.Format("{0,-80} {1,-80} {2,-80} {3,-80}", e.FullName, e.BookTitle, e.PublisherTitle, e.PublisherAddresses));
            }
            return sb.ToString();
        }

        private void DefaultInitialization()
        {
            Publisher williams = new Publisher { Title = "И.Д. ВИЛЬЯМС" };
            Address wAddress = new Address { Country = "РФ", City = "г.Москва", Street = "ул.Лесная, д.43, к.1", Publisher = williams };
            context.Publishers.Add(williams);
            context.Addresses.Add(wAddress);


            Publisher piter = new Publisher { Title = "И.Д. ПИТЕР" };
            Address pAddress = new Address { Country = "РФ", City = "г.Санкт-Петербург", Street = "пр.Б.Сампсониевский, 29а", Publisher = piter };
            context.Publishers.Add(piter);
            context.Addresses.Add(pAddress);


            Author author_1 = new Author { FirstName = "Джеффри", LastName = "Рихтер" };
            Book book_1 = new Book { Title = "CLR via C# - Программирование на .NET Framework 4.0. изд. 3", Year = 2012, Publisher = piter };
            context.Authors.Add(author_1);
            context.Books.Add(book_1);
            context.AuthorBooks.Add(new AuthorBook { Author = author_1, Book = book_1 });


            Author author_2 = new Author { FirstName = "Чарльз", LastName = "Петцольд" };
            Book book_2 = new Book { Title = "Microsoft Windows Presentation Foundation. Базовый курс", Year = 2008, Publisher = piter };

            context.Authors.Add(author_2);
            context.Books.Add(book_2);
            context.AuthorBooks.Add(new AuthorBook { Author = author_2, Book = book_2 });

            Book book_3 = new Book { Title = "Программирование с использованием MS WindowsForms", Year = 2006, Publisher = piter };
            context.Books.Add(book_3);
            context.AuthorBooks.Add(new AuthorBook { Author = author_2, Book = book_3 });


            Author author_4 = new Author { FirstName = "Мэтью", LastName = "Мак-Дональд" };
            Book book_4 = new Book { Title = "WPF в .NET 3.5 с примерами на C#", Year = 2008, Publisher = williams };

            context.Authors.Add(author_4);
            context.Books.Add(book_4);
            context.AuthorBooks.Add(new AuthorBook { Author = author_4, Book = book_4 });


            Author author_5 = new Author { FirstName = "Марк", LastName = "Прайс" };
            Book book_5 = new Book { Title = "C#7.0 & .NET-Core. Кросс-платформенная разработка для профессионалов", Year = 2018, Publisher = piter };

            context.Authors.Add(author_5);
            context.Books.Add(book_5);
            context.AuthorBooks.Add(new AuthorBook { Author = author_5, Book = book_5 });


            Author author_6 = new Author { FirstName = "Джозеф", LastName = "Албахари" };
            Author author_7 = new Author { FirstName = "Бэн", LastName = "Албахари" };
            Book book_6 = new Book { Title = "Карманный справочник C# 7.0", Year = 2018, Publisher = williams };

            context.Authors.Add(author_6);
            context.Authors.Add(author_7);
            context.Books.Add(book_6);
            context.AuthorBooks.Add(new AuthorBook { Author = author_6, Book = book_6 });
            context.AuthorBooks.Add(new AuthorBook { Author = author_7, Book = book_6 });

            context.SaveChanges();
        }
    }
}
