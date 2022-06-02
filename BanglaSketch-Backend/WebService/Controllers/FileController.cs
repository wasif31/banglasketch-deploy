using Dtos.Responses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.FileService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Dtos.FileDtos;

namespace WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService fileService;

        public FileController(IFileService fileService)
        {
            this.fileService = fileService;
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload([FromForm(Name ="file")]IFormFile file)
        {
            ServiceResponse<string> response = await fileService.FileUpload(file);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("download/{fileName}")]
        public async Task<IActionResult> Download(string fileName)
        {
            try
            {
                var response = await fileService.FileDownload(fileName);
                return response;
            }
            catch (Exception)
            {
                return BadRequest("File not found.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFiles()
        {
            try
            {
                ServiceResponse<List<string>> response = await fileService.GetFiles();
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest("File not found.");
            }
        }

        [HttpPost("Create")]
        [Route("create")]
        public IActionResult Create([FromForm] FileUploadDto model, IFormFile file)
        {
            try
            {
                /*//ServiceResponse<List<string>> response = await fileService.GetFiles();
                if (!response.Success)
                {
                    return BadRequest(response);
                }*/
                return Ok(200);
            }
            catch (Exception)
            {
                return BadRequest("File not found.");
            }
        }
    }
}
