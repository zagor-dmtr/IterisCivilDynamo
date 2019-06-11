using Autodesk.DesignScript.Runtime;
using System;
using System.Reflection;

namespace Iteris.Civil.Dynamo.Support
{
    /// <summary>
    /// Support methods for use reflection
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public static class ReflectionSupport
    {
        /// <summary>
        /// Get property from object
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="defaultValue">Returned value if any error happened</param>
        /// <returns></returns>
        public static object GetProperty
            (object obj, string propertyName, object defaultValue)
        {
            Type curveObjType = obj?.GetType();
            try
            {
                return curveObjType?
                    .GetProperty(propertyName, BindingFlags.IgnoreCase)?
                    .GetValue(obj) ?? defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Get typed property from object
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="defaultValue">Returned value if any error happened</param>
        /// <returns></returns>
        public static T GetProperty<T>(object obj, string propertyName, T defaultValue)
        {
            Type curveObjType = obj?.GetType();

            try
            {
                return
                    (T)(curveObjType?.GetProperty(propertyName, BindingFlags.IgnoreCase)?
                    .GetValue(obj) ?? defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
