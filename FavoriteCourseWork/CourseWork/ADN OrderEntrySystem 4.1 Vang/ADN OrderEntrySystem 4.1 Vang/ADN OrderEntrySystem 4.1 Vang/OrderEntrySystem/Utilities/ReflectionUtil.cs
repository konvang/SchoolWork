using System;
using System.Reflection;

namespace OrderEntrySystem
{
    public static class ReflectionUtil
    {
        public static bool HasAttribute(MemberInfo memberInfo, Type attributeType)
        {
            bool result = false; 

            Attribute[] attributes = (Attribute[])memberInfo.GetCustomAttributes(attributeType, false);

            if (attributes.Length > 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        public static object GetAttributePropertyValue(MemberInfo memberInfo, Type attributeType, string propertyName)
        {
            object result = null;

            // Read all attributes of the specified type from the member.
            Attribute[] attributes = (Attribute[])memberInfo.GetCustomAttributes(attributeType, false);

            if (attributes.Length > 0)
            {
                // Get the first attribute (only one is supported).
                Attribute attribute = attributes[0];

                // Read the property from the attribute.
                PropertyInfo propertyInfo = attribute.GetType().GetProperty(propertyName);

                // If property is found...
                if (propertyInfo != null)
                {
                    // Read the value from the property.
                    result = propertyInfo.GetValue(attribute, null);
                }
            }

            return result;
        }

        public static string GetAttributePropertyValueAsString(MemberInfo memberInfo, Type attributeType, string propertyName)
        {
            string result = GetAttributePropertyValue(memberInfo, attributeType, propertyName) as string;

            return result;
        }
    }
}