using System.Collections.Generic;

namespace DynamicComponents.Dto
{
    public class FormDto
    {
        public decimal Version { get; set; }
        public string Description { get; set; }
        public List<FormFieldDto> Fields { get; set; }
    }
}