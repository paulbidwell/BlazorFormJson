using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using VxFormGenerator.Core.Layout;

namespace DynamicComponents
{
    public class NewTypeBuilder : INewTypeBuilder
    {
        private List<FieldDescriptor> _fields;

        public object CreateNewObject(List<FieldDescriptor> fields)
        {
            _fields = fields;

            var newTypeInfo = CompileResultTypeInfo();
            var newType = newTypeInfo.AsType();
            var newObject = Activator.CreateInstance(newType);

            return newObject;
        }

        private TypeInfo CompileResultTypeInfo()
        {
            var typeBuilder = GetTypeBuilder();
            
            foreach (var field in _fields)
            {
                CreateProperty(typeBuilder, field);
            }

            var objectTypeInfo = typeBuilder.CreateTypeInfo();
            return objectTypeInfo;
        }

        private static TypeBuilder GetTypeBuilder()
        {
            var typeSignature = "GeneratedType";
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(Guid.NewGuid().ToString()), AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder tb = moduleBuilder.DefineType(typeSignature,
                    TypeAttributes.Public |
                    TypeAttributes.Class |
                    TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout,
                    null);
            return tb;
        }

        private static void CreateProperty(TypeBuilder tb, FieldDescriptor field)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + field.FieldName, field.FieldType, FieldAttributes.Private);
            PropertyBuilder propertyBuilder = tb.DefineProperty(field.FieldName, PropertyAttributes.HasDefault, field.FieldType, null);

            if (field.LayoutAttributes.Any())
            {
                foreach (var attribute in field.LayoutAttributes)
                {
                    Type[] ctorParams = new Type[] { typeof(string) };
                    ConstructorInfo classCtorInfo = typeof(VxFormElementLayoutAttribute).GetConstructor(ctorParams);
                    CustomAttributeBuilder att = new CustomAttributeBuilder(classCtorInfo, new object[] { "1" });

                    if (att != null)
                    {
                        propertyBuilder.SetCustomAttribute(att);
                    }
                }
            }

            foreach (var attribute in field.ValidationAttributes)
            {
                CustomAttributeBuilder att = null;

                if (attribute.Key == "Required" && (bool)attribute.Value == true)
                {
                    Type[] ctorParams = Array.Empty<Type>();
                    ConstructorInfo classCtorInfo = typeof(RequiredAttribute).GetConstructor(ctorParams);
                    att = new CustomAttributeBuilder(classCtorInfo, Array.Empty<object>());                   
                }

                if (att != null)
                {
                    propertyBuilder.SetCustomAttribute(att);
                }
            }

            MethodBuilder getPropMthdBldr = 
                tb.DefineMethod("get_" + field.FieldName,
                    MethodAttributes.Public |
                    MethodAttributes.SpecialName |
                    MethodAttributes.HideBySig, field.FieldType,
                    Type.EmptyTypes);
            
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + field.FieldName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { field.FieldType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }
    }
}
