using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Pos.Api.Infrastructure;
using Pos.Contracts;

namespace Pos.Api.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : BaseApiController
    {
        private readonly ILoggerManager _logger;

        public OrderController(ILoggerManager logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get products with paging
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            _logger.LogInfo("Hello from index");

            return new[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}