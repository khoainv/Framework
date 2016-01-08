using System;
using System.Text;

namespace SSD.Web.UI.Captcha
{
    internal static class RandomText
    {
        /// <summary>
        /// Generates characters for captcha
        /// </summary>
        /// <param name="count">Number of characters</param>
        /// <returns></returns>
        public static string Generate(int count)
        {
            const string chars = "qwertyuipasdfghjklzcvbnmQWERTYUIOPASDFGHJKLZXCVBNM123456789";
            var output = new StringBuilder(4);

            var lenght = RandomNumber.Next(count, count);

            for (int i = 0; i < lenght; i++)
            {
                var randomIndex = RandomNumber.Next(chars.Length - 1);
                output.Append(chars[randomIndex]);
            }

            return output.ToString();
        }

        /// <summary>
        /// Generates Salt for captcha
        /// </summary>
        /// <param name="count">Number of characters</param>
        /// <returns></returns>
        public static string GenerateSalt(int count)
        {
            return Convert.ToBase64String(Encoding.Unicode.GetBytes(Generate(count)));
        }
    }
}
