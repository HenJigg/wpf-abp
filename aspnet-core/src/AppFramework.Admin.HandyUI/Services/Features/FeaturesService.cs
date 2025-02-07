using Abp.Application.Services.Dto;
using AppFramework.Editions.Dto;
using AppFramework.Admin.Models;
using AppFramework.Shared.Services.Mapper;
using AutoMapper;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AppFramework.Admin.Services;

namespace AppFramework.Admin.HandyUI.Services
{
    public class FeaturesService : BindableBase, IFeaturesService
    {
        private readonly IAppMapper mapper;

        public FeaturesService(IAppMapper mapper)
        {
            this.mapper = mapper;
        }

        private ObservableCollection<FlatFeatureModel> features;

        public ObservableCollection<FlatFeatureModel> Features
        {
            get { return features; }
            set { features = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<object> SelectedItems { get; set; }

        public void CreateFeatures(List<FlatFeatureDto> features, List<NameValueDto> featureValues)
        {
            if (features == null)
                throw new NullReferenceException(nameof(features));

            if (featureValues == null)
                throw new NullReferenceException(nameof(featureValues));

            var flats = mapper.Map<List<FlatFeatureModel>>(features);

            Features = CreateFeatureTrees(flats);
            UpdateFeaturesIsCheckedState(Features, featureValues);
        }

        public List<NameValueDto> GetSelectedItems()
        {
            List<NameValueDto> items = new List<NameValueDto>();
            GetFeatures(Features, ref items);

            return items;
        }

        /// <summary>
        /// 获取选中的功能节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="GrantedPermissionNames"></param>
        private void GetFeatures(ObservableCollection<FlatFeatureModel> flatFeatures, ref List<NameValueDto> featureValues)
        {
            foreach (var item in flatFeatures)
            {
                if (bool.TryParse(item.DefaultValue, out bool result))
                {
                    featureValues.Add(new NameValueDto(item.Name, item.IsChecked ? "true" : "false"));
                }
                else
                    featureValues.Add(new NameValueDto(item.Name, item.DefaultValue));

                GetFeatures(item.Items, ref featureValues);
            }
        }

        /// <summary>
        /// 创建功能结点目录树
        /// </summary>
        /// <param name="flats"></param>
        /// <param name="parentName"></param>
        /// <returns></returns>
        private ObservableCollection<FlatFeatureModel> CreateFeatureTrees(List<FlatFeatureModel> flatFeatureModels, string? parentName = null)
        {
            var trees = new ObservableCollection<FlatFeatureModel>();
            var nodes = flatFeatureModels.Where(q => q.ParentName == parentName).ToArray();

            foreach (var node in nodes)
            {
                node.Items = CreateFeatureTrees(flatFeatureModels, node.Name);
                trees.Add(node);
            }
            return trees;
        }

        private void UpdateFeaturesIsCheckedState(ObservableCollection<FlatFeatureModel> features, List<NameValueDto> featureValues)
        {
            foreach (var f in featureValues)
            {
                UpdateIsCheckedState(features, f);
            }

            void UpdateIsCheckedState(ObservableCollection<FlatFeatureModel> flatFeatures, NameValueDto nameValue)
            {
                foreach (var flat in flatFeatures)
                {
                    if (flat.Name.Equals(nameValue.Name) && flat.Items.Count == 0)
                    {
                        bool isAdd = false;
                        if (bool.TryParse(nameValue.Value, out bool result))
                            isAdd = result;
                        else
                            isAdd = true;

                        if (isAdd) flat.IsChecked=true;
                    }
                    UpdateIsCheckedState(flat.Items, nameValue);
                }
            }
        }
    }
}
