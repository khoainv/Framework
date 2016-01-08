using System.Drawing;

namespace SSD.Web.UI.Captcha
{
    /// <summary>
    /// Interface for implementing image Captcha.
    /// </summary>
    public interface IGenerateImage
    {
        Bitmap Generate(string captchaText);
    }
}
