using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParserApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParserController : ControllerBase
    {
        private readonly ParsingService _parsingService;

        
        public ParserController(ParsingService parsingService )
        {
            _parsingService = parsingService;
        }

        [HttpGet]
        public  IActionResult Test()
        {

            _parsingService.Parse("C:/Users/User/Desktop/SOEM1_TN_RADIO_LINK_POWER_20200312_001500.txt");
          
            return Ok();
        }
    }
}
