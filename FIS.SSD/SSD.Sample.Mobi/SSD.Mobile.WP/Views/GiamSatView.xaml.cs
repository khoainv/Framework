using Cirrious.MvvmCross.WindowsCommon.Views;
using SSD.Mobile.BusinessLogics;
using SSD.Mobile.Share;
using System;
using Windows.UI.Xaml.Input;

namespace SSD.Mobile.WP.Views
{
    public sealed partial class GiamSatView : BaseMvxWindowsPage
    {
        public new GiamSatViewModel ViewModel
        {
            get { return (GiamSatViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
        //int y1, y2;
        int x1, x2;
        public GiamSatView()
        {
            this.InitializeComponent();

            //ListViewGiamSat.ManipulationMode =  ManipulationModes.TranslateY | ManipulationModes.TranslateX;
            //ListViewGiamSat.ManipulationStarted += (s, e) => y1 = (int)e.Position.Y;
            //ListViewGiamSat.ManipulationCompleted += (s, e) =>
            //{
            //    y2 = (int)e.Position.Y;
            //    if (y1 > y2)
            //    {
            //        DisplayAlert("swipe down", "swipe down", AlertButton.Close);
            //    };
            //    if (y1 < y2)
            //    {
            //        DisplayAlert("swipe up", "swipe up", AlertButton.Close);
            //    };
            //};

            this.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            this.ManipulationStarted += (s, e) => x1 = (int)e.Position.X;
            this.ManipulationCompleted += (s, e) =>
            {
                x2 = (int)e.Position.X;
                if (x1 > x2)
                {
                    DisplayAlert("swipe left", "swipe left", AlertButton.Close);
                };
                if (x1 < x2)
                {
                    DisplayAlert("swipe right", "swipe right", AlertButton.Close);
                };
            };
        }
        IncrementalSource<BanLeModel> ListModel;
        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ListModel = new IncrementalSource<BanLeModel>(ViewModel);
            ListViewGiamSat.ItemsSource = ListModel;
        }

        private void imgRefresh_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var l = ListModel;
        }
    }
}
