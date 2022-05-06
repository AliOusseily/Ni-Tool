using AggregatorApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AggregatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AggregatorController : ControllerBase
    {
        private readonly AggregatingService _aggregatingService;

        public AggregatorController(AggregatingService aggregatingService)
        {
            _aggregatingService = aggregatingService;
        }

        [HttpGet("Aggregate")]
        public IActionResult Aggregate()
        {
            Console.WriteLine("Inside the Aggregator controller");
            Console.WriteLine("---------------------------------------------------------------");
            _aggregatingService.Aggregate();
            Console.WriteLine("Aggregated");
            return Ok();
        }
    }
}
