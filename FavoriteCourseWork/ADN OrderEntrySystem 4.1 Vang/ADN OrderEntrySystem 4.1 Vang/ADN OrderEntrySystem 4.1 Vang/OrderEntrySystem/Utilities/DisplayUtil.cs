using OrderEntryEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OrderEntrySystem
{
    public class DisplayUtil
    {
        public static string GetControlDescription(MemberInfo memberInfo)
        {
            string result = ReflectionUtil.GetAttributePropertyValueAsString(memberInfo, typeof(EntityControlAttribute), "Description");

            if (result == null || result == String.Empty)
            {
                return memberInfo.Name.ToString();
            }

            return result;
        }

        public static ControlType GetControlType(PropertyInfo propertyInfo)
        {
            object result = ReflectionUtil.GetAttributePropertyValue(propertyInfo, typeof(EntityControlAttribute), "ControlType");

            if (result == null)
            {
                return ControlType.None;
            }

            return (ControlType)result;
        }

        public static bool HasControl(MemberInfo memberInfo)
        {
            return ReflectionUtil.HasAttribute(memberInfo, typeof(EntityControlAttribute));
        }

        public static string GetFieldDescription(MemberInfo memberInfo)
        {
            string value = ReflectionUtil.GetAttributePropertyValueAsString(memberInfo, typeof(EntityDescriptionAttribute), "Description");

            return value == null || value == string.Empty ? memberInfo.Name : value;
        }

        public static int GetControlSequence(MemberInfo memberInfo)
        {
            string val = ReflectionUtil.GetAttributePropertyValueAsString(memberInfo, typeof(EntityControlAttribute), "Sequence");

            int value;

            if (val == null || val == string.Empty || val == "0")
            {
                value = 0;
            }
            else
            {
                value = int.Parse(val);
            }

            return value;
        }

        public static string GetColumnDescription(MemberInfo memberInfo)
        {
            string value = ReflectionUtil.GetAttributePropertyValueAsString(memberInfo, typeof(EntityColumnAttribute), "Description");

            return value == null || value == string.Empty ? memberInfo.Name : value;
        }

        public static int GetColumnWidth(MemberInfo memberInfo)
        {
            string value = ReflectionUtil.GetAttributePropertyValueAsString(memberInfo, typeof(EntityColumnAttribute), "Width");

            int returnval;

            if (value == null || value == string.Empty || value == "0")
            {
                returnval = 0;
            }
            else
            {
                returnval = int.Parse(value);
            }

            return returnval;
        }

        public static int GetColumnSequence(MemberInfo memberInfo)
        {
            string value = ReflectionUtil.GetAttributePropertyValueAsString(memberInfo, typeof(EntityColumnAttribute), "Sequence");

            int returnval;

            if (value == null || value == string.Empty || value == "0")
            {
                returnval = 0;
            }
            else
            {
                returnval = int.Parse(value);
            }

            return returnval;
        }

        public static bool HasColumn(MemberInfo memberInfo)
        {
            return ReflectionUtil.HasAttribute(memberInfo, typeof(EntityColumnAttribute));
        }
    }
}
