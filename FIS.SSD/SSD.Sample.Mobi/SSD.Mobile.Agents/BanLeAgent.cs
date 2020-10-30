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
    public partial class BanLeAgent : UGBaseAgent<BanLeAgent>
    {
        private const string BanLeController = "BanLe";
        //api/BanLe/Get
        private readonly string ResourceGet = string.Format(ResourceBaseGet, BanLeController);
        public async Task<BanLe> Get(IDPara id,string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGet, ugToken, id);
            var banle = await ExecuteAsync<BanLe>(request);

            return banle;
        }
        //api/BanLe/GetPagedOlder
        private readonly string ResourceGetPagedOlder = string.Format(ResourceBaseGetPagedOlder, BanLeController);
        public async Task<ObservableCollection<BanLe>> GetPagedOlder(PagedPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGetPagedOlder, ugToken, id);
            var banle = await ExecuteAsync<ObservableCollection<BanLe>>(request);

            return banle;
        }
        //api/BanLe/GetLastest
        private readonly string ResourceGetLastest = string.Format(ResourceBaseGetLastest, BanLeController);
        public async Task<ObservableCollection<BanLe>> GetLastest(LastestPagedPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGetLastest, ugToken, id);
            var banle = await ExecuteAsync<ObservableCollection<BanLe>>(request);

            return banle;
        }
        //api/BanLe/GetCountLastest
        private readonly string ResourceGetCountLastest = string.Format(ResourceBaseGetCountLastest, BanLeController);
        public async Task<int> GetCountLastest(LastestPagedPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGetCountLastest, ugToken, id);
            var banle = await ExecuteAsync<int>(request);

            return banle;
        }
        //api/BanLe/GetFullPagedOlder
        private readonly string ResourceGetFullPagedOlder = string.Format("api/{0}/GetFullPagedOlder", BanLeController);
        public async Task<ObservableCollection<BanLe>> GetFullPagedOlder(PagedPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGetFullPagedOlder, ugToken, id);
            var banle = await ExecuteAsync<ObservableCollection<BanLe>>(request);

            return banle;
        }
        //api/BanLe/GetFullLastest
        private readonly string ResourceGetFullLastest = string.Format("api/{0}/GetFullLastest", BanLeController);
        public async Task<ObservableCollection<BanLe>> GetFullLastest(LastestPagedPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGetFullLastest, ugToken, id);
            var banle = await ExecuteAsync< ObservableCollection<BanLe>>(request);

            return banle;
        }
    }
}
