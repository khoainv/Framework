using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.Mvc.Ajax;

namespace SSD.Web.UI.Captcha
{
    public static class CaptchaHelper
    {
        private const string CaptchaFormat = @"<img id=""CaptchaImage"" src=""{0}""/>{1}<br/>";

        #region Working with the captcha

        /// <summary>
        /// Create full captcha
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        internal static MvcHtmlString GenerateFullCaptcha(HtmlHelper htmlHelper, int length)
        {
            var encryptorModel = GetEncryptorModel();
            var captchaText = RandomText.Generate(length);
            var encryptText = Encryption.Encrypt(captchaText, encryptorModel.Password, encryptorModel.Salt);

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var url = urlHelper.Action("Create", "CaptchaImage", new { encryptText});

            var ajax = new AjaxHelper(htmlHelper.ViewContext, htmlHelper.ViewDataContainer);
            var refresh = ajax.ActionLink("Refresh", "NewCaptcha", "CaptchaImage", new {l = length},
                                          new AjaxOptions { UpdateTargetId = "CaptchaDeText", OnSuccess = "Success" });

            return MvcHtmlString.Create(string.Format(CaptchaFormat, url, htmlHelper.Hidden("CaptchaDeText", encryptText)) +
                                         refresh.ToHtmlString() + " <br/>Enter symbol: " +
                                        htmlHelper.TextBox("CaptchaInputText"));
        }

        /// <summary>
        /// Create partial captcha
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        internal static RefreshModel GeneratePartialCaptcha(RequestContext requestContext, int length)
        {
            var encryptorModel = GetEncryptorModel();
            var captchaText = RandomText.Generate(length);
            var encryptText = Encryption.Encrypt(captchaText, encryptorModel.Password, encryptorModel.Salt);

            var urlHelper = new UrlHelper(requestContext);
            var url = urlHelper.Action("Create", "CaptchaImage", new { encryptText });


            return new RefreshModel() { Code = encryptText, Image = url };

        }

        /// <summary>
        /// Check for proper input captcha
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Verify(CaptchaModel model)
        {
            try
            {
                var encryptorModel = GetEncryptorModel();

                if (encryptorModel == null)
                {
                    return false;
                }

                var textDecrypt = Encryption.Decrypt(model.CaptchaDeText, encryptorModel.Password, encryptorModel.Salt);
                return textDecrypt == model.CaptchaInputText;
            }
            catch
            {

                return false;
            }
           
        }

        /// <summary>
        /// Returns the model for decoding from the web.config
        /// </summary>
        /// <returns></returns>
        internal static EncryptorModel GetEncryptorModel()
        {
            var pass = ConfigurationManager.AppSettings["CaptchaPass"];
            var salt = ConfigurationManager.AppSettings["CaptchaSalt"];
            if ((string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(salt)))
                throw new ConfigurationErrorsException("In the web.config file, there are no options for Captcha.");
            try
            {
                var encryptorModel = new EncryptorModel() { Password = pass, Salt = Convert.FromBase64String(salt) };
                return encryptorModel;
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Returns the implementation IGenerateImage custom or default.
        /// </summary>
        /// <returns></returns>
        internal static IGenerateImage GetGenerateImage()
        {
            var nameType = ConfigurationManager.AppSettings["CaptchaIGenerate"];
            
            if (!string.IsNullOrEmpty(nameType))
            {
                var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                Type typeImage = null;
                foreach (var assembly in allAssemblies.Where(assembl => !assembl.FullName.Contains("System")))
                {
                    typeImage = (from type in assembly.GetTypes()
                                where type.IsClass &&
                                      (type.GetInterface("IGenerateImage") != null) && type.FullName == nameType
                                select type).FirstOrDefault();
                    if (typeImage != null)
                        break;
                }

                if (typeImage != null)
                {
                    var result = (IGenerateImage) typeImage.Assembly.CreateInstance(typeImage.FullName, true);
                    return result;
                }
            }

            return new GenerateImage();
        }
        #endregion

    }
}
