using Dtos.SearchingDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.SearchingService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchingController : ControllerBase
    {
        private readonly ISearchingService searchingService;

        public SearchingController(ISearchingService searchingService)
        {
            this.searchingService = searchingService;
        }

        [HttpPost]
        public async Task<IActionResult> Searching(SearchingDto request)
        {
            try
            {
                var response = searchingService.Searching(request.Words);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest("File not found.");
            }
        }

        [HttpPost]
        [Route("WithDataset")]
        public async Task<IActionResult> SearchingWithDataset(SearchingWithDatasetDto request)
        {
            try
            {
                var response = searchingService.SearchingWithDataset(request.Words, request.DatasetName);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest("File not found.");
            }
        }
    }
}
