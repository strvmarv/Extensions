using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions
{
    public static class TypeExtensions
    {
        #region Attributes

        public static T GetAttribute<T>(this Type typeWithAttributes) where T : Attribute
        {
            return GetAttributes<T>(typeWithAttributes).FirstOrDefault();
        }

        public static IEnumerable<T> GetAttributes<T>(this Type typeWithAttributes) where T : Attribute
        {
            // Try to find the configuration attribute for the default logger if it exists
            var configAttributes = Attribute.GetCustomAttributes(typeWithAttributes, typeof(T), false);

            // get just the first one
            if (configAttributes.Length > 0)
            {
                foreach (T attribute in configAttributes)
                {
                    yield return attribute;
                }
            }
        }

        #endregion
    }
}
