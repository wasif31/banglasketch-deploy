using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Services.SearchingService
{
    public class SearchingService : ISearchingService
    {
        private readonly IWebHostEnvironment hostEnvironment;

        public SearchingService(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public List<string> Searching(string words)
        {
            List<string> result = new List<string>();
            var folderPath = Path.Combine(hostEnvironment.ContentRootPath, "Datasets");
            var files = Directory.GetFiles(folderPath, "*.txt", SearchOption.TopDirectoryOnly);

            foreach (var file in files)
            {
                var input = File.ReadAllText(files[0]);
                string[] sentences = Regex.Split(input, @"\n");

                foreach (string sentence in sentences)
                {
                    if (Regex.IsMatch(sentence, $"\\b{words}\\b"))
                        result.Add(sentence);
                }
            }

            return result;
        }

        public List<string> SearchingWithDataset(string words, string datasetName)
        {
            try
            {
                List<string> result = new List<string>();
                var filePath = Path.Combine(hostEnvironment.ContentRootPath, "Datasets", datasetName);

                var input = File.ReadAllText(filePath);
                string[] sentences = Regex.Split(input, @"\n");

                foreach (string sentence in sentences)
                {
                    if (Regex.IsMatch(sentence, $"\\b{words}\\b"))
                        result.Add(sentence);
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
