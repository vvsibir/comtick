using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SharedTools
{
    public static class PropHelper
    {
        public static IEnumerable<System.Reflection.PropertyInfo> GetPropsForApply(Type type = null)
        {
            return (type ?? typeof(StartUpRoutine_base)).GetProperties().Where(p => p.PropertyType.Equals(typeof(string)));
        }

        /// <summary>
        /// берет набор свойств объекта для перечисления
        /// </summary>
        /// <param name="notNull"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> getPropsEnu(object notNull)
        {
            return ((IEnumerable<PropertyInfo>)notNull.GetType().GetProperties());
        }
        /// <summary>
        /// берет набор свойств объекта для перечисления
        /// </summary>
        /// <param name="notNull"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> getPropsEnuType(Type notNull)
        {
            return ((IEnumerable<PropertyInfo>)notNull.GetProperties());
        }
        /// <summary>
        /// копирует свойства с одного объекта на другой
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="target"></param>
        /// <param name="skipNull">пропускать значения null</param>
        public static void CopyProperties(object resource, object target, bool skipNull = true)
        {
            if (resource == null || target == null) return;

            // пересечение свойств двух объектов
            var duplicates =
                (resource.GetType() == target.GetType())
                ? getPropsEnu(resource)
                : getPropsEnu(resource).Intersect(getPropsEnu(target)); // Intersect можно расширить компаратором https://msdn.microsoft.com/ru-ru/library/bb355408(v=vs.110).aspx

            // применяем свойства
            duplicates.ForEach((d) => { if (d.CanWrite && d.CanRead && (!skipNull || d.GetValue(resource, null) != null)) d.SetValue(target, d.GetValue(resource, null), null); });

        }
    }
}
