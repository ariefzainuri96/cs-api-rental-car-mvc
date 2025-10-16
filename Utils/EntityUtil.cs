using System;
using System.Collections.Generic;
using System.Reflection;

namespace cs_api_rental_car_mvc.Utils
{
    public class EntityUtil
    {
        public static List<string> CheckEntityField<T>(Dictionary<string, object> values)
        {
            List<string> invalidPropertyList = new List<string>();

            foreach (var item in values)
            {
                var propertyName = item.Key;

                // Use reflection to check if the property exists and is writable
                var propertyInfo = typeof(T).GetProperty(propertyName,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                var isValid = propertyInfo != null && propertyInfo.CanWrite;

                if (!isValid)
                {
                    invalidPropertyList.Add(propertyName);
                }
            }

            return invalidPropertyList;
        }

        public static void PatchEntity<T>(T entity, Dictionary<string, object> updates)
        {
            foreach (var update in updates)
            {
                var propertyName = update.Key;
                var propertyValue = update.Value;

                // Use reflection to check if the property exists and is writable
                var propertyInfo = typeof(T).GetProperty(propertyName,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                propertyInfo?.SetValue(entity, Convert.ChangeType(propertyValue, propertyInfo.PropertyType));
            }
        }
    }
}