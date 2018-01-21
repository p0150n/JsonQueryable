using System;
using JsonQueryable.Enums;

namespace JsonQueryable.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FilterKindAttribute : Attribute
    {
        public FilterKindAttribute(FilterKinds filterKind)
        {
            this.FilterKind = filterKind;
        }

        public FilterKinds FilterKind { get; }
    }
}
