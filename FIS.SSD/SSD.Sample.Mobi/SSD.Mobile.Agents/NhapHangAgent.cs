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
    public partial class NhapHangAgent : UGBaseAgent<NhapHangAgent>
    {

        private const string NhapHangController = "NhapHang";
        //api/NhapHang/Get
        private readonly string ResourceGet = string.Format(ResourceBaseGet, NhapHangController);
        public async Task<NhapHang> Get(IDPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGet, ugToken, id);
            var banle = await ExecuteAsync<NhapHang>(request);

            return banle;
        }
        //api/NhapHang/GetPagedOlder
        private readonly string ResourceGetPagedOlder = string.Format(ResourceBaseGetPagedOlder, NhapHangController);
        public async Task<ObservableCollection<NhapHang>> GetPagedOlder(PagedPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGetPagedOlder, ugToken, id);
            var banle = await ExecuteAsync< ObservableCollection<NhapHang>>(request);

            return banle;
        }
        //api/NhapHang/GetLastest
        private readonly string ResourceGetLastest = string.Format(ResourceBaseGetLastest, NhapHangController);
        public async Task<ObservableCollection<NhapHang>> GetLastest(LastestPagedPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGetLastest, ugToken, id);
            var banle = await ExecuteAsync<ObservableCollection<NhapHang>>(request);

            return banle;
        }
        //api/NhapHang/GetCountLastest
        private readonly string ResourceGetCountLastest = string.Format(ResourceBaseGetCountLastest, NhapHangController);
        public async Task<int> GetCountLastest(LastestPagedPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGetCountLastest, ugToken, id);
            var banle = await ExecuteAsync<int>(request);

            return banle;
        }
        //api/NhapHang/GetPlan
        private readonly string ResourceGetPlan = string.Format("api/{0}/GetPlan", NhapHangController);
        public async Task<ObservableCollection<NhapHang>> GetPlan(ListFullNotDatePara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGetPlan, ugToken, id);
            var banle = await ExecuteAsync<ObservableCollection<NhapHang>>(request);

            return banle;
        }
        //api/NhapHang/GetCongNo
        private readonly string ResourceGetCongNo = string.Format("api/{0}/GetCongNo", NhapHangController);
        public async Task<ObservableCollection<NhapHang>> GetCongNo(ListFullNotDatePara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGetCongNo, ugToken, id);
            var banle = await ExecuteAsync<ObservableCollection<NhapHang>>(request);

            return banle;
        }
    }
}
