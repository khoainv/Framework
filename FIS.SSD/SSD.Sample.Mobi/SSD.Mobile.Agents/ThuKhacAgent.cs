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
    public partial class ThuKhacAgent : UGBaseAgent<ThuKhacAgent>
    {
        private const string ThuKhacController = "ThuKhac";
        //api/ThuKhac/Get
        private readonly string ResourceGet = string.Format(ResourceBaseGet, ThuKhacController);
        public async Task<ThuKhac> Get(IDPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGet, ugToken, id);
            var banle = await ExecuteAsync<ThuKhac>(request);

            return banle;
        }
        //api/ThuKhac/GetPagedOlder
        private readonly string ResourceGetPagedOlder = string.Format(ResourceBaseGetPagedOlder, ThuKhacController);
        public async Task<ObservableCollection<ThuKhac>> GetPagedOlder(PagedPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGetPagedOlder, ugToken, id);
            var banle = await ExecuteAsync<ObservableCollection<ThuKhac>>(request);

            return banle;
        }
        //api/ThuKhac/GetLastest
        private readonly string ResourceGetLastest = string.Format(ResourceBaseGetLastest, ThuKhacController);
        public async Task<ObservableCollection<ThuKhac>> GetLastest(LastestPagedPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGetLastest, ugToken, id);
            var banle = await ExecuteAsync<ObservableCollection<ThuKhac>>(request);

            return banle;
        }
        //api/ThuKhac/GetCountLastest
        private readonly string ResourceGetCountLastest = string.Format(ResourceBaseGetCountLastest, ThuKhacController);
        public async Task<int> GetCountLastest(LastestPagedPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGetCountLastest, ugToken, id);
            var banle = await ExecuteAsync< int>(request);

            return banle;
        }
    }
}
