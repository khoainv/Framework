using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text;
using SSD.Mobile.Entities;
using SSD.Mobile.Agents;
using SSD.Mobile.Common.Para;

namespace SSD.Mobile.BusinessLogics
{
	public interface IBanLeBiz
	{
        Task<BanLe> Get(IDPara id);
        Task<ObservableCollection<BanLe>> GetPagedOlder(PagedPara id);
        Task<ObservableCollection<BanLe>> GetLastest(LastestPagedPara id);
        Task<int> GetCountLastest(LastestPagedPara id);
        Task<ObservableCollection<BanLe>> GetFullPagedOlder(PagedPara id);
        Task<ObservableCollection<BanLe>> GetFullLastest(LastestPagedPara id);
	}

    public class BanLeBiz : BaseBiz<BanLeBiz>, IBanLeBiz
	{
        public async Task<BanLe> Get(IDPara id)
        {
            return await AgentRepository.Instance.BanLe.Get(id,UserBiz.Instance.UGToken);
        }
        public async Task<ObservableCollection<BanLe>> GetPagedOlder(PagedPara id)
        {
            return await AgentRepository.Instance.BanLe.GetPagedOlder(id, UserBiz.Instance.UGToken);
        }
        public async Task<ObservableCollection<BanLe>> GetLastest(LastestPagedPara id)
        {
            return await AgentRepository.Instance.BanLe.GetLastest(id, UserBiz.Instance.UGToken);
        }
        public async Task<int> GetCountLastest(LastestPagedPara id)
        {
            return await AgentRepository.Instance.BanLe.GetCountLastest(id, UserBiz.Instance.UGToken);
        }
        public async Task<ObservableCollection<BanLe>> GetFullPagedOlder(PagedPara id)
        {
            return await AgentRepository.Instance.BanLe.GetFullPagedOlder(id, UserBiz.Instance.UGToken);
        }
        public async Task<ObservableCollection<BanLe>> GetFullLastest(LastestPagedPara id)
        {
            return await AgentRepository.Instance.BanLe.GetFullLastest(id, UserBiz.Instance.UGToken);
        }
    }
}

