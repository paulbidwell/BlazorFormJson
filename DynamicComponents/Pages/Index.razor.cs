using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DynamicComponents.Dto;
using DynamicComponents.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;

namespace DynamicComponents.Pages
{
    public partial class Index
    {
        [Inject]
        INewTypeBuilder TypeBuilder { get; set; }

        [Parameter]
        public EventCallback<object> ValueChanged { get; set; }

        [Parameter]
        public EventCallback<object> ModelChanged { get; set; }

        private FormDto _formToRender;
        private object _model;

        protected override async Task OnInitializedAsync()
        {
            var json = await File.ReadAllTextAsync("TestForm.json");
            _formToRender = JsonConvert.DeserializeObject<FormDto>(json);

            var fieldsForType = new List<FieldDescriptor>();

            foreach (var formField in _formToRender.Fields)
            {
                fieldsForType.Add(new FieldDescriptor
                {
                    FieldName = formField.Name,
                    FieldType = formField.DataType.GetDataType(),
                    LayoutAttributes = formField.Layout,
                    ValidationAttributes = formField.Validation
                });
            }

            _model = TypeBuilder.CreateNewObject(fieldsForType);
        }

        private static void Save(EditContext editContext)
        {
            
        }
    }
}