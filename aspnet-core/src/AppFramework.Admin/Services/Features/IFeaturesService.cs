using Abp.Application.Services.Dto;
using AppFramework.Editions.Dto; 
using System.Collections.Generic; 

namespace AppFramework.Admin.Services
{
    public interface IFeaturesService
    {
        void CreateFeatures(List<FlatFeatureDto> features, List<NameValueDto> featureValues);

        List<NameValueDto> GetSelectedItems();
    }
}
