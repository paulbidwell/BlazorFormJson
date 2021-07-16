using System.Collections.Generic;

namespace DynamicComponents
{
    public interface INewTypeBuilder
    {
        public object CreateNewObject(List<FieldDescriptor> fields);
    }
}