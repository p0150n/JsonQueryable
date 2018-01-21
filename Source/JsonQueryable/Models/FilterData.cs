using Newtonsoft.Json.Linq;

namespace JsonQueryable.Models
{
    public class FilterData
    {
        public string Name { get; set; }

        public JToken Data { get; set; }
    }
}
