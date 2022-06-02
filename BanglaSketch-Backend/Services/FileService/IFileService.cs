using Dtos.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services.FileService
{
    public interface IFileService
    {
        Task<ServiceResponse<string>> FileUpload(IFormFile file);
        Task<FileContentResult> FileDownload(string fileName);
        Task<ServiceResponse<List<string>>> GetFiles();
    }
}
