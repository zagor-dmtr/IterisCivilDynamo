using Autodesk.Civil.DatabaseServices;
using Autodesk.Civil.DynamoNodes;
using DynamoServices;
using System.Reflection;
using System.Runtime.CompilerServices;
using C3dDb = Autodesk.Civil.DatabaseServices;

namespace IterisCivilDynamo.CivilObjects
{
    /// <summary>
    /// 
    /// </summary> 
    [RegisterForTrace]
    public class CivilEntity : CivilObject
    {
        internal C3dDb.Entity AeccEntity => AcObject as C3dDb.Entity;

        /// <summary>
        /// 
        /// </summary>
        protected const string NotApplicableMsg = "<Not applicable>";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isDynamoOwned"></param>
        internal CivilEntity
            (Entity entity, bool isDynamoOwned) : base(entity, isDynamoOwned)
        {
        }

        /// <summary>
        /// Gets whether the Network is a shortcut object.
        /// A shortcut object is located in another drawing, and linked using a data shortcut.
        /// If the entity is native to the current drawing this property returns false;
        /// if it is being referenced via data shortcuts it returns true.
        /// </summary>
        public bool IsReference
            => AeccEntity.IsReferenceObject || AeccEntity.IsReferenceSubObject;

        /// <summary>
        /// Get the civil object property
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public object GetProperty(string propertyName)
        {
            try
            {
                PropertyInfo propInfo = AeccEntity.GetType()
                        .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

                return propInfo?.GetValue(AeccEntity);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Sets the value for the civil object property.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public bool SetProperty(string propertyName, object value) => SetValue(propertyName, value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected double GetDouble([CallerMemberName]string propertyName = null)
        {
            try
            {
                PropertyInfo propInfo = AeccEntity.GetType()
                    .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

                if (propInfo != null)
                {
                    return (double)propInfo.GetValue(AeccEntity);
                }
            }
            catch { }
            return double.NaN;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected string GetString([CallerMemberName]string propertyName = null)
        {
            try
            {
                PropertyInfo propInfo = AeccEntity.GetType()
                    .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

                if (propInfo != null)
                {
                    return propInfo.GetValue(AeccEntity).ToString();
                }
            }
            catch { }
            return NotApplicableMsg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected int GetInt([CallerMemberName]string propertyName = null)
        {
            try
            {
                PropertyInfo propInfo = AeccEntity.GetType()
                    .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

                if (propInfo != null)
                {
                    return (int)propInfo.GetValue(AeccEntity);
                }
            }
            catch { }
            return int.MinValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool GetBool([CallerMemberName]string propertyName = null)
        {
            try
            {
                PropertyInfo propInfo = AeccEntity.GetType()
                    .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

                if (propInfo != null)
                {
                    return (bool)propInfo.GetValue(AeccEntity);
                }
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="value"></param>        
        protected bool SetValue(object value, [CallerMemberName]string methodName = null)
        {
            if (methodName.StartsWith("Set"))
            {
                methodName = methodName.Substring(3);
            }

            return SetValue(methodName, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected bool SetValue(string propertyName, object value)
        {
            try
            {
                bool openedForWrite = AeccEntity.IsWriteEnabled;
                if (!openedForWrite) AeccEntity.UpgradeOpen();
                PropertyInfo propInfo = AeccEntity.GetType()
                    .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                propInfo?.SetValue(AeccEntity, value);
                if (!openedForWrite) AeccEntity.DowngradeOpen();
                return true;
            }
            catch { }
            return false;
        }
    }
}
