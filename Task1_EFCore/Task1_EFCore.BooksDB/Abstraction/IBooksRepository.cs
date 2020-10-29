using System;
using System.Collections.Generic;
using System.Text;

using Task1_EFCore.BooksDB.Entities;
using Task1_EFCore.BooksDB.Enumeration;

namespace Task1_EFCore.BooksDB.Abstraction
{
    public interface IBooksRepository
    {
        public String Print(Entitie entitie);
        public Boolean Add(Object obj);
        public Boolean Remove(Object obj);
        public Boolean Refresh(Object obj);

        public Address GetAddress(Int32 addres_id);
        public Publisher GetPublisher(Int32 publisher_id);

        public Author GetAuthor(Int32 author_id);
        public Author GetAuthor(String firstName, String lastName);

        public List<Book> GetAuthorBooks(Int32 author_id);
        public List<Book> GetAuthorBooks(String firstName, String lastName);

        public Book GetBook(Int32 book_id);
        public Book GetBook(String title);

        public Int32 GetEntitieCount(Entitie entitie);

        public List<String> GetBooksTitle();
        public List<String> GetAuthorsName();

        public Double GetAvgNumberOfBooksPerAuthor();
        public Double GetAvgNumberOfBooksPerPublishers();
        public Author GetAuthorWiththeManyBooks();
    }
}
