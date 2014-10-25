using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class CompatibilityException : Exception
    {
        public CompatibilityException(object thisObject, object bindingObject, Type expectedType)
            : base("The type " + thisObject.GetType().Name + " is not compatible with " + bindingObject.GetType().Name + ". " + expectedType.Name + " is expected")
        { }
    }

    public class Compatibility
    {
        public static T Check<T>(object thisObject, object bindingObject)
        {
            try
            {
                return (T)bindingObject;
            }
            catch
            {
                throw new CompatibilityException(thisObject, bindingObject, typeof(T));
            }
        }
    }
}
