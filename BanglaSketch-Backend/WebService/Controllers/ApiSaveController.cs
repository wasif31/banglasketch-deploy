using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dtos;
using Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using Services.TranslateService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiSaveController : ControllerBase
    {
        private readonly ITranslateService translateService;
        public ApiSaveController(ITranslateService translateService)
        {
            this.translateService = translateService;
        }

        // POST api/<ApiSaveController>
        [HttpPost("ApiPost")]
        public async Task<IActionResult> ApiPost([FromBody] TranslatedDataDto value)
        {
            ServiceResponse<string> response =  await translateService.SaveTranslationAsync(value);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> test()
        {
            return Ok(200);
        }
    }
}
