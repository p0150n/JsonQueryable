using System;

namespace JsonQueryable.Exceptions
{
    public class FilterAttributeException<TAttribute> : Exception
        where TAttribute : Attribute
    {
        public FilterAttributeException() : base($"Filter must be annotated with {typeof(TAttribute).FullName}.")
        {
        }
    }
}
