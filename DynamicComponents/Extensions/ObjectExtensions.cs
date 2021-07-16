using System;

namespace DynamicComponents.Extensions
{
    public static class ObjectExtensions
    {
        public static Type GetDataType(this string dataTypeName)
        {
            return dataTypeName switch
            {
                "bool" => typeof(bool),
                "byte" => typeof(byte),
                "byte?" => typeof(byte?),
                "sbyte" => typeof(sbyte),
                "sbyte?" => typeof(sbyte?),
                "char" => typeof(char),
                "char?" => typeof(char?),
                "decimal" => typeof(decimal),
                "decimal?" => typeof(decimal?),
                "double" => typeof(double),
                "double?" => typeof(double?),
                "float" => typeof(float),
                "float?" => typeof(float?),
                "int" => typeof(int),
                "int?" => typeof(int?),
                "uint" => typeof(uint),
                "uint?" => typeof(uint?),
                "long" => typeof(long),
                "long?" => typeof(long?),
                "ulong" => typeof(ulong),
                "ulong?" => typeof(ulong?),
                "short" => typeof(short),
                "short?" => typeof(short?),
                "ushort" => typeof(ushort),
                "ushort?" => typeof(ushort?),
                "string" => typeof(string),
                _ => null,
            };
        }
    }
}