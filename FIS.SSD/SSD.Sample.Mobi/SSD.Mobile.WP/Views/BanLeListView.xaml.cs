using Cirrious.MvvmCross.WindowsCommon.Views;
using SSD.Mobile.BusinessLogics;
using SSD.Mobile.Share;
using System;

namespace SSD.Mobile.WP.Views
{
    public sealed partial class BanLeListView : BaseMvxWindowsPage
    {
        public new BanLeListViewModel ViewModel
        {
            get { return (BanLeListViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
        public BanLeListView()
        {
            this.InitializeComponent();
        }
        IncrementalSource<BanLeModel> ListModel;
        protected override void LoadDataToList()
        {
            ListModel = new IncrementalSource<BanLeModel>(ViewModel);
            ListViewBanLe.ItemsSource = ListModel;
        }

        private void imgRefresh_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var l = ListModel;
        }
    }
}
