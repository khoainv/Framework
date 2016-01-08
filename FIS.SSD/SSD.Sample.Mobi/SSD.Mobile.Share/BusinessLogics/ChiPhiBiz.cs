using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text;
using SSD.Mobile.Entities;
using SSD.Mobile.Agents;
using SSD.Mobile.Common.Para;

namespace SSD.Mobile.BusinessLogics
{
    public interface IChiPhiBiz
	{
        Task<ChiPhi> Get(IDPara id);
        Task<ObservableCollection<ChiPhi>> GetPagedOlder(PagedPara id);
        Task<ObservableCollection<ChiPhi>> GetLastest(LastestPagedPara id);
        Task<int> GetCountLastest(LastestPagedPara id);
	}

    public class ChiPhiBiz : BaseBiz<ChiPhiBiz>, IChiPhiBiz
	{
        public async Task<ChiPhi> Get(IDPara id)
        {
            return await AgentRepository.Instance.ChiPhi.Get(id, UserBiz.Instance.UGToken);
        }
        public async Task<ObservableCollection<ChiPhi>> GetPagedOlder(PagedPara id)
        {
            return await AgentRepository.Instance.ChiPhi.GetPagedOlder(id, UserBiz.Instance.UGToken);
        }
        public async Task<ObservableCollection<ChiPhi>> GetLastest(LastestPagedPara id)
        {
            return await AgentRepository.Instance.ChiPhi.GetLastest(id, UserBiz.Instance.UGToken);
        }
        public async Task<int> GetCountLastest(LastestPagedPara id)
        {
            return await AgentRepository.Instance.ChiPhi.GetCountLastest(id, UserBiz.Instance.UGToken);
        }
    }
}

