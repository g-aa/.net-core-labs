using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Task3_WebApiCore.BooksDB.Abstraction;


namespace WebApiBooksDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IBooksRepository m_repository;

        public StatisticsController(IBooksRepository repository)
        {
            if (repository != null)
            {
                this.m_repository = repository;
                return;
            }
            throw new ArgumentNullException("inpun repositiry is null!");
        }

        // GET: api/Statistics
        [HttpGet]
        public ActionResult<IEnumerable<KeyValuePair<String, String>>> GetStatistic()
        {
            String log = String.Format("{0} - [GET REQUEST] : {1}", DateTime.Now.ToString(), "BD statistics");
            Console.WriteLine(log);
            return new ActionResult<IEnumerable<KeyValuePair<String, String>>>(this.m_repository.GetStatistics());
        }
    }
}
