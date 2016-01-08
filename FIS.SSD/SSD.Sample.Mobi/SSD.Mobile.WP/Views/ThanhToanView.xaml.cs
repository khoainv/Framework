using Cirrious.MvvmCross.WindowsCommon.Views;
using SSD.Mobile.BusinessLogics;
using SSD.Mobile.Share;
using System;

namespace SSD.Mobile.WP.Views
{
    public sealed partial class ThanhToanView : BaseMvxWindowsPage
    {
        public new ThanhToanViewModel ViewModel
        {
            get { return (ThanhToanViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
        public ThanhToanView()
        {
            this.InitializeComponent();
        }
        IncrementalSource<NhapHangModel> ListModel;
        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ListModel = new IncrementalSource<NhapHangModel>(ViewModel);
            ListViewThanhToan.ItemsSource = ListModel;
        }

        private void imgRefresh_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var l = ListModel;
        }
    }
}
