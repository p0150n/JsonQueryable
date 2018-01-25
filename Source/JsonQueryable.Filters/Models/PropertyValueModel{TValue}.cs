namespace JsonQueryable.Filters.Models
{
    public class PropertyValueModel<TValue>
    {
        public string PropertyName { get; set; }

        public TValue Value { get; set; }
    }
}
