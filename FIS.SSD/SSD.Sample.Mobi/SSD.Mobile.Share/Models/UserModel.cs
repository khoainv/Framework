using System;
using System.Collections.Generic;

namespace SSD.Mobile.Share
{
    public class UserModel : BaseModel
	{
        private string _UserName;

        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                SetProperty(ref _UserName, value);
            }
        }

        private string _Password;

        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                SetProperty(ref _Password, value);
            }
        }

	}
}

