using System;
using System.Collections.Generic;
using System.Text;

namespace Services.SearchingService
{
    public interface ISearchingService
    {
        public List<string> Searching(string words);
        public List<string> SearchingWithDataset(string words, string datasetName);
    }
}
