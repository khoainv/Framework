using Cirrious.MvvmCross.WindowsCommon.Views;
using SSD.Mobile.BusinessLogics;
using SSD.Mobile.Share;
using System;
using Windows.UI.Xaml.Input;

namespace SSD.Mobile.WP.Views
{
    public sealed partial class NhapHangCTView : BaseMvxWindowsPage
    {
        public new NhapHangCTViewModel ViewModel
        {
            get { return (NhapHangCTViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
        public NhapHangCTView()
        {
            this.InitializeComponent();
        }
    }
}
