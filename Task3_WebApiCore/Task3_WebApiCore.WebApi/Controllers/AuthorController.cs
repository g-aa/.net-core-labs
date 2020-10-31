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
    public class AuthorController : ControllerBase
    {
        private readonly IBooksRepository m_repository;

        public AuthorController(IBooksRepository repository)
        {
            if (repository != null)
            {
                this.m_repository = repository;
                return;
            }
            throw new ArgumentNullException("входной параметр repositiry равен null");
        }

        // GET: api/Author
        [HttpGet]
        public ActionResult<IEnumerable<WebAuthor>> GetAllAuthors()
        {
            string log = string.Format("{0} - [GET REQUEST] : {1}", DateTime.Now.ToString(), "all authors");
            Console.WriteLine(log);
            
            List<Object> objs = this.m_repository.GetAllEntities(Entitie.Author);
            List<WebAuthor> result = new List<WebAuthor>(objs.Count);
            objs.ForEach((Object o) => result.Add((WebAuthor)o));
            return new ActionResult<IEnumerable<WebAuthor>>(result);
        }

        // GET: api/Author/5
        [HttpGet("{id}")]
        public ActionResult<WebAuthor> GetAuthor(Int32? id)
        {
            if (id != null)
            {
                WebAuthor a = (WebAuthor)this.m_repository.GetEntitie(Entitie.Author, (Int32)id);
                string log = string.Format("{0} - [GET REQUEST] : id = {1}, name = {2} {3}", DateTime.Now.ToString(), id, a?.FirstName, a?.LastName);
                Console.WriteLine(log);
                if (a != null)
                {
                    return new ActionResult<WebAuthor>(a);
                }
                return this.NotFound();
            }
            return this.ValidationProblem();
        }

        // POST: api/Author
        [HttpPost]
        public ActionResult<Int32> Post([FromBody] WebAuthor author)
        {
            if (author != null)
            {
                Int32 id = m_repository.Add(author);
                string log = string.Format("{0} - [POST REQUEST] : id = {1}, name = {2} {3}", DateTime.Now.ToString(), id, author?.FirstName, author?.LastName);
                Console.WriteLine(log);
                return new ActionResult<Int32>(id);
            }
            return this.ValidationProblem();
        }

        // PUT: api/Author/5
        [HttpPut("{id}")]
        public ActionResult<Int32> Put(Int32? id, [FromBody] WebAuthor author)
        {
            if (id != null && author != null)
            {
                string log = string.Format("{0} - [PUT REQUEST] : id = {1}, name = {2} {3}", DateTime.Now.ToString(), id, author?.FirstName, author?.LastName);
                Console.WriteLine(log);
                if (author.AuthorId.Equals(id))
                {
                    return new ActionResult<Int32>(this.m_repository.Refresh(author));
                }
                return NotFound();
            }
            return this.ValidationProblem();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult<bool> Delete(Int32? id)
        {
            if (id != null)
            {
                string log = string.Format("{0} - [DELETE REQUEST] : id = {1}, {2}", DateTime.Now.ToString(), id, "delete author");
                Console.WriteLine(log);
                return new ActionResult<Boolean>(this.m_repository.Remove(this.m_repository.GetEntitie(Entitie.Author, (Int32)id)));
            }
            return this.ValidationProblem();
        }
    }
}
