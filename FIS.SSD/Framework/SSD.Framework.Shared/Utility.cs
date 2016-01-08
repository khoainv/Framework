
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SSD.Framework.Extensions;

namespace SSD.Framework
{
    
    public partial class Utility
    {
        public static bool SearchByTen(string ten, string textSearch)
        {
            if (string.IsNullOrWhiteSpace(ten))
                return false;
            if (string.IsNullOrWhiteSpace(textSearch))
                return true;
            ten = ten.ToLower();
            textSearch = textSearch.ToLower();
            var listOr = textSearch.Split(new char[] { ' ' });

            bool result = true;
            foreach (var txt in listOr)
                result = result && ten.Contains(txt);
            return result;
        }
        public static bool SearchByTenRemoveVietnameseSigns(string ten, string textSearch)
        {
            if (string.IsNullOrWhiteSpace(ten))
                return false;
            ten = ten.RemoveSign4Vietnamese();
            textSearch = textSearch.RemoveSign4Vietnamese();

            return SearchByTen(ten, textSearch);
        }
        public static long ParseLong(string tem)
        {
            try
            {
                return long.Parse(tem);
            }
            catch
            {
                return 0;
            }

        }
        public static int ParseInt(string tem)
        {
            try
            {
                return int.Parse(tem);
            }
            catch
            {
                return 0;
            }

        }
        public static double ParseDouble(string tem)
        {
            try
            {
                tem = tem.Replace("_", "");
                tem = tem.Replace(" ", "");
                tem = tem.Replace(",", "");
                double d = double.Parse(tem);
                return d;
            }
            catch
            {
                return 0;
            }
        }
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
        // Mã hóa Base64 với chuỗi Unicode.
        public static string StringToBase64(string src)
        {
            // Chuyển chuỗi thành mảng kiểu byte.  
            byte[] b = Encoding.Unicode.GetBytes(src);
            // Trả về chuỗi được mã hóa theo Base64.  
            return Convert.ToBase64String(b);

        }
        // Giải mã một chuỗi Unicode được mã hóa theo Base64.
        public static string Base64ToString(string src)
        {
            // Giải mã vào mảng kiểu byte.    
            byte[] b = Convert.FromBase64String(src);
            // Trả về chuỗi Unicode.  
            return Encoding.Unicode.GetString(b,0,b.Count());
        }
        private static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
        public static string ChuyenSo(string number)
        {
            string[] strTachPhanSauDauPhay;
            if (number.Contains(".") || number.Contains(","))
            {
                strTachPhanSauDauPhay = number.Split(',', '.');
                return (ChuyenSo(strTachPhanSauDauPhay[0]) + "phẩy " + ChuyenSo(strTachPhanSauDauPhay[1]));
            }

            string[] dv = { "", "mươi", "trăm", "nghìn", "triệu", "tỉ" };
            string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string doc;
            int i, j, k, n, len, found, ddv, rd;

            len = number.Length;
            number += "ss";
            doc = "";
            found = 0;
            ddv = 0;
            rd = 0;

            i = 0;
            while (i < len)
            {
                //So chu so o hang dang duyet
                n = (len - i + 2) % 3 + 1;

                //Kiem tra so 0
                found = 0;
                for (j = 0; j < n; j++)
                {
                    if (number[i + j] != '0')
                    {
                        found = 1;
                        break;
                    }
                }

                //Duyet n chu so
                if (found == 1)
                {
                    rd = 1;
                    for (j = 0; j < n; j++)
                    {
                        ddv = 1;
                        switch (number[i + j])
                        {
                            case '0':
                                if (n - j == 3) doc += cs[0] + " ";
                                if (n - j == 2)
                                {
                                    if (number[i + j + 1] != '0') doc += "linh ";
                                    ddv = 0;
                                }
                                break;
                            case '1':
                                if (n - j == 3) doc += cs[1] + " ";
                                if (n - j == 2)
                                {
                                    doc += "mười ";
                                    ddv = 0;
                                }
                                if (n - j == 1)
                                {
                                    if (i + j == 0) k = 0;
                                    else k = i + j - 1;

                                    if (number[k] != '1' && number[k] != '0')
                                        doc += "mốt ";
                                    else
                                        doc += cs[1] + " ";
                                }
                                break;
                            case '5':
                                if ((i + j == len - 1) || (i + j + 3 == len - 1))
                                    doc += "lăm ";
                                else
                                    doc += cs[5] + " ";
                                break;
                            default:
                                doc += cs[(int)number[i + j] - 48] + " ";
                                break;
                        }

                        //Doc don vi nho
                        if (ddv == 1)
                        {
                            doc += ((n - j) != 1) ? dv[n - j - 1] + " " : dv[n - j - 1];
                        }
                    }
                }


                //Doc don vi lon
                if (len - i - n > 0)
                {
                    if ((len - i - n) % 9 == 0)
                    {
                        if (rd == 1)
                            for (k = 0; k < (len - i - n) / 9; k++)
                                doc += "tỉ ";
                        rd = 0;
                    }
                    else
                        if (found != 0) doc += dv[((len - i - n + 1) % 9) / 3 + 2] + " ";
                }

                i += n;
            }

            if (len == 1)
                if (number[0] == '0' || number[0] == '5') return cs[(int)number[0] - 48];

            return UppercaseFirst(doc + "đồng chẵn.");
        }
    }
    public class ConvertObject : Singleton<ConvertObject>
    {
        public int ConvertToInt(Object val)
        {
            if (val == null)
            {
                return 0;
            }
            return System.Convert.ToInt32(val);
        }
    }
    public class ConvertString : Singleton<ConvertString>
    {
        public const string IGNORED_STRING_VALUE = "-";

