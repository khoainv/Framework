using Cirrious.MvvmCross.ViewModels;

namespace SSD.Mobile.Share
{
    public class PagedBaseModel : BaseModel
    {
        public long OnID { get; set; }
    }
    public abstract class BaseModel : MvxNotifyPropertyChanged
	{
        
	}
}

