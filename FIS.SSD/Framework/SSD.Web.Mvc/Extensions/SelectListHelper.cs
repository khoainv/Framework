using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using SSD.Framework.Extensions;

namespace SSD.Web.Mvc.Extensions
{
    public static class SelectListHelper
    {
        /// <summary>
        /// Description: To the select list.
        /// </summary>
        /// <typeparam name="TEnum">The type of the t enum.</typeparam>
        /// <param name="enumObj">The enum object.</param>
        /// <param name="defaultText">The default text.</param>
        /// <param name="markCurrentAsSelected">if set to <c>true</c> [mark current as selected].</param>
        /// <param name="hasDefault">if set to <c>true</c> [has default].</param>
        /// <returns>SelectList.</returns>
        /// <exception cref="System.ArgumentException">An Enumeration type is required.;enumObj</exception>
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj, string defaultText = "", bool markCurrentAsSelected = true, bool hasDefault = true) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) throw new ArgumentException(@"An Enumeration type is required.", "enumObj");

            var values = (from TEnum enumValue in Enum.GetValues(typeof(TEnum))
                          select new
                          {
                              Value = Convert.ToInt32(enumValue).ToString(),
                              Text = enumValue.Description()
                          }).ToList();
            if (hasDefault)
            {
                values.Insert(0, new
                    {
                        Value = "",
                        Text = defaultText
                    });
            }

            if (markCurrentAsSelected)
            {
                object selectedValue = Convert.ToInt32(enumObj).ToString();
                return new SelectList(values, "Value", "Text", selectedValue);
            }
            return new SelectList(values, "Value", "Text");
        }

        /// <summary>
        /// Description: To the select list.
        /// </summary>
        /// <typeparam name="TEnum">The type of the t enum.</typeparam>
        /// <param name="enumObj">The enum object.</param>
        /// <param name="defaultText">The default text.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="markCurrentAsSelected">if set to <c>true</c> [mark current as selected].</param>
        /// <returns>SelectList.</returns>
        /// <exception cref="System.ArgumentException">An Enumeration type is required.;enumObj</exception>
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj, string defaultText, string defaultValue, bool markCurrentAsSelected) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) throw new ArgumentException(@"An Enumeration type is required.", "enumObj");

            var values = (from TEnum enumValue in Enum.GetValues(typeof(TEnum))
                          select new
                          {
                              Value = Convert.ToInt32(enumValue).ToString(CultureInfo.InvariantCulture),
                              Text = enumValue.Description()
                          }).ToList();
            values.Insert(0, new
            {
                Value = defaultValue,
                Text = defaultText
            });

            object selectedValue = null;
            if (markCurrentAsSelected)
                selectedValue = Convert.ToInt32(enumObj).ToString(CultureInfo.InvariantCulture);
            return new SelectList(values, "Value", "Text", selectedValue);
        }

        /// <summary>
        /// Description: To the select list.
        /// </summary>
        /// <typeparam name="TEnum">The type of the t enum.</typeparam>
        /// <param name="enumObj">The enum object.</param>
        /// <param name="currentAsSelected">The current as selected.</param>
        /// <param name="defaultText">The default text.</param>
        /// <param name="hasDefaultRow">if set to <c>true</c> [has default row].</param>
        /// <returns>SelectList.</returns>
        /// <exception cref="System.ArgumentException">An Enumeration type is required.;enumObj</exception>
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj, object currentAsSelected, string defaultText = "", bool hasDefaultRow = true) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) throw new ArgumentException(@"An Enumeration type is required.", "enumObj");


            var values = (from TEnum enumValue in Enum.GetValues(typeof(TEnum))
                          select new
                          {
                              Value = Convert.ToInt32(enumValue).ToString(),
                              Text = enumValue.Description()
                          }).ToList();
            if (hasDefaultRow)
            {
                values.Insert(0, new
                {
                    Value = "",
                    Text = defaultText
                });
            }

            return new SelectList(values, "Value", "Text", currentAsSelected);
        }

        /// <summary>
        /// Author: MarkNguen
        /// Created on: 9/19/2014 10:52 AM
        /// Description: To the select list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <param name="textName">Name of the text.</param>
        /// <param name="valueSelected">The value selected.</param>
        /// <param name="valueDefault">The value default.</param>
        /// <param name="textDefault">The text default.</param>
        /// <returns>SelectList.</returns>
        /// <exception cref="System.Exception">valueName not of  + typeof(T).Name</exception>
        public static SelectList ToSelectList<T>(this IEnumerable<T> helper, string valueName, string textName, string valueSelected = "",
            string valueDefault = "", string textDefault = "")
        {
            var selectListItems = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Value = valueDefault,
                    Text = textDefault
                }
            };
            var props = TypeDescriptor.GetProperties(typeof(T));

            PropertyDescriptor prop = props.Find(valueName, true);
            if (prop == null)
            {
                throw new Exception(string.Format("{0} not of " + typeof(T).Name, valueName));
            }
            int valueIndex = props.IndexOf(prop);

            prop = props.Find(textName, true);
            if (prop == null)
            {
                throw new Exception(string.Format("{0} not of " + typeof(T).Name, textName));
            }
            int textIndex = props.IndexOf(prop);

            selectListItems.AddRange(from item in helper
                                     let value = Convert.ToString(props[valueIndex].GetValue(item))
                                     let selected = !string.IsNullOrEmpty(valueSelected) && valueSelected.Equals(value, StringComparison.OrdinalIgnoreCase)
                                     select new SelectListItem()
                                     {
                                         Value = value,
                                         Selected = selected,
                                         Text = Convert.ToString(props[textIndex].GetValue(item))
                                     });

            return new SelectList(selectListItems, "Value", "Text", valueSelected);
        }

        /// <summary>
        /// Author: MarkNguen
        /// Created on: 9/19/2014 10:52 AM
        /// Description: To the select list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <param name="textName">Name of the text.</param>
        /// <param name="textDefault">The text default.</param>
        /// <returns>SelectList.</returns>
        /// <exception cref="System.Exception">valueName not of  + typeof(T).Name</exception>
        public static SelectList ToSelectList<T>(this IEnumerable<T> helper, string valueName, string textName, string textDefault = "")
        {
            return ToSelectList<T>(helper, valueName, textName, textDefault, true);
        }

        /// <summary>
        /// Author: MarkNguen
        /// Created on: 3/27/2015 1:07 PM
        /// Description: To the select list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <param name="textName">Name of the text.</param>
        /// <param name="textDefault">The text default.</param>
        /// <param name="hasDefault">if set to <c>true</c> [has default].</param>
        /// <returns>SelectList.</returns>
        /// <exception cref="System.Exception"></exception>
        public static SelectList ToSelectList<T>(this IEnumerable<T> helper, string valueName, string textName, string textDefault = "", bool hasDefault = true)
        {
            var selectListItems = new List<SelectListItem>();
            var props = TypeDescriptor.GetProperties(typeof(T));
            if (props[valueName] == null)
            {
                throw new Exception(string.Format("{0} not of " + typeof(T).Name, valueName));
            }
            if (props[textName] == null)
            {
                throw new Exception(string.Format("{0} not of " + typeof(T).Name, textName));
            }

            selectListItems.AddRange(from item in helper
                                     let value = Convert.ToString(props[valueName].GetValue(item))
                                     select new SelectListItem()
                                     {
                                         Value = value,
                                         Text = Convert.ToString(props[textName].GetValue(item))
                                     });

            if (hasDefault)
            {
                selectListItems.Insert(0, new SelectListItem
                                                            {
                                                                Selected = true,
                                                                Value = "",
                                                                Text = textDefault
                                                            });
            }

            return new SelectList(selectListItems, "Value", "Text");
        }

        /// <summary>
        /// Author: MarkNguen
        /// Created on: 1/22/2015 3:21 PM
        /// Description: To the select list.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <param name="textName">Name of the text.</param>
        /// <param name="textDefault">The text default.</param>
        /// <returns>SelectList.</returns>
        /// <exception cref="System.Exception"></exception>
        public static SelectList ToSelectList(this DataTable helper, string valueName, string textName, string textDefault = "")
        {
            var selectListItems = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Selected = true,
                    Value = "",
                    Text = textDefault
                }
            };
            if (!helper.Columns.Contains(valueName))
            {
                throw new Exception(string.Format("{0} not of Table", valueName));
            }
            if (!helper.Columns.Contains(textName))
            {
                throw new Exception(string.Format("{0} not of Table", textName));
            }

            selectListItems.AddRange(from item in helper.Select()
                                     select new SelectListItem()
                                     {
                                         Value = ((DataRow)item)[valueName].ToString(),
                                         Text = ((DataRow)item)[textName].ToString()
                                     });

            return new SelectList(selectListItems, "Value", "Text");
        }

        /// <summary>
        /// Author: MarkNguyen
        /// Created on: 7/13/2015 5:37 PM
        /// Description: Drops down list.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="selectList">The select list.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>MvcHtmlString.</returns>
        public static MvcHtmlString DropDownList(this HtmlHelper htmlHelper, string name, string value, IEnumerable<SelectListItem> selectList, object htmlAttributes = null)
        {
            var selectListItems = selectList as SelectListItem[] ?? selectList.ToArray();
            if (selectListItems.Any())
            {
                foreach (var selectListItem in selectListItems)
                {
                    if (selectListItem.Value == value)
                    {
                        selectListItem.Selected = true;
                    }
                    else
                    {
                        selectListItem.Selected = false;
                    }
                }
            }
            return htmlHelper.DropDownList(name, selectListItems, htmlAttributes);
        }

        /// <summary>
        /// Author: MarkNguyen
        /// Created on: 7/13/2015 5:37 PM
        /// Description: Drops down list.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="dataTable">The data table.</param>
        /// <param name="displayText">The display text.</param>
        /// <param name="displayValue">The display value.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>MvcHtmlString.</returns>
        public static MvcHtmlString DropDownList(this HtmlHelper htmlHelper, string name, object value, DataTable dataTable, string displayText, string displayValue, object htmlAttributes = null)
        {
            if (dataTable == null)
            {
                throw new Exception("Datasource not nullable");
            }
            if (!dataTable.Columns.Contains(displayText))
            {
                throw new Exception(string.Format("{0} not belong Datasource", displayText));
            }
            if (!dataTable.Columns.Contains(displayValue))
            {
                throw new Exception(string.Format("{0} not belong Datasource", displayValue));
            }

            var valStr = Convert.ToString(value);
            var selectListItems = new List<SelectListItem>();
            if (dataTable.Rows.Count > 0)
            {
                selectListItems.AddRange(from DataRow row in dataTable.Rows
                    let val = Convert.ToString(row[displayValue])
                    select new SelectListItem()
                    {
                        Text = Convert.ToString(row[displayText]), 
                        Value = val, 
                        Selected = val == valStr
                    });
            }
            return htmlHelper.DropDownList(name, selectListItems, htmlAttributes);
        }

    }
}