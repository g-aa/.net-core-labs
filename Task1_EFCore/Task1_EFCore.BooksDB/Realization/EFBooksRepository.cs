using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

using Microsoft.EntityFrameworkCore;

using Task1_EFCore.BooksDB.Abstraction;
using Task1_EFCore.BooksDB.Entities;
using Task1_EFCore.BooksDB.Enumeration;

namespace Task1_EFCore.BooksDB.Realization
{
    public class EFBooksRepository : IBooksRepository
    {
        private readonly BooksEFDbContext context ;
        
        private readonly static EFBooksRepository efBooks = new EFBooksRepository();

        private EFBooksRepository()
        {
            context = new BooksEFDbContext();

            this.DefaultInitialization();
        }

        public static EFBooksRepository GetInstance()
        {
            return efBooks;
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

        public Boolean Add(Object obj)
        {
            if (obj != null)
            {
                switch (obj)
                {
                    case Address address:
                        if (context.Addresses.FirstOrDefault((Address a) => a.Country.Equals(address.Country) && a.City.Equals(address.City) && a.Street.Equals(address.Street)) == null)
                        {
                            context.Addresses.Add(address);
                            context.SaveChanges();
                            return true;
                        }
                        break;
                    case Author author:
                        if (context.Authors.FirstOrDefault((Author a) => a.FirstName.Equals(author.FirstName) && a.LastName.Equals(author.LastName)) == null)
                        {
                            context.Authors.Add(author);
                            context.SaveChanges();
                            return true;
                        }
                        break;
                    case Book book:
                        if (context.Books.FirstOrDefault((Book b) => b.BookTitle.Equals(book.BookTitle) && b.Year.Equals(book.Year)) == null)
                        {
                            context.Books.Add(book);
                            context.SaveChanges();
                            return true;
                        }
                        break;
                    case Publisher publisher:
                        if (context.Publishers.FirstOrDefault((Publisher p) => p.Title.Equals(publisher.Title)) == null)
                        {
                            context.Publishers.Add(publisher);
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
        
        public Boolean Remove(Object obj)
        {
            if (obj != null)
            {
                switch (obj)
                {
                    case Address address:
                        Address dbAddress = context.Addresses.FirstOrDefault(
                            (Address a) => a.City.Equals(address.City) && a.Street.Equals(address.Street) ||
                            a.AddressId.Equals(address.AddressId));
                        if (dbAddress != null)
                        {
                            context.Remove(dbAddress);
                            context.SaveChanges();
                            return true;
                        }
                        break;
                    case Author author:
                        Author dbAuthor = context.Authors.FirstOrDefault(
                            (Author a) => a.FirstName.Equals(author.FirstName) && a.LastName.Equals(author.LastName) ||
                            a.AuthorId.Equals(author.AuthorId));
                        if (dbAuthor != null)
                        {
                            context.Remove(author);
                            context.SaveChanges();
                            return true;
                        }
                        break;
                    case Book book:
                        Book dbBook = context.Books.FirstOrDefault(
                            (Book b) => b.BookTitle.Equals(book.BookTitle) || b.BookId.Equals(book.BookId));
                        if (dbBook != null)
                        {
                            context.Remove(book);
                            context.SaveChanges();
                            return true;
                        }
                        break;
                    case Publisher publisher:
                        Publisher dbPublisher = context.Publishers.FirstOrDefault(
                            (Publisher p) => p.Title.Equals(publisher.Title) || p.PublisherId.Equals(publisher.PublisherId));
                        if (dbPublisher != null)
                        {
                            context.Remove(publisher);
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
        
        public Boolean Refresh(Object obj)
        {
            if (obj != null)
            {
                switch (obj)
                {
                    case Address address:
                        Address dbAddress = context.Addresses.FirstOrDefault(a => a.AddressId.Equals(address.AddressId));
                        if (dbAddress != null)
                        {
                            if (!address.Country.Equals(String.Empty))
                            {
                                dbAddress.Country = address.Country;
                            }
                            if (!address.City.Equals(String.Empty))
                            {
                                dbAddress.City = address.City;
                            }
                            if (!address.Street.Equals(String.Empty))
                            {
                                dbAddress.Street = address.Street;
                            }
                            dbAddress.Publisher = address.Publisher;
                            context.SaveChanges();
                            return true;
                        }
                        break;
                    case Author author:
                        Author dbAuthor = context.Authors.FirstOrDefault(a => a.AuthorId.Equals(author.AuthorId));
                        if (dbAuthor != null)
                        {
                            if (!author.FirstName.Equals(String.Empty))
                            {
                                dbAuthor.FirstName = author.FirstName;
                            }
                            if (!author.LastName.Equals(String.Empty))
                            {
                                dbAuthor.LastName = author.LastName;
                            }
                            dbAuthor.Books = author.Books;
                            context.SaveChanges();
                            return true;
                        }
                        break;
                    case Book book:
                        Book dbBook = context.Books.FirstOrDefault(b => b.BookId.Equals(book.BookId));
                        if (dbBook != null)
                        {
                            if (!book.BookTitle.Equals(String.Empty))
                            {
                                dbBook.BookTitle = book.BookTitle;
                            }
                            dbBook.Year = book.Year;
                            dbBook.Authors = book.Authors;
                            dbBook.Publisher = book.Publisher;
                            context.SaveChanges();
                            return true;
                        }
                        break;
                    case Publisher publisher:
                        Publisher dbPublisher = context.Publishers.FirstOrDefault(p => p.PublisherId.Equals(publisher.PublisherId));
                        if (dbPublisher != null)
                        {
                            if (!publisher.Title.Equals(String.Empty))
                            {
                                dbPublisher.Title = publisher.Title;
                            }
                            dbPublisher.Addresses = publisher.Addresses;
                            dbPublisher.Books = publisher.Books;
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

        public Address GetAddress(Int32 addres_id)
        {
            Address address = context.Addresses.FirstOrDefault(a => a.AddressId.Equals(addres_id));
            return address != null ? address : new Address();
        }

        public Address GetAddress(String city, String street)
        {
            if (city != null && street != null)
            {
                Address address = context.Addresses.FirstOrDefault(a => a.City.Equals(city) && a.Street.Equals(street));
                return address != null ? address : new Address();
            }
            throw new ArgumentNullException("входной параметр city или street равен null");
        }

        public Author GetAuthor(Int32 author_id)
        {
            Author author = context.Authors.FirstOrDefault(a => a.AuthorId.Equals(author_id));
            return author != null ? author : new Author();
        }

        public Author GetAuthor(String firstName, String lastName)
        {
            if (firstName != null && lastName != null)
            {
                Author result = context.Authors.Where(a => a.LastName == lastName && a.FirstName == firstName).FirstOrDefault();
                return result != null ? result : new Author();
            }
            throw new ArgumentNullException("входной параметр firstName или lastName равен null");
        }

        public List<Book> GetAuthorBooks(Int32 author_id)
        {
            return (from a in context.Authors
                    where a.AuthorId == author_id
                    join ab in context.AuthorBooks on a.AuthorId equals ab.AuthorId
                    join b in context.Books on ab.BookId equals b.BookId
                    select b).ToList();
        }

        public List<Book> GetAuthorBooks(String firstName, String lastName)
        {
            if (firstName != null && lastName != null)
            {
                return (from a in context.Authors
                        where a.LastName == lastName && a.FirstName == firstName
                        join ab in context.AuthorBooks on a.AuthorId equals ab.AuthorId
                        join b in context.Books on ab.BookId equals b.BookId
                        select b).ToList();
            }
            throw new ArgumentNullException("входной параметр firstName или lastName равен null");
        }

        public Book GetBook(Int32 book_id)
        {
            Book book = context.Books.FirstOrDefault(b => b.BookId == book_id);
            return book != null ? book : new Book();
        }

        public Book GetBook(String title)
        {
            if (title != null)
            {
                Book book = context.Books.Where(b => b.BookTitle == title).FirstOrDefault();
                return book != null ? book : new Book();
            }
            throw new ArgumentNullException("входной параметр title равен null");
        }

        public Publisher GetPublisher(Int32 publisher_id)
        {
            Publisher publisher = context.Publishers.FirstOrDefault(p => p.PublisherId == publisher_id);
            return publisher != null ? publisher : new Publisher();
        }

        public Publisher GetPublisher(String title)
        {
            if (title != null)
            {
                Publisher publisher = context.Publishers.FirstOrDefault(p => p.Title.Equals(title));
                return publisher != null ? publisher : new Publisher();
            }
            throw new ArgumentNullException("входной параметр title равен null");
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

        public List<String> GetAuthorsName()
        {
            return context.Authors.Select(a => a.ToString()).ToList();
        }

        public List<String> GetBooksTitle()
        {
            return context.Books.Select(b => b.ToString()).ToList();
        }

        public Author GetAuthorWiththeManyBooks()
        {
            return context.Authors.Include(a => a.Books).OrderBy(a => -a.Books.Count).FirstOrDefault();
        }

        public Double GetAvgNumberOfBooksPerAuthor()
        {
            return context.Books.Count() / context.Authors.Count();
        }

        public Double GetAvgNumberOfBooksPerPublishers()
        {
            return context.Books.Count() / context.Publishers.Count();
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
            Book book_1 = new Book { BookTitle = "CLR via C# - Программирование на .NET Framework 4.0. изд. 3", Year = 2012, Publisher = piter };
            context.Authors.Add(author_1);
            context.Books.Add(book_1);
            context.AuthorBooks.Add(new AuthorBook { Author = author_1, Book = book_1 });


            Author author_2 = new Author { FirstName = "Чарльз", LastName = "Петцольд" };
            Book book_2 = new Book { BookTitle = "Microsoft Windows Presentation Foundation. Базовый курс", Year = 2008, Publisher = piter };

            context.Authors.Add(author_2);
            context.Books.Add(book_2);
            context.AuthorBooks.Add(new AuthorBook { Author = author_2, Book = book_2 });

            Book book_3 = new Book { BookTitle = "Программирование с использованием MS WindowsForms", Year = 2006, Publisher = piter };
            context.Books.Add(book_3);
            context.AuthorBooks.Add(new AuthorBook { Author = author_2, Book = book_3 });


            Author author_4 = new Author { FirstName = "Мэтью", LastName = "Мак-Дональд" };
            Book book_4 = new Book { BookTitle = "WPF в .NET 3.5 с примерами на C#", Year = 2008, Publisher = williams };

            context.Authors.Add(author_4);
            context.Books.Add(book_4);
            context.AuthorBooks.Add(new AuthorBook { Author = author_4, Book = book_4 });


            Author author_5 = new Author { FirstName = "Марк", LastName = "Прайс" };
            Book book_5 = new Book { BookTitle = "C#7.0 & .NET-Core. Кросс-платформенная разработка для профессионалов", Year = 2018, Publisher = piter };

            context.Authors.Add(author_5);
            context.Books.Add(book_5);
            context.AuthorBooks.Add(new AuthorBook { Author = author_5, Book = book_5 });


            Author author_6 = new Author { FirstName = "Джозеф", LastName = "Албахари" };
            Author author_7 = new Author { FirstName = "Бэн", LastName = "Албахари" };
            Book book_6 = new Book { BookTitle = "Карманный справочник C# 7.0", Year = 2018, Publisher = williams };

            context.Authors.Add(author_6);
            context.Authors.Add(author_7);
            context.Books.Add(book_6);
            context.AuthorBooks.Add(new AuthorBook { Author = author_6, Book = book_6 });
            context.AuthorBooks.Add(new AuthorBook { Author = author_7, Book = book_6 });

            context.SaveChanges();
        }
    }
}
