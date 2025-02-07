using AppFramework.Admin.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AppFramework
{
    public static class PermissionHelper
    {
        /// <summary>
        /// 创建权限节点目录树
        /// </summary>
        /// <param name="flats"></param>
        /// <param name="parentName"></param>
        /// <returns></returns>
        public static ObservableCollection<PermissionModel> CreateTrees(this List<PermissionModel> flats, PermissionModel? model)
        {
            var trees = new ObservableCollection<PermissionModel>();
            var nodes = flats.Where(q => q.ParentName == model?.Name).ToArray();

            foreach (var node in nodes)
            {
                node.Items = CreateTrees(flats, node);
                node.Parent = model;
                trees.Add(node);
            }

            return trees;
        }

        /// <summary>
        /// 获取选中节点列表
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="GrantedPermissionNames"></param>
        /// <returns></returns>
        public static ObservableCollection<object> GetSelectedItems(this ObservableCollection<PermissionModel> nodes, List<string> GrantedPermissionNames)
        {
            var permItems = new ObservableCollection<object>();

            foreach (var item in GrantedPermissionNames)
            {
                var permItem = GetSelectedItems(nodes, item);
                if (permItem != null) permItems.Add(permItem);
            }

            return permItems;

            PermissionModel GetSelectedItems(ObservableCollection<PermissionModel> nodes, string key)
            {
                PermissionModel model = null;

                foreach (var flat in nodes)
                {
                    if (flat.Name.Equals(key))//&& flat.Items.Count == 0)
                    {
                        model = flat;
                        break;
                    }
                    model = GetSelectedItems(flat.Items, key);

                    if (model != null) break;
                }
                return model;
            }
        }
    }
}
