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
    public class PublisherController : ControllerBase
    {
        private readonly IBooksRepository m_repository;

        public PublisherController(IBooksRepository repository)
        {
            if (repository != null)
            {
                this.m_repository = repository;
                return;
            }
            throw new ArgumentNullException("входной параметр repositiry равен null");
        }

        // GET: api/Publisher
        [HttpGet]
        public ActionResult<IEnumerable<WebPublisher>> GetAllPublisher()
        {
            string log = string.Format("{0} - [GET REQUEST] : {1}", DateTime.Now.ToString(), "all publisher");
            Console.WriteLine(log);
            List<Object> objs = this.m_repository.GetAllEntities(Entitie.Publisher);
            List<WebPublisher> result = new List<WebPublisher>(objs.Count);
            objs.ForEach((Object o) => result.Add((WebPublisher)o));
            return new ActionResult<IEnumerable<WebPublisher>>(result);
        }

        // GET: api/Publisher/5
        [HttpGet("{id}")]
        public ActionResult<WebPublisher> Get(Int32? id)
        {
            if (id != null)
            {
                WebPublisher p = (WebPublisher)this.m_repository.GetEntitie(Entitie.Publisher, (Int32)id);
                string log = string.Format("{0} - [GET REQUEST] : id = {1}, title = {2}", DateTime.Now.ToString(), id, p?.Title);
                Console.WriteLine(log);
                if (p != null)
                {
                    return new ActionResult<WebPublisher>(p);
                }
                return this.NotFound();
            }
            return this.ValidationProblem();
        }

        // POST: api/Publisher
        [HttpPost]
        public ActionResult<Int32> Post([FromBody] WebPublisher publisher)
        {
            if (publisher != null)
            {
                Int32 id = this.m_repository.Add(publisher);
                string log = string.Format("{0} - [POST REQUEST] : id = {1}, title = {2}", DateTime.Now.ToString(), id, publisher?.Title);
                Console.WriteLine(log);
                return new ActionResult<Int32>(id);
            }
            return this.ValidationProblem();
        }

        // PUT: api/Publisher/5
        [HttpPut("{id}")]
        public ActionResult<Int32> Put(Int32? id, [FromBody] WebPublisher publisher)
        {
            if (id != null && publisher != null)
            {
                string log = string.Format("{0} - [PUT REQUEST] : id = {1}, title = {2}", DateTime.Now.ToString(), id, publisher?.Title);
                Console.WriteLine(log);
                if (publisher.PublisherId.Equals(id))
                {
                    return new ActionResult<Int32>(this.m_repository.Refresh(publisher));
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
                string log = string.Format("{0} - [DELETE REQUEST] : id = {1}, {2}", DateTime.Now.ToString(), id, "delete publisher");
                Console.WriteLine(log);
                return new ActionResult<Boolean>(this.m_repository.Remove(this.m_repository.GetEntitie(Entitie.Publisher, (Int32)id)));
            }
            return this.ValidationProblem();
        }
    }
}
