using Dtos;
using Dtos.Responses;
using Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Services.TranslateService
{
    public class TranslateService : ITranslateService
    {
        private readonly DataContext context;
        public TranslateService(DataContext context)
        {
            this.context = context;
        }
        public async Task<ServiceResponse<string>> SaveTranslationAsync(TranslatedDataDto dto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>() { Success = true};

            TranslatedData data = new TranslatedData()
            {
                BanglaText = dto.BanglaText,
                EnglishText = dto.EnglishText
            };

            await context.TranslatedData.AddAsync(data);
            await context.SaveChangesAsync();
            return response;
        }
    }
}
