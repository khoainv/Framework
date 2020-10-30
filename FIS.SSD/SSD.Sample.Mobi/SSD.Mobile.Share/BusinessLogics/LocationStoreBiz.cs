using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text;
using SSD.Mobile.Entities;
using SSD.Mobile.Agents;

namespace SSD.Mobile.BusinessLogics
{
    public interface ILocationStoreBiz
	{
        Task<ObservableCollection<ComLocationStore>> GetAll();
	}

    public class LocationStoreBiz : BaseBiz<LocationStoreBiz>, ILocationStoreBiz
	{
        public async Task<ObservableCollection<ComLocationStore>> GetAll()
        {
            return await AgentRepository.Instance.LocationStore.GetAll(UserBiz.Instance.UGToken);
        }
    }
}

