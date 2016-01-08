using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text;
using SSD.Mobile.Entities;
using SSD.Mobile.Agents;
using SSD.Mobile.Common.Para;

namespace SSD.Mobile.BusinessLogics
{
    public interface IBanSiTTBiz
	{
        Task<ObservableCollection<BanSiTT>> GetList(ListFullPara id);
	}

    public class BanSiTTBiz : BaseBiz<BanSiTTBiz>, IBanSiTTBiz
	{
        public async Task<ObservableCollection<BanSiTT>> GetList(ListFullPara id)
        {
            return await AgentRepository.Instance.BanSiTT.GetList(id, UserBiz.Instance.UGToken);
        }
    }
}

