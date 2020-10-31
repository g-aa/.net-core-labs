using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Task3_WebApiCore.BooksDB.Abstraction;
using Task3_WebApiCore.BooksDB.Entities.DbEntity;
using Task3_WebApiCore.BooksDB.Entities.WebEntity;
using Task3_WebApiCore.BooksDB.Enumeration;
using Task3_WebApiCore.BooksDB.Realization;

namespace Task3_WebApiCore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IBooksRepository m_repository;

        public AddressController(IBooksRepository repository)
        {
            if (repository != null)
            {
                this.m_repository = repository;
                return;
            }
            throw new ArgumentNullException("входной параметр repositiry равен null");
        }

        // GET: api/Address
        [HttpGet]
        public ActionResult<IEnumerable<WebAddress>> GetAllAddresses()
        {
            string log = string.Format("{0} - [GET REQUEST] : {1}", DateTime.Now.ToString(), "all addresses");
            Console.WriteLine(log);
            
            List<Object> objs = this.m_repository.GetAllEntities(Entitie.Address);
            List<WebAddress> result = new List<WebAddress>(objs.Count);
            objs.ForEach((Object o) => result.Add((WebAddress)o));
            return new ActionResult<IEnumerable<WebAddress>>(result);
        }

        // GET: api/Address/5
        [HttpGet("{id}")]
        public ActionResult<WebAddress> GetAddress(Int32? id)
        {
            if (id != null)
            {
                WebAddress a = (WebAddress)this.m_repository.GetEntitie(Entitie.Address, (Int32)id);
                string log = string.Format("{0} - [GET REQUEST] : id = {1}, address = {2} {3} {4}", DateTime.Now.ToString(), id, a?.Country, a?.City, a?.Street);
                Console.WriteLine(log);
                if (a != null)
                {
                    return new ActionResult<WebAddress>(a);
                }
                return this.NotFound();
            }
            return this.ValidationProblem();
        }

        // POST: api/Address
        [HttpPost]
        public ActionResult<Int32> Post([FromBody] WebAddress address)
        {
            if (address != null)
            {
                Int32 id = this.m_repository.Add(address);
                string log = string.Format("{0} - [POST REQUEST] : id = {1}, address = {2} {3} {4}", DateTime.Now.ToString(), id, address?.Country, address?.City, address?.Street);
                Console.WriteLine(log);
                return new ActionResult<Int32>(id);
            }
            return this.ValidationProblem();
        }

        // PUT: api/Address/5
        [HttpPut("{id}")]
        public ActionResult<Int32> Put(Int32? id, [FromBody] WebAddress address)
        {
            if (id != null && address != null)
            {
                string log = string.Format("{0} - [PUT REQUEST] : id = {1}, address = {2} {3} {4}", DateTime.Now.ToString(), id, address?.Country, address?.City, address?.Street);
                Console.WriteLine(log);
                if (address.AddressId.Equals(id))
                {
                    return new ActionResult<Int32>(this.m_repository.Refresh(address));
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
                string log = string.Format("{0} - [DELETE REQUEST] : id = {1}, {2}", DateTime.Now.ToString(), id, "delete address");
                Console.WriteLine(log);
                return new ActionResult<Boolean>(this.m_repository.Remove(this.m_repository.GetEntitie(Entitie.Address, (Int32)id)));
            }
            return this.ValidationProblem();
        }
    }
}
