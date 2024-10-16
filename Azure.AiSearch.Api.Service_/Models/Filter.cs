namespace Azure.AISearch.Api.Service.Models
{
    public class Filter
    {
       public const int DEFAULT_LIMIT = 5;
       public string? Platform { get; set; }
       public string? Country { get; set; }
       public int? Star { get; set; }
       public string? Label { get; set; }
        public int? Limit { get; set; } = DEFAULT_LIMIT;
    }
}
