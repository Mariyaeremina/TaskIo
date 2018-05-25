using System;
using System.Collections.Generic;
using System.Reflection;

namespace IocExample.Classes
{
    class MyDependencyResolver
    {
        private Dictionary<Type, Type> typeBindings;
        private Dictionary<Type, object> objBindings;

        public MyDependencyResolver()
        {
            typeBindings = new Dictionary<Type, Type>();
            objBindings = new Dictionary<Type, object>();
        }

        public void BindToType(Type from, Type to)
        {
            typeBindings.Add(from, to);
        }

        public void BindToObj(Type from, object to)
        {
            objBindings.Add(from, to);
        }

        public T Get<T>()
        {
            return (T)Get(typeof(T));
        }

        public object Get(Type type)
        {
            if (objBindings.ContainsKey(type))
            {
                return objBindings[type];
            }
            else
            {
                ParameterInfo[] paramInfo = Utils.GetSingleConstructor(typeBindings[type])
                                                  .GetParameters();       
                List<object> parameters = new List<object>(paramInfo.Length);
                foreach (ParameterInfo p in paramInfo)
                {
                    parameters.Add(Get(p.ParameterType));
                }                    
                return Utils.CreateInstance(typeBindings[type], parameters);
            }
        }
    }
}
