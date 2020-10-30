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
    public partial class ChiPhiAgent : UGBaseAgent<ChiPhiAgent>
    {
        private const string ChiPhiController = "ChiPhi";
        //api/ChiPhi/Get
        private readonly string ResourceGet = string.Format(ResourceBaseGet, ChiPhiController);
        public async Task<ChiPhi> Get(IDPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGet, ugToken, id);
            var banle = await ExecuteAsync<ChiPhi>(request);

            return banle;
        }
        //api/ChiPhi/GetPagedOlder
        private readonly string ResourceGetPagedOlder = string.Format(ResourceBaseGetPagedOlder, ChiPhiController);
        public async Task<ObservableCollection<ChiPhi>> GetPagedOlder(PagedPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGetPagedOlder, ugToken, id);
            var banle = await ExecuteAsync<ObservableCollection<ChiPhi>>(request);

            return banle;
        }
        //api/ChiPhi/GetLastest
        private readonly string ResourceGetLastest = string.Format(ResourceBaseGetLastest, ChiPhiController);
        public async Task<ObservableCollection<ChiPhi>> GetLastest(LastestPagedPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGetLastest, ugToken, id);
            var banle = await ExecuteAsync< ObservableCollection<ChiPhi>>(request);

            return banle;
        }
        //api/ChiPhi/GetCountLastest
        private readonly string ResourceGetCountLastest = string.Format(ResourceBaseGetCountLastest, ChiPhiController);
        public async Task<int> GetCountLastest(LastestPagedPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGetCountLastest, ugToken, id);
            var banle = await ExecuteAsync<int>(request);

            return banle;
        }
    }
}
