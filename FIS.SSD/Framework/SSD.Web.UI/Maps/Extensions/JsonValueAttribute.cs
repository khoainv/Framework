using System;

namespace SSD.Web.UI.Maps.Extensions
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    internal sealed class JsonValueAttribute : Attribute
    {
        public JsonValueAttribute(string jsonValue)
        {
            this.JsonValue = jsonValue;
        }

        public string JsonValue { get; set; }

    }
}
