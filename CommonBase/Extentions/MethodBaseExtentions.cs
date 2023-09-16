using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CommonBase.Extentions
{
    public static class MethodBaseExtentions
    {
        public static MethodBase GetAsyncOriginal(this MethodBase method)
        {
            var result = method;

            if (method != null && method.DeclaringType != null
                && method.DeclaringType.GetInterfaces().Any(i => i == typeof(IAsyncStateMachine)))
            {
                var generatedType = method.DeclaringType;
                var originalType = generatedType.DeclaringType;

                if (originalType != null)
                {
                    result = originalType.GetMethods(BindingFlags.Instance
                                                 | BindingFlags.Static
                                                 | BindingFlags.Public
                                                 //NonPublic removed
                                                 | BindingFlags.DeclaredOnly)
                                         .First(m => m.GetCustomAttribute<AsyncStateMachineAttribute>()?.StateMachineType == generatedType);
                }
            }
            return result;
        }
    }
}
