using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions
{
    // Credit: plunje
    // Source: http://stackoverflow.com/a/4440606/578859
    // License: https://creativecommons.org/licenses/by-sa/3.0/
    public static class ObjectComparator<T>
    {
        public static bool CompareProperties(T newObject, T oldObject)
        {
            if (Equals(newObject, oldObject)) return true;
            var newProps = newObject.GetType().GetProperties();
            var oldProps = oldObject.GetType().GetProperties();

            if (newProps.Length != oldProps.Length) return false;

            foreach (var newProperty in newProps)
            {
                var oldProperty = oldProps.SingleOrDefault(pi => pi.Name == newProperty.Name);
                if (oldProperty == null) return false;

                var newval = newProperty.GetValue(newObject, null);
                var oldval = oldProperty.GetValue(oldObject, null);

                if (!Equals(newval, oldval)) return false;
            }

            return true;
        }
    }
}
