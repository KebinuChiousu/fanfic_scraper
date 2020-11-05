using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using web_scraper.Models.Sites.Base;

namespace web_scraper.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WebScraperController : ControllerBase
    {

        private readonly ILogger<WebScraperController> _logger;        
        
        // Constructor
        public WebScraperController(ILogger<WebScraperController> logger)
        {
            _logger = logger;
        }        
        
        [HttpGet]
        public string Get()
        {
            return "Hello";
        }
    }
}