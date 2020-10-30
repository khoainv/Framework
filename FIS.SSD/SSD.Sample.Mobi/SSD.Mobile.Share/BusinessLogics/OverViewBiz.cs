using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text;
using SSD.Mobile.Entities;
using SSD.Mobile.Agents;
using SSD.Mobile.Common.Para;

namespace SSD.Mobile.BusinessLogics
{
    public interface IOverViewBiz
	{
        Task<OverView> Get(ListFullPara id);
	}

    public class OverViewBiz : BaseBiz<OverViewBiz>, IOverViewBiz
	{
        public async Task<OverView> Get(ListFullPara id)
        {
            return await AgentRepository.Instance.OverView.Get(id, UserBiz.Instance.UGToken);
        }
    }
}

