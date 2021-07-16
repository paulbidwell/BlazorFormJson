using System.Collections.Generic;

namespace DynamicComponents
{
    public class FormFieldDto
    {
        public string DataType { get; set; }
        public string Label { get; set; }
        public bool ShowLabel { get; set; } = true;
        public string Name { get; set; }
        public bool Enabled { get; set; } = true;
        public Dictionary<string, object> Layout { get; set; }
        public Dictionary<string, object> Validation { get; set; }
    }
}