using System;

namespace SSD.Framework.Extensions
{
    #region Extended Attributes

    class EnumGuidAttribute : Attribute
    {
        private Guid _value;
        public Guid Value
        {
            get { return _value; }
        }

        public EnumGuidAttribute(Guid guid)
        {
            this._value = guid;
        }
    }
    #endregion
}