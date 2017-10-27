using System;
using System.Reflection;

namespace MVCWebProject2.utilities
{

    public class StringValue : Attribute
    {
        #region Properties
        public string Value { get; protected set; }
        #endregion

        #region Constructor
        public StringValue(string value)
        {
            this.Value = value;
        }
        #endregion
    }

    public static class EnumHelper
    {
        #region GetStringValue
        public static string GetStringValue(this Enum value)
        {
            var type = value.GetType();
            MemberInfo[] memInfo = type.GetMember(value.ToString());

            if (memInfo.Length > 0)
            {
                object[] attributes = memInfo[0].GetCustomAttributes(false);
                return attributes.Length > 0 ? ((StringValue)attributes[0]).Value : string.Empty;
            }
            return String.Empty;
        }
        #endregion
    }
}