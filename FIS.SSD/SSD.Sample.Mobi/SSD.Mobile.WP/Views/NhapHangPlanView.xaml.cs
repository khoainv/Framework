using Cirrious.MvvmCross.WindowsCommon.Views;
using SSD.Mobile.BusinessLogics;
using SSD.Mobile.Share;
using System;

namespace SSD.Mobile.WP.Views
{
    public sealed partial class NhapHangPlanView : BaseMvxWindowsPage
    {
        public new NhapHangPlanViewModel ViewModel
        {
            get { return (NhapHangPlanViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
        public NhapHangPlanView()
        {
            this.InitializeComponent();
        }
        
        private void imgRefresh_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
        }
    }
}
