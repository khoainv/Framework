using Cirrious.MvvmCross.WindowsCommon.Views;
using SSD.Mobile.BusinessLogics;
using SSD.Mobile.Share;
using System;

namespace SSD.Mobile.WP.Views
{
    public sealed partial class ThuKhacView : BaseMvxWindowsPage
    {
        public new ThuKhacViewModel ViewModel
        {
            get { return (ThuKhacViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
        public ThuKhacView()
        {
            this.InitializeComponent();
        }
        IncrementalSource<ThuKhacModel> ListModel;
        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ListModel = new IncrementalSource<ThuKhacModel>(ViewModel);
            ListViewChiPhi.ItemsSource = ListModel;
        }

        private void imgRefresh_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var l = ListModel;
        }
    }
}
