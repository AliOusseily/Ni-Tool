using AggregatorApi.Model;
using AggregatorApi.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AggregatorApi.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class FetchController : ControllerBase
    {
        private readonly FetchingService _fetchingService;
        IList<AggregatedField> Fields = new List<AggregatedField>();

        public FetchController(FetchingService fetchingService)
        {
            _fetchingService = fetchingService;
        }
        [EnableCors("MyPolicy")]
        [HttpGet("FetchHourly")]
        public IActionResult FetchHourly(/*DateTime From, DateTime To, string NeAlias*/)
        {
           Fields= _fetchingService.GetHourlyData(/*From, To, NeAlias*/);
            return Ok(Fields);
        }
        [EnableCors("MyPolicy")]
        [HttpGet("FetchDaily")]
        public IActionResult FetchDaily(/*DateTime From, DateTime To, string NeAlias*/)
        {
            Fields = _fetchingService.GetDailyData(/*From, To, NeAlias*/);
            return Ok(Fields);
        }
    }
}
