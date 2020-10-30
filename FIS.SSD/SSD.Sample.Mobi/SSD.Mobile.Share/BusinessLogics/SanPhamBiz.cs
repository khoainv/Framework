using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text;
using SSD.Mobile.Entities;
using SSD.Mobile.Agents;
using SSD.Mobile.Common.Para;

namespace SSD.Mobile.BusinessLogics
{
    public interface ISanPhamBiz
	{
        Task<DMSanPham> Get(IDPara id);
        Task<ObservableCollection<DMSanPham>> GetPagedOlder(SearchPagedPara id);
	}

    public class SanPhamBiz : BaseBiz<SanPhamBiz>, ISanPhamBiz
	{
        public async Task<DMSanPham> Get(IDPara id)
        {
            return await AgentRepository.Instance.SanPham.Get(id, UserBiz.Instance.UGToken);
        }
        public async Task<ObservableCollection<DMSanPham>> GetPagedOlder(SearchPagedPara id)
        {
            return await AgentRepository.Instance.SanPham.GetPagedOlder(id, UserBiz.Instance.UGToken);
        }
    }
}

