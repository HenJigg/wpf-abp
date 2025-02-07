using Abp.Application.Services.Dto;
using AppFramework.ApiClient;
using AppFramework.Localization;
using AppFramework.Localization.Dto;
using System.Threading.Tasks;

namespace AppFramework.Application
{
    public class LanguageAppService : ProxyAppServiceBase, ILanguageAppService
    {
        public LanguageAppService(AbpApiClient apiClient) : base(apiClient)
        {
        }

        public async Task CreateOrUpdateLanguage(CreateOrUpdateLanguageInput input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(CreateOrUpdateLanguage)), input);
        }

        public async Task DeleteLanguage(EntityDto input)
        {
            await ApiClient.DeleteAsync(GetEndpoint(nameof(DeleteLanguage)), input);
        }

        public async Task<GetLanguageForEditOutput> GetLanguageForEdit(NullableIdDto input)
        {
            return await ApiClient.GetAsync<GetLanguageForEditOutput>(GetEndpoint(nameof(GetLanguageForEdit)), input);
        }

        public async Task<GetLanguagesOutput> GetLanguages()
        {
            return await ApiClient.GetAsync<GetLanguagesOutput>(GetEndpoint(nameof(GetLanguages)));
        }

        public async Task<PagedResultDto<LanguageTextListDto>> GetLanguageTexts(GetLanguageTextsInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<LanguageTextListDto>>(GetEndpoint(nameof(GetLanguageTexts)), input);
        }

        public async Task SetDefaultLanguage(SetDefaultLanguageInput input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(SetDefaultLanguage)), input);
        }

        public async Task UpdateLanguageText(UpdateLanguageTextInput input)
        {
            await ApiClient.PutAsync(GetEndpoint(nameof(UpdateLanguageText)), input);
        }
    }
}