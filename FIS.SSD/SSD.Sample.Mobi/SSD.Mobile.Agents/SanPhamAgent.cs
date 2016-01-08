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
    public partial class SanPhamAgent : UGBaseAgent<SanPhamAgent>
    {
        private const string SanPhamController = "SanPham";
        //api/SanPham/Get
        private readonly string ResourceGet = string.Format(ResourceBaseGet, SanPhamController);
        public async Task<DMSanPham> Get(IDPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGet, ugToken, id);
            var banle = await ExecuteAsync<DMSanPham>(request);

            return banle;
        }
        //api/SanPham/GetPagedOlder
        private readonly string ResourceGetPagedOlder = string.Format(ResourceBaseGetPagedOlder, SanPhamController);
        public async Task<ObservableCollection<DMSanPham>> GetPagedOlder(SearchPagedPara id, string ugToken)
        {
            RequestBase request = new RequestBase(ResourceGetPagedOlder, ugToken, id);
            var banle = await ExecuteAsync<ObservableCollection<DMSanPham>>(request);

            return banle;
        }
    }
}
