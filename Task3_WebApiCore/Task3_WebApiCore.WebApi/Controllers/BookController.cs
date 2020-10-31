using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Task3_WebApiCore.BooksDB.Abstraction;
using Task3_WebApiCore.BooksDB.Entities.WebEntity;
using Task3_WebApiCore.BooksDB.Enumeration;

namespace Task3_WebApiCore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBooksRepository m_repository;

        public BookController(IBooksRepository repository)
        {
            if (repository != null)
            {
                this.m_repository = repository;
                return;
            }
            throw new ArgumentNullException("входной параметр repositiry равен null!");
        }

        // GET: api/Book
        [HttpGet]
        public ActionResult<IEnumerable<WebBook>> GetAllBooks()
        {
            string log = string.Format("{0} - [GET REQUEST] : {1}", DateTime.Now.ToString(), "all books");
            Console.WriteLine(log);
            List<Object> objs = this.m_repository.GetAllEntities(Entitie.Book);
            List<WebBook> result = new List<WebBook>(objs.Count);
            objs.ForEach((Object o) => result.Add((WebBook)o));
            return new ActionResult<IEnumerable<WebBook>>(result);
        }

        // GET: api/Book/5
        [HttpGet("{id}")]
        public ActionResult<WebBook> GetBook(Int32? id)
        {
            if (id != null)
            {
                WebBook b = (WebBook)this.m_repository.GetEntitie(Entitie.Book, (Int32)id);
                string log = string.Format("{0} - [GET REQUEST] : id = {1}, title = {2}", DateTime.Now.ToString(), id, b?.Title);
                Console.WriteLine(log);
                if (b != null)
                {
                    return new ActionResult<WebBook>(b);
                }
                return this.NotFound();
            }
            return this.ValidationProblem();
        }

        // POST: api/Book
        [HttpPost]
        public ActionResult<Int32> Post([FromBody] WebBook book)
        {
            if (book != null)
            {
                Int32 id = m_repository.Add(book);
                string log = string.Format("{0} - [POST REQUEST] : id = {1}, title = {2}", DateTime.Now.ToString(), id, book?.Title);
                Console.WriteLine(log);
                return new ActionResult<Int32>(id);
            }
            return this.ValidationProblem();
        }

        // PUT: api/Book/5
        [HttpPut("{id}")]
        public ActionResult<Int32> Put(Int32? id, [FromBody] WebBook book)
        {
            if (id != null && book != null)
            {
                string log = string.Format("{0} - [PUT REQUEST] : id = {1}, title = {2}", DateTime.Now.ToString(), id, book?.Title);
                Console.WriteLine(log);
                if (book.BookId.Equals(id))
                {
                    return new ActionResult<Int32>(this.m_repository.Refresh(book));
                }
                return NotFound();
            }
            return this.ValidationProblem();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult<Boolean> Delete(Int32? id)
        {
            if (id != null)
            {
                string log = string.Format("{0} - [DELETE REQUEST] : id = {1}, {2}", DateTime.Now.ToString(), id, "delete book");
                Console.WriteLine(log);
                return new ActionResult<Boolean>(this.m_repository.Remove(this.m_repository.GetEntitie(Entitie.Book, (Int32)id)));
            }
            return this.ValidationProblem();
        }
    }
}
