using GalaSoft.MvvmLight;
using NoteMoblie.Core.BusinessTemp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NoteMoblie.ViewModel
{
    public class UserViewModel : ViewModelBase
    {
        public UserViewModel()
        {
            BillList = new ObservableCollection<UserBillEntity>();

            BillList.Add(new UserBillEntity() { Title = "基金收益", Money = 268.00M, Remark = "投资理财", });
            BillList.Add(new UserBillEntity() { Title = "大润发购物中心", Money = 45.00M, Remark = "其他", });
            BillList.Add(new UserBillEntity() { Title = "Surface Laptop3", Money = -11999, Remark = "生活日用", });
            BillList.Add(new UserBillEntity() { Title = "Office 365 家庭版", Money = -243.99M, Remark = "家庭教育", });
            BillList.Add(new UserBillEntity() { Title = "购买大会员连续包年", Money = 148.00M, Remark = "休闲娱乐", });
            BillList.Add(new UserBillEntity() { Title = "MEO X 防雾霾口罩", Money = 560.24M, Remark = "生活日用", });
            BillList.Add(new UserBillEntity() { Title = "全新2019MacBook Pro/I7/16/512", Money = -15480, Remark = "生活日用", Status = "交易关闭" });
            BillList.Add(new UserBillEntity() { Title = "AA收款帐单", Money = 472.00M, Remark = "交通出行", });
            BillList.Add(new UserBillEntity() { Title = "UNIQLO", Money = -399.00M, Remark = "服饰美容", });
        }

        private ObservableCollection<UserBillEntity> billList;

        public ObservableCollection<UserBillEntity> BillList
        {
            get { return billList; }
            set { billList = value; RaisePropertyChanged(); }
        }

    }
}
