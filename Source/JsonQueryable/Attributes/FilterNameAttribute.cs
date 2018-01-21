using System;

namespace JsonQueryable.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FilterNameAttribute : Attribute
    {
        public FilterNameAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
