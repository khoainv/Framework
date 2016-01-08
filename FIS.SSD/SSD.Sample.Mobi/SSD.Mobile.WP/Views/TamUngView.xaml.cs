using Cirrious.MvvmCross.WindowsCommon.Views;
using SSD.Mobile.BusinessLogics;
using SSD.Mobile.Share;
using System;

namespace SSD.Mobile.WP.Views
{
    public sealed partial class TamUngView : BaseMvxWindowsPage
    {
        public new TamUngViewModel ViewModel
        {
            get { return (TamUngViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
        public TamUngView()
        {
            this.InitializeComponent();
        }
        IncrementalSource<TamUngModel> ListModel;
        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ListModel = new IncrementalSource<TamUngModel>(ViewModel);
            ListViewChiPhi.ItemsSource = ListModel;
        }

        private void imgRefresh_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var l = ListModel;
        }
    }
}
