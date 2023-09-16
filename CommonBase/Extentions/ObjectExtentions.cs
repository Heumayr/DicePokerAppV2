using CommonBase.Attributes;
using System.Reflection;

namespace CommonBase.Extentions
{
    public static class ObjectExtentions
    {
        public static void ShallowCopyFrom(this object target, object source)
        {
            ArgumentNullException.ThrowIfNull(target);
            ArgumentNullException.ThrowIfNull(source);

            var targetType = target.GetType();
            var sourceType = source.GetType();

            foreach (var targetProp in targetType.GetProperties())
            {
                if (!targetProp.CanWrite)
                    continue;

                var sourceProp = sourceType.GetProperties()
                                           .FirstOrDefault(p => p.CanRead && p.Name == targetProp.Name && p.PropertyType == targetProp.PropertyType);

                if (sourceProp == null)
                {
                    continue;
                }

                var copyAtt = sourceProp.GetCustomAttributes<CopyFromAttribute>().FirstOrDefault();

                if (copyAtt != null && copyAtt.BlockCopy)
                {
                    targetProp.SetValue(target, default);
                    continue;
                }

                targetProp.SetValue(target, sourceProp.GetValue(source));
            }
        }

        public static void CheckArgument(this object source, string name)
        {
            if (source == null)
                throw new ArgumentNullException(name);

        }

        public static void CopyFrom(this object objectTarget, object source)
        {
            source.CheckArgument(nameof(source));

            Type typeSource = source.GetType();

            var probInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetIndexParameters().Length == 0); //removed nonpublic flag

            foreach (var prob in probInfo)
            {

                if (prob.CanWrite && prob.CanRead)
                {
                    if (prob.PropertyType.IsValueType || prob.PropertyType.IsEnum || prob.PropertyType == typeof(string) || prob.PropertyType.IsArray)
                    {
                        object? value = prob.GetValue(source);
                        prob.SetValue(objectTarget, value);

                    }

                    else if (!prob.PropertyType.GetTypeInfo().IsValueType) // IsComplexType
                    {

                        object? probValue = prob.GetValue(source, null);

                        prob.SetValue(objectTarget, probValue);

                        object? srcValue = prob.GetValue(source);
                        object? tarValue = prob.GetValue(objectTarget);

                        if (srcValue != null && tarValue != null)
                            CopyFrom(tarValue, srcValue);
                    }
                }
            }
        }

        /// <summary>
        /// Shallow copies same properties into the given generic type.
        /// </summary>
        /// <typeparam name="Out">Type to copy into</typeparam>
        /// <param name="source">Source</param>
        /// <returns>New instance of generic type with values from source</returns>
        public static Out CopyInto<Out>(this object source) where Out : class, new()
        {
            var outType = new Out();
            outType.ShallowCopyFrom(source);
            return outType;

        }




    }

}
