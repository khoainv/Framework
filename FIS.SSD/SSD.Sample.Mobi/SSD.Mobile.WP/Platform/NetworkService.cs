using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using SSD.Mobile.Share;
using Windows.Networking.Connectivity;

namespace SSD.Mobile.WP
{
    public class NetworkService : INetworkService
    {
        public bool TryReach(string uri)
        {
            throw new NotImplementedException();
        }

        public bool IsNetworkAvailable()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                ConnectionProfile connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                return (connectionProfile != null && connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);
            }
            return true;
        }


        public string MacAddress()
        {
            return "12345";
            //var networkProfiles = Windows.Networking.Connectivity.NetworkInformation.GetConnectionProfiles();
            //var adapter = networkProfiles.First<Windows.Networking.Connectivity.ConnectionProfile>().NetworkAdapter;//takes the first network adapter
            //return adapter.NetworkAdapterId.ToString();//produces a string in the format: 90de0377-d988-4e1b-b89b-475bbca46e1d
        }
    }
}

