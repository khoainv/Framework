using Cirrious.MvvmCross.WindowsCommon.Views;
using SSD.Mobile.BusinessLogics;
using SSD.Mobile.Share;
using System;

namespace SSD.Mobile.WP.Views
{
    public sealed partial class NhapHangCNView : BaseMvxWindowsPage
    {
        public new NhapHangCNViewModel ViewModel
        {
            get { return (NhapHangCNViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
        public NhapHangCNView()
        {
            this.InitializeComponent();
        }
        private void imgRefresh_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
        }
    }
}
