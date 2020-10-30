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
    public partial class TamUngAgent : UGBaseAgent<TamUngAgent>
    {
        private const string TamUngController = "TamUng";
        //api/TamUng/Get
        private readonly string ResourceGet = string.Format(ResourceBaseGet, TamUngController);
        public async Task<TamUng> Get(IDPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGet, ugToken, id);
            var banle = await ExecuteAsync<TamUng>(request);

            return banle;
        }
        //api/TamUng/GetPagedOlder
        private readonly string ResourceGetPagedOlder = string.Format(ResourceBaseGetPagedOlder, TamUngController);
        public async Task<ObservableCollection<TamUng>> GetPagedOlder(PagedPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGetPagedOlder, ugToken, id);
            var banle = await ExecuteAsync<ObservableCollection<TamUng>>(request);

            return banle;
        }
        //api/TamUng/GetLastest
        private readonly string ResourceGetLastest = string.Format(ResourceBaseGetLastest, TamUngController);
        public async Task<ObservableCollection<TamUng>> GetLastest(LastestPagedPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGetLastest, ugToken, id);
            var banle = await ExecuteAsync<ObservableCollection<TamUng>>(request);

            return banle;
        }
        //api/TamUng/GetCountLastest
        private readonly string ResourceGetCountLastest = string.Format(ResourceBaseGetCountLastest, TamUngController);
        public async Task<int> GetCountLastest(LastestPagedPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGetCountLastest, ugToken, id);
            var banle = await ExecuteAsync<int>(request);

            return banle;
        }
    }
}
