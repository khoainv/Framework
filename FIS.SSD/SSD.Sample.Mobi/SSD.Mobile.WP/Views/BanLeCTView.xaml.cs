using Cirrious.MvvmCross.WindowsCommon.Views;
using SSD.Mobile.BusinessLogics;
using SSD.Mobile.Share;
using System;
using Windows.UI.Xaml.Input;


namespace SSD.Mobile.WP.Views
{
    public sealed partial class BanLeCTView : BaseMvxWindowsPage
    {
        public new BanLeCTViewModel ViewModel
        {
            get { return (BanLeCTViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
        public BanLeCTView()
        {
            this.InitializeComponent();
        }
    }
}
