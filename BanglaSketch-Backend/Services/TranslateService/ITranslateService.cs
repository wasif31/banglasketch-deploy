using Dtos;
using Dtos.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.TranslateService
{
    public interface ITranslateService
    {
        Task<ServiceResponse<string>> SaveTranslationAsync(TranslatedDataDto dto);
    }
}