        #region Vietnamese Symbol
        public string ConvertToMarkUrl(string notNormalUrl)
        {
            return ConvertToMarkUrl(notNormalUrl, IGNORED_STRING_VALUE);
        }
        public string ConvertToMarkUrl(string notNormalUrl, string ignoredStringValue)
        {
            string result = string.Empty;
            char[] illegalcharacters = { '#', '%', '&', '*', '{', '}', '\'', '\"', ':', ';', '<', '>', '?', '/', '+', '.', ',', (char)32, (char)34 };
            string[] dupplicatestring = { string.Concat(ignoredStringValue, ignoredStringValue, ignoredStringValue), string.Concat(ignoredStringValue, ignoredStringValue) };

            if (!string.IsNullOrEmpty(notNormalUrl))
            {
                //Chuyển TV có dấu sang TV ko dấu >>>
                notNormalUrl = ConvertToNoMark(notNormalUrl);

                foreach (char chr in illegalcharacters)
                {
                    if (notNormalUrl != null)
                    {
                        notNormalUrl = notNormalUrl.Replace(chr, ignoredStringValue.ToCharArray()[0]);
                    }
                }

                foreach (string str in dupplicatestring)
                {
                    if (notNormalUrl != null)
                        notNormalUrl = notNormalUrl.Replace(str, ignoredStringValue);
                }

                result = notNormalUrl;
            }
            return notNormalUrl;
        }

        public string ConvertToNoMark(string str)
        {
            string output = string.Empty;
            string[,] arrMang = new string[14, 18];

            string strChuoi = "aAeEoOuUiIdDyY";
            string Thga, Thge, Thgo, Thgu, Thgi, Thgd, Thgy;
            string HoaA, HoaE, HoaO, HoaU, HoaI, HoaD, HoaY;
            Thga = "áàạảãâấầậẩẫăắằặẳẵ";
            HoaA = "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ";
            Thge = "éèẹẻẽêếềệểễeeeeee";
            HoaE = "ÉÈẸẺẼÊẾỀỆỂỄEEEEEE";
            Thgo = "óòọỏõôốồộổỗơớờợởỡ";
            HoaO = "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ";
            Thgu = "úùụủũưứừựửữuuuuuu";
            HoaU = "ÚÙỤỦŨƯỨỪỰỬỮUUUUUU";
            Thgi = "íìịỉĩiiiiiiiiiiii";
            HoaI = "ÍÌỊỈĨIIIIIIIIIIII";
            Thgd = "đdddddddddddddddd";
            HoaD = "ĐDDDDDDDDDDDDDDDD";
            Thgy = "ýỳỵỷỹyyyyyyyyyyyy";
            HoaY = "ÝỲỴỶỸYYYYYYYYYYYY";
            for (int i = 0; i <= 13; i++)
            {
                arrMang[i, 0] = strChuoi.Substring(i, 1);
            }
            for (int j = 1; j <= 17; j++)
            {
                for (int i = 1; i <= 17; i++)
                {
                    arrMang[0, i] = Thga.Substring(i - 1, 1);
                    arrMang[1, i] = HoaA.Substring(i - 1, 1);
                    arrMang[2, i] = Thge.Substring(i - 1, 1);
                    arrMang[3, i] = HoaE.Substring(i - 1, 1);
                    arrMang[4, i] = Thgo.Substring(i - 1, 1);
                    arrMang[5, i] = HoaO.Substring(i - 1, 1);
                    arrMang[6, i] = Thgu.Substring(i - 1, 1);
                    arrMang[7, i] = HoaU.Substring(i - 1, 1);
                    arrMang[8, i] = Thgi.Substring(i - 1, 1);
                    arrMang[9, i] = HoaI.Substring(i - 1, 1);
                    arrMang[10, i] = Thgd.Substring(i - 1, 1);
                    arrMang[11, i] = HoaD.Substring(i - 1, 1);
                    arrMang[12, i] = Thgy.Substring(i - 1, 1);
                    arrMang[13, i] = HoaY.Substring(i - 1, 1);
                }
            }

            for (int i = 0; i <= 13; i++)
            {
                for (int j = 1; j <= 17; j++)
                {
                    output = str.Replace(arrMang[i, j], arrMang[i, 0]);
                    str = output;
                }
            }
            return str;
        }
        #endregion
    }
}