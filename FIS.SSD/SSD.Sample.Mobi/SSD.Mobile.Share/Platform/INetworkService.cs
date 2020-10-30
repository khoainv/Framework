using System;

namespace SSD.Mobile.Share
{
	public interface INetworkService
	{
		bool TryReach(string uri);

		bool IsNetworkAvailable();

        string MacAddress();
	}
}

