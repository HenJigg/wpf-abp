using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.UI;
using AppFramework.Authorization;
using AppFramework.Localization.Dto;

namespace AppFramework.Localization
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Languages)]
    public class LanguageAppService : AppFrameworkAppServiceBase, ILanguageAppService
    {
        private readonly IApplicationLanguageManager _applicationLanguageManager;
        private readonly IApplicationLanguageTextManager _applicationLanguageTextManager;
        private readonly IRepository<ApplicationLanguage> _languageRepository;
        private readonly IApplicationCulturesProvider _applicationCulturesProvider;

        public LanguageAppService(
            IApplicationLanguageManager applicationLanguageManager,
            IApplicationLanguageTextManager applicationLanguageTextManager,
            IRepository<ApplicationLanguage> languageRepository,
            IApplicationCulturesProvider applicationCulturesProvider)
        {
            _applicationLanguageManager = applicationLanguageManager;
            _languageRepository = languageRepository;
            _applicationLanguageTextManager = applicationLanguageTextManager;
            _applicationCulturesProvider = applicationCulturesProvider;
        }

        public async Task<GetLanguagesOutput> GetLanguages()
        {
            var languages =
                (await _applicationLanguageManager.GetLanguagesAsync(AbpSession.TenantId)).OrderBy(l => l.DisplayName);
            var defaultLanguage = await _applicationLanguageManager.GetDefaultLanguageOrNullAsync(AbpSession.TenantId);

            return new GetLanguagesOutput(
                ObjectMapper.Map<List<ApplicationLanguageListDto>>(languages),
                defaultLanguage?.Name
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Languages_Create,
            AppPermissions.Pages_Administration_Languages_Edit)]
        public async Task<GetLanguageForEditOutput> GetLanguageForEdit(NullableIdDto input)
        {
            ApplicationLanguage language = null;
            if (input.Id.HasValue)
            {
                language = await _languageRepository.GetAsync(input.Id.Value);
            }

            var output = new GetLanguageForEditOutput();

            //Language
            output.Language = language != null
                ? ObjectMapper.Map<ApplicationLanguageEditDto>(language)
                : new ApplicationLanguageEditDto();

            //Language names
            output.LanguageNames = _applicationCulturesProvider
                .GetAllCultures()
                .Select(c => new ComboboxItemDto(c.Name, c.EnglishName + " (" + c.Name + ")")
                    {IsSelected = output.Language.Name == c.Name})
                .ToList();

            //Flags
            output.Flags = FamFamFamFlagsHelper
                .FlagClassNames
                .OrderBy(f => f)
                .Select(f => new ComboboxItemDto(f, FamFamFamFlagsHelper.GetCountryCode(f))
                    {IsSelected = output.Language.Icon == f})
                .ToList();

            return output;
        }

        public async Task CreateOrUpdateLanguage(CreateOrUpdateLanguageInput input)
        {
            if (input.Language.Id.HasValue)
            {
                await UpdateLanguageAsync(input);
            }
            else
            {
                await CreateLanguageAsync(input);
            }
        }

        public async Task DeleteLanguage(EntityDto input)
        {
            var language = await _languageRepository.GetAsync(input.Id);
            await _applicationLanguageManager.RemoveAsync(AbpSession.TenantId, language.Name);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Languages_ChangeDefaultLanguage)]
        public async Task SetDefaultLanguage(SetDefaultLanguageInput input)
        {
            await _applicationLanguageManager.SetDefaultLanguageAsync(
                AbpSession.TenantId,
                CultureHelper.GetCultureInfoByChecking(input.Name).Name
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Languages_ChangeTexts)]
        public async Task<PagedResultDto<LanguageTextListDto>> GetLanguageTexts(GetLanguageTextsInput input)
        {
            /* Note: This method is used by SPA without paging, MPA with paging.
             * So, it can both usable with paging or not */

            //Normalize base language name
            if (input.BaseLanguageName.IsNullOrEmpty())
            {
                var defaultLanguage =
                    await _applicationLanguageManager.GetDefaultLanguageOrNullAsync(AbpSession.TenantId);
                if (defaultLanguage == null)
                {
                    defaultLanguage = (await _applicationLanguageManager.GetLanguagesAsync(AbpSession.TenantId))
                        .FirstOrDefault();
                    if (defaultLanguage == null)
                    {
                        throw new Exception("No language found in the application!");
                    }
                }

                input.BaseLanguageName = defaultLanguage.Name;
            }

            var source = LocalizationManager.GetSource(input.SourceName);
            var baseCulture = CultureInfo.GetCultureInfo(input.BaseLanguageName);
            var targetCulture = CultureInfo.GetCultureInfo(input.TargetLanguageName);

            var allStrings = source.GetAllStrings();
            var baseValues = _applicationLanguageTextManager.GetStringsOrNull(
                AbpSession.TenantId,
                source.Name,
                baseCulture,
                allStrings.Select(x => x.Name).ToList()
            );

            var targetValues = _applicationLanguageTextManager.GetStringsOrNull(
                AbpSession.TenantId,
                source.Name,
                targetCulture,
                allStrings.Select(x => x.Name).ToList()
            );

            var languageTexts = allStrings.Select((t, i) => new LanguageTextListDto
            {
                Key = t.Name,
                BaseValue = GetValueOrNull(baseValues, i),
                TargetValue = GetValueOrNull(targetValues, i) ?? GetValueOrNull(baseValues, i) 
            }).AsQueryable();

            //Filters
            if (input.TargetValueFilter == "EMPTY")
            {
                languageTexts = languageTexts.Where(s => s.TargetValue.IsNullOrEmpty());
            }

            if (!input.FilterText.IsNullOrEmpty())
            {
                languageTexts = languageTexts.Where(
                    l => (l.Key != null &&
                          l.Key.IndexOf(input.FilterText, StringComparison.CurrentCultureIgnoreCase) >= 0) ||
                         (l.BaseValue != null &&
                          l.BaseValue.IndexOf(input.FilterText, StringComparison.CurrentCultureIgnoreCase) >= 0) ||
                         (l.TargetValue != null &&
                          l.TargetValue.IndexOf(input.FilterText, StringComparison.CurrentCultureIgnoreCase) >= 0)
                );
            }

            var totalCount = languageTexts.Count();

            //Ordering
            if (!input.Sorting.IsNullOrEmpty())
            {
                languageTexts = languageTexts.OrderBy(input.Sorting);
            }

            //Paging
            if (input.SkipCount > 0)
            {
                languageTexts = languageTexts.Skip(input.SkipCount);
            }

            if (input.MaxResultCount > 0)
            {
                languageTexts = languageTexts.Take(input.MaxResultCount);
            }

            return new PagedResultDto<LanguageTextListDto>(
                totalCount,
                languageTexts.ToList()
            );
        }

        public async Task UpdateLanguageText(UpdateLanguageTextInput input)
        {
            var culture = CultureHelper.GetCultureInfoByChecking(input.LanguageName);
            var source = LocalizationManager.GetSource(input.SourceName);
            await _applicationLanguageTextManager.UpdateStringAsync(
                AbpSession.TenantId,
                source.Name,
                culture,
                input.Key,
                input.Value
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Languages_Create)]
        protected virtual async Task CreateLanguageAsync(CreateOrUpdateLanguageInput input)
        {
            if (AbpSession.MultiTenancySide != MultiTenancySides.Host)
            {
                throw new UserFriendlyException(L("TenantsCannotCreateLanguage"));
            }

            var culture = CultureHelper.GetCultureInfoByChecking(input.Language.Name);

            await CheckLanguageIfAlreadyExists(culture.Name);

            await _applicationLanguageManager.AddAsync(
                new ApplicationLanguage(
                    AbpSession.TenantId,
                    culture.Name,
                    culture.DisplayName,
                    input.Language.Icon
                )
                {
                    IsDisabled = !input.Language.IsEnabled
                }
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Languages_Edit)]
        protected virtual async Task UpdateLanguageAsync(CreateOrUpdateLanguageInput input)
        {
            Debug.Assert(input.Language.Id != null, "input.Language.Id != null");

            var culture = CultureHelper.GetCultureInfoByChecking(input.Language.Name);

            await CheckLanguageIfAlreadyExists(culture.Name, input.Language.Id.Value);

            var language = await _languageRepository.GetAsync(input.Language.Id.Value);

            language.Name = culture.Name;
            language.DisplayName = culture.DisplayName;
            language.Icon = input.Language.Icon;
            language.IsDisabled = !input.Language.IsEnabled;

            await _applicationLanguageManager.UpdateAsync(AbpSession.TenantId, language);
        }

        private async Task CheckLanguageIfAlreadyExists(string languageName, int? expectedId = null)
        {
            var existingLanguage = (await _applicationLanguageManager.GetLanguagesAsync(AbpSession.TenantId))
                .FirstOrDefault(l => l.Name == languageName);

            if (existingLanguage == null)
            {
                return;
            }

            if (expectedId != null && existingLanguage.Id == expectedId.Value)
            {
                return;
            }

            throw new UserFriendlyException(L("ThisLanguageAlreadyExists"));
        }

        private string GetValueOrNull(List<string> items, int index)
        {
            return items.Count > index ? items[index] : null;
        }
    }
}
