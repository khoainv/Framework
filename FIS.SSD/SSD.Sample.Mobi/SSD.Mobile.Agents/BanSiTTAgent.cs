using SSD.Mobile.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using SSD.Framework;
using SSD.Mobile.Common.Para;

namespace SSD.Mobile.Agents
{
    public partial class BanSiTTAgent : UGBaseAgent<BanSiTTAgent>
    {
        private const string BanSiTTController = "BanSiTT";
        //api/BanSiTT/GetList
        private readonly string ResourceGetList = string.Format("api/{0}/GetList", BanSiTTController);
        public async Task<ObservableCollection<BanSiTT>> GetList(ListFullPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGetList, ugToken, id);
            var banle = await ExecuteAsync<ObservableCollection<BanSiTT>>(request);

            return banle;
        }
    }
}
