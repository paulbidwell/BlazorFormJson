using System;
using System.Collections.Generic;

namespace DynamicComponents
{
    public class FieldDescriptor
    {
        public string FieldName { get; set; }
        public Type FieldType { get; set; }
        public Dictionary<string, object> LayoutAttributes { get; set; }
        public Dictionary<string, object> ValidationAttributes { get; set; }
    }
}