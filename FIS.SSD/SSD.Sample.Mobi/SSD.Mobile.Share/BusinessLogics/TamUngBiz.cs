using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text;
using SSD.Mobile.Entities;
using SSD.Mobile.Agents;
using SSD.Mobile.Common.Para;

namespace SSD.Mobile.BusinessLogics
{
    public interface ITamUngBiz
	{
        Task<TamUng> Get(IDPara id);
        Task<ObservableCollection<TamUng>> GetPagedOlder(PagedPara id);
        Task<ObservableCollection<TamUng>> GetLastest(LastestPagedPara id);
        Task<int> GetCountLastest(LastestPagedPara id);
	}

    public class TamUngBiz : BaseBiz<TamUngBiz>, ITamUngBiz
	{
        public async Task<TamUng> Get(IDPara id)
        {
            return await AgentRepository.Instance.TamUng.Get(id, UserBiz.Instance.UGToken);
        }
        public async Task<ObservableCollection<TamUng>> GetPagedOlder(PagedPara id)
        {
            return await AgentRepository.Instance.TamUng.GetPagedOlder(id, UserBiz.Instance.UGToken);
        }
        public async Task<ObservableCollection<TamUng>> GetLastest(LastestPagedPara id)
        {
            return await AgentRepository.Instance.TamUng.GetLastest(id, UserBiz.Instance.UGToken);
        }
        public async Task<int> GetCountLastest(LastestPagedPara id)
        {
            return await AgentRepository.Instance.TamUng.GetCountLastest(id, UserBiz.Instance.UGToken);
        }
    }
}

