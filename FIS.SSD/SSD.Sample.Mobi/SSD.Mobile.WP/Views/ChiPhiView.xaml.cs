using Cirrious.MvvmCross.WindowsCommon.Views;
using SSD.Mobile.BusinessLogics;
using SSD.Mobile.Share;
using System;

namespace SSD.Mobile.WP.Views
{
    public sealed partial class ChiPhiView : BaseMvxWindowsPage
    {
        public new ChiPhiViewModel ViewModel
        {
            get { return (ChiPhiViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
        public ChiPhiView()
        {
            this.InitializeComponent();
        }
        IncrementalSource<ChiPhiModel> ListModel;
        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ListModel = new IncrementalSource<ChiPhiModel>(ViewModel);
            ListViewChiPhi.ItemsSource = ListModel;
        }

        private void imgRefresh_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var l = ListModel;
        }
    }
}
