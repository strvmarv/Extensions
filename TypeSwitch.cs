using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions
{
    // Credit: JaredPar
    // Source: http://stackoverflow.com/a/299120/578859
    public static class TypeSwitch
    {
        public static CaseInfo Case<T>(Action action)
        {
            return new CaseInfo()
            {
                Action = x => action(),
                Target = typeof(T)
            };
        }

        public static CaseInfo Case<T>(Action<T> action)
        {
            return new CaseInfo()
            {
                Action = (x) => action((T)x),
                Target = typeof(T)
            };
        }

        public static CaseInfo Default(Action action)
        {
            return new CaseInfo()
            {
                Action = x => action(),
                IsDefault = true
            };
        }

        public static void Do(object source, params CaseInfo[] cases)
        {
            var type = source.GetType();
            foreach (var entry in cases)
            {
                if (entry.IsDefault || entry.Target.IsAssignableFrom(type))
                {
                    entry.Action(source);
                    break;
                }
            }
        }

        public class CaseInfo
        {
            public Action<object> Action { get; set; }
            public bool IsDefault { get; set; }

            public Type Target { get; set; }
        }
    }
}
