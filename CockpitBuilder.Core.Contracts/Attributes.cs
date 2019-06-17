using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CockpitBuilder.Core.Contracts
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class PropertyEditorAttribute : Attribute
    {
        public PropertyEditorAttribute(string typeIdentifier, string category)
        {
            TypeIdentifier = typeIdentifier;
            Category = category;
        }

        public string TypeIdentifier { get; }

        public string Category { get; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ControlAttribute : Attribute
    {
        public ControlAttribute(string typeIdentifier, string name, string category, Type renderer)
        {
            TypeIdentifier = typeIdentifier;
            Name = name;
            Category = category;
            Renderer = renderer;
        }

        public string TypeIdentifier { get; }

        public string Name { get; }

        public string Category { get; }

        public Type Renderer { get; }

        public string Requires { get; set; }
    }


}
