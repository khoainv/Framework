using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text;
using SSD.Mobile.Entities;
using SSD.Mobile.Agents;
using SSD.Mobile.Common.Para;

namespace SSD.Mobile.BusinessLogics
{
    public interface IThanhToanBiz
	{
        Task<ObservableCollection<NhapHang>> GetPagedOlder(PagedPara id);
	}

    public class ThanhToanBiz : BaseBiz<ThanhToanBiz>, IThanhToanBiz
	{
        public async Task<ObservableCollection<NhapHang>> GetPagedOlder(PagedPara id)
        {
            return await AgentRepository.Instance.ThanhToan.GetPagedOlder(id, UserBiz.Instance.UGToken);
        }
    }
}

