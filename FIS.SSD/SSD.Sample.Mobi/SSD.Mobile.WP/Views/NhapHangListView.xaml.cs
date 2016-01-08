using Cirrious.MvvmCross.WindowsCommon.Views;
using SSD.Mobile.BusinessLogics;
using SSD.Mobile.Share;
using System;

namespace SSD.Mobile.WP.Views
{
    public sealed partial class NhapHangListView : BaseMvxWindowsPage
    {
        public new NhapHangListViewModel ViewModel
        {
            get { return (NhapHangListViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
        public NhapHangListView()
        {
            this.InitializeComponent();
        }
        IncrementalSource<NhapHangModel> ListModel;
        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ListModel = new IncrementalSource<NhapHangModel>(ViewModel);
            ListViewNhapHang.ItemsSource = ListModel;
        }

        private void imgRefresh_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var l = ListModel;
        }
    }
}
