// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using SSD.Framework.Cryptography;

namespace SSD.Framework.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants
        private const string UrlBaseKey = "UrlBaseKey";
        private const string AuthorityBaseKey = "AuthorityBaseKey";
        private const string ClientIdKey = "ClientIdKey";
        private const string ClientSecretKey = "ClientSecretKey";
        private const string IoTClientIdKey = UGConstants.HTTPHeaders.IOT_CLIENT_ID;
        private const string IoTClientSecretKey = UGConstants.HTTPHeaders.IOT_CLIENT_SECRET;
        private const string UGTokenKey = UGConstants.HTTPHeaders.TOKEN_NAME;
        private const string SaltKey = "SaltKey";
        private const string UserNameKey = "UserNameKey";
        private const string PasswordKey = "PasswordKey";
        private static readonly string SettingsDefault = string.Empty;
        private const string IsUsingSSOKey = "IsUsingSSOKey";
        private const string ProfileJsonKey = "ProfileJsonKey";

        #endregion

        public static string ProfileJson
        {
            get
            {
                return AppSettings.GetValueOrDefault(ProfileJsonKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ProfileJsonKey, value);
            }
        }

        public static bool IsUsingSSO
        {
            get
            {
                return AppSettings.GetValueOrDefault(IsUsingSSOKey, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(IsUsingSSOKey, value);
            }
        }
        public static string UrlBase
        {
            get
            {
                return AppSettings.GetValueOrDefault(UrlBaseKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UrlBaseKey, value);
            }
        }
        public static string AuthorityBase
        {
            get
            {
                return AppSettings.GetValueOrDefault(AuthorityBaseKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AuthorityBaseKey, value);
            }
        }
        public static string SSOClientId
        {
            get
            {
                return AppSettings.GetValueOrDefault(ClientIdKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ClientIdKey, value);
            }
        }
        public static string SSOClientSecret
        {
            get
            {
                var cipherText = AppSettings.GetValueOrDefault(ClientSecretKey, SettingsDefault);

                if (string.IsNullOrWhiteSpace(cipherText) || string.IsNullOrWhiteSpace(Salt))
                    return string.Empty;

                return AESEngine.Decryption(cipherText, Salt);
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (string.IsNullOrWhiteSpace(Salt))
                        Salt = AESEngine.CreateSalt();

                    var text = AESEngine.Encryption(value, Salt);

                    AppSettings.AddOrUpdateValue(ClientSecretKey, text);
                }
                else AppSettings.AddOrUpdateValue(ClientSecretKey, value);
            }
        }
        public static string IoTClientId
        {
            get
            {
                return AppSettings.GetValueOrDefault(IoTClientIdKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(IoTClientIdKey, value);
            }
        }
        public static string IoTClientSecret
        {
            get
            {
                var cipherText = AppSettings.GetValueOrDefault(IoTClientSecretKey, SettingsDefault);

                if (string.IsNullOrWhiteSpace(cipherText) || string.IsNullOrWhiteSpace(Salt))
                    return string.Empty;

                return AESEngine.Decryption(cipherText, Salt);
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (string.IsNullOrWhiteSpace(Salt))
                        Salt = AESEngine.CreateSalt();

                    var text = AESEngine.Encryption(value, Salt);

                    AppSettings.AddOrUpdateValue(IoTClientSecretKey, text);
                }
                else AppSettings.AddOrUpdateValue(IoTClientSecretKey, value);
            }
        }
        public static string UserName
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserNameKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserNameKey, value);
            }
        }
        public static string Password
        {
            get
            {
                var cipherText = AppSettings.GetValueOrDefault(PasswordKey, SettingsDefault);

                if (string.IsNullOrWhiteSpace(cipherText) || string.IsNullOrWhiteSpace(Salt))
                    return string.Empty;

                return AESEngine.Decryption(cipherText, Salt);
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (string.IsNullOrWhiteSpace(Salt))
                        Salt = AESEngine.CreateSalt();

                    var text = AESEngine.Encryption(value, Salt);

                    AppSettings.AddOrUpdateValue(PasswordKey, text);
                }
                else AppSettings.AddOrUpdateValue(PasswordKey, value);
            }
        }
        private static string Salt
        {
            get
            {
                return AppSettings.GetValueOrDefault(SaltKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SaltKey, value);
            }
        }
        public static string AccessToken
        {
            get
            {
                var cipherText = AppSettings.GetValueOrDefault(UGTokenKey, SettingsDefault);

                if (string.IsNullOrWhiteSpace(cipherText) || string.IsNullOrWhiteSpace(Salt))
                    return string.Empty;

                return AESEngine.Decryption(cipherText, Salt);
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (string.IsNullOrWhiteSpace(Salt))
                        Salt = AESEngine.CreateSalt();

                    var text = AESEngine.Encryption(value, Salt);

                    AppSettings.AddOrUpdateValue(UGTokenKey, text);
                }
                else AppSettings.AddOrUpdateValue(UGTokenKey, value);
            }
        }

    }
}