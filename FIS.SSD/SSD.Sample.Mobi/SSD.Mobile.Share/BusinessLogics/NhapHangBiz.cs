using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text;
using SSD.Mobile.Entities;
using SSD.Mobile.Agents;
using SSD.Mobile.Common.Para;

namespace SSD.Mobile.BusinessLogics
{
    public interface INhapHangBiz
	{
        Task<NhapHang> Get(IDPara id);
        Task<ObservableCollection<NhapHang>> GetPagedOlder(PagedPara id);
        Task<ObservableCollection<NhapHang>> GetLastest(LastestPagedPara id);
        Task<int> GetCountLastest(LastestPagedPara id);
        Task<ObservableCollection<NhapHang>> GetPlan(ListFullNotDatePara id);
        Task<ObservableCollection<NhapHang>> GetCongNo(ListFullNotDatePara id);
	}

    public class NhapHangBiz : BaseBiz<NhapHangBiz>, INhapHangBiz
	{
        public async Task<NhapHang> Get(IDPara id)
        {
            return await AgentRepository.Instance.NhapHang.Get(id, UserBiz.Instance.UGToken);
        }
        public async Task<ObservableCollection<NhapHang>> GetPagedOlder(PagedPara id)
        {
            return await AgentRepository.Instance.NhapHang.GetPagedOlder(id, UserBiz.Instance.UGToken);
        }
        public async Task<ObservableCollection<NhapHang>> GetLastest(LastestPagedPara id)
        {
            return await AgentRepository.Instance.NhapHang.GetLastest(id, UserBiz.Instance.UGToken);
        }
        public async Task<int> GetCountLastest(LastestPagedPara id)
        {
            return await AgentRepository.Instance.NhapHang.GetCountLastest(id, UserBiz.Instance.UGToken);
        }
        public async Task<ObservableCollection<NhapHang>> GetPlan(ListFullNotDatePara id)
        {
            return await AgentRepository.Instance.NhapHang.GetPlan(id, UserBiz.Instance.UGToken);
        }
        public async Task<ObservableCollection<NhapHang>> GetCongNo(ListFullNotDatePara id)
        {
            return await AgentRepository.Instance.NhapHang.GetCongNo(id, UserBiz.Instance.UGToken);
        }
    }
}

