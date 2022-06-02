using Dtos.Responses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Services.FileService
{
    public class FileService : IFileService
    {
        private readonly DataContext context;
        private readonly IWebHostEnvironment hostEnvironment;

        public FileService(DataContext context, IWebHostEnvironment hostEnvironment)
        {
            this.context = context;
            this.hostEnvironment = hostEnvironment;
        }
        public async Task<ServiceResponse<string>> FileUpload(IFormFile file)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try
            {
                if (file == null)
                {
                    response.Success = false;
                    response.Message = "Please choose a file.";
                }
                string fileName = new String(Path.GetFileNameWithoutExtension(file.FileName).Take(35).ToArray()).Replace(' ', '-');
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(file.FileName);
                var destinationFolder = Path.Combine(hostEnvironment.ContentRootPath, "FileStorage");
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                var filePath = Path.Combine(destinationFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                    fileStream.Flush();
                }

                response.Data = fileName;
                response.Message = filePath;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.ToString();
            }
            return response;
        }

        public async Task<FileContentResult> FileDownload(string fileName)
        {
            var filePath = Path.Combine(hostEnvironment.ContentRootPath, "FileStorage", fileName);
            var imagePath= Path.Combine(Directory.GetCurrentDirectory(), "FileStorage", fileName);
            Console.WriteLine(imagePath);
            var mimeType = "application/octet-stream";

            try
            {
                var fileBytes = await File.ReadAllBytesAsync(filePath);
                return new FileContentResult(fileBytes, mimeType)
                {
                    FileDownloadName = fileName
                };
            }
            
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceResponse<List<string>>> GetFiles()
        {
            ServiceResponse<List<string>> response = new ServiceResponse<List<string>>();
            try
            {
                var folderPath = Path.Combine(hostEnvironment.ContentRootPath, "FileStorage");
                var files = Directory.GetFiles(folderPath, "*.pdf", SearchOption.AllDirectories);
                response.Data = files.Select(file => file = Path.GetFileName(file)).ToList();
                response.Message = "All available pdf files";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.ToString();
            }
            return response;
        }
    }
}
