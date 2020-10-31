using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Task3_WebApiCore.BooksDB.Abstraction;
using Task3_WebApiCore.BooksDB.Enumeration;
using Task3_WebApiCore.BooksDB.Realization;

namespace Task3_WebApiCore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookDbController : ControllerBase
    {
        private readonly IBooksRepository m_repository;

        public BookDbController(IBooksRepository repository)
        {
            if (repository != null)
            {
                this.m_repository = repository;
                return;
            }
            throw new ArgumentNullException("входной параметр repositiry равен null!");
        }

        // GET: api/BookDb
        [HttpGet]
        public ActionResult<String> GetBookDb()
        {
            string log = string.Format("{0} - [GET REQUEST] : {1}", DateTime.Now.ToString(), "Book BD");
            Console.WriteLine(log);
            return new ActionResult<String>(this.m_repository.Print(Entitie.None));
        }
    }
}
