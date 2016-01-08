using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Collections.ObjectModel;
using SSD.Mobile.Entities;
using System.Threading.Tasks;
using SSD.Mobile.BusinessLogics;
using SSD.Mobile.Common.Para;

namespace SSD.Mobile.Share
{
    public interface IPageSource<T> where T : PagedBaseModel
    {
        Task<ObservableCollection<T>> GetPagedOlder(long currrentOnID,int count);
        Task<ObservableCollection<T>> GetPagedOlder(PagedPara id);
    }
}

