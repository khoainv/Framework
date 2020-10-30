using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsCommon.Platform;
using SSD.Framework;
using SSD.Mobile.Share;
using Windows.UI.Xaml.Controls;

namespace SSD.Mobile.WP
{
    public class Setup : MvxWindowsSetup
    {
        public Setup(Frame rootFrame) : base(rootFrame)
        {
        }
        private const string APIUri = "http://localhost:1733/";
        private const string IoTClientId = "919ba91e6589419395c1cb0e3bd5237c";
        private const string IoTClientSecret = "grABvcYypOU8a82tucxBdFikH";

        private const string AuthorityBaseUri = "http://localhost:1379";
        private const string ClientId = "ResourceOwner";
        private const string ClientSecret = "t+eG5XgsYU+L/KjKjUBehJN3a";
        protected override IMvxApplication CreateApp()
        {
            UGStartup.Register(APIUri, IoTClientId, IoTClientSecret, AuthorityBaseUri, ClientId, ClientSecret);
            return new SSD.Mobile.Share.App();
        }
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();
            Mvx.RegisterType<INetworkService, NetworkService>();
            Mvx.RegisterSingleton<IPlatform>(new WPPlatform());
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}