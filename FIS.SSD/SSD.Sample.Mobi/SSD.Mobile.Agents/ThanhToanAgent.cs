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
    public partial class ThanhToanAgent : UGBaseAgent<ThanhToanAgent>
    {
        private const string ThanhToanController = "ThanhToan";
        //api/ThanhToan/GetPagedOlder
        private readonly string ResourceGetPagedOlder = string.Format(ResourceBaseGetPagedOlder, ThanhToanController);
        public async Task<ObservableCollection<NhapHang>> GetPagedOlder(PagedPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGetPagedOlder, ugToken, id);
            var banle = await ExecuteAsync<ObservableCollection<NhapHang>>(request);

            return banle;
        }
    }
}
