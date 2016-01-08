using Cirrious.MvvmCross.WindowsCommon.Views;
using SSD.Mobile.Common;
using SSD.Mobile.Share;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using SSD.Framework;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace SSD.Mobile.WP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginView : BaseMvxWindowsPage
    {
        public new LoginViewModel ViewModel
        {
            get { return (LoginViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
        private const string USER_NAME = "User name";
        // Constructor
        public LoginView()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (ViewModel != null)
            {
                if (!string.IsNullOrWhiteSpace( SSD.Mobile.BusinessLogics.UserBiz.Instance.UGToken))
                {
                    ContentPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    //this.Frame.Navigate(typeof(HomeView));
                    //if (ViewModel.IsBusy)
                    //{
                        
                    //}
                    //else ContentPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    //txtPassword_LostFocus(null, null);
                }
            }
        }
        private void txtUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox obj = sender as TextBox;
            if (obj.Text == USER_NAME)
            {
                obj.Text = string.Empty;
            }
        }

        private void txtUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox obj = sender as TextBox;
            if (obj.Text == String.Empty)
            {
                obj.Text = USER_NAME;
            }
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordWatermark.Visibility = Visibility.Collapsed;
        }

        private void txtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            PasswordWatermark.Visibility = txtPassword.Password.Length > 0
                                    ? Visibility.Collapsed
                                    : Visibility.Visible;
        }

        private void txtUsername_Loaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Model != null && string.IsNullOrWhiteSpace(ViewModel.Model.UserName))
                txtUsername.Text = USER_NAME;
        }
        private void PasswordWatermark_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PasswordWatermark.Visibility = Visibility.Collapsed;
            txtPassword.Focus(FocusState.Keyboard);
        }
    }
}
