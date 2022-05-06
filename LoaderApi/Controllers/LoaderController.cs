using LoaderApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoaderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

 
    public class LoaderController : ControllerBase
    {
        private LoaderService _LoaderService;
        private readonly HttpRequestService _httpRequestService;


        public LoaderController(LoaderService LoaderService, HttpRequestService httpRequestService)
        {
            _LoaderService = LoaderService;
            _httpRequestService = httpRequestService;
        }

        [HttpPost("Load")]
        public async Task<IActionResult> Load(FilePathObjectRequest file)
        {
            Console.WriteLine(file.FilePath);
            Console.WriteLine("----------------------------------------------------------");
            _LoaderService.Load(file.FilePath);
            await _httpRequestService.UpdateAggregator();
            Console.WriteLine("Loaded");
            return Ok();
        }
    }

    public class FilePathObjectRequest
    {
        public string FilePath { get; set; }
    }
}
