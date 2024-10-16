using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace Azure.AISearch.Api.Service.Models
{
    public class AppReviewAI
    {
        [SimpleField(IsKey = true, IsFilterable = true)]
        public string Id { get; set; }
        [SimpleField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string Date { get; set; }
        [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string? Platform { get; set; }
        [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string? Country { get; set; }
        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string? Review { get; set; }
        [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string Star { get; set; }
        [SearchableField(IsFilterable = true)]
        public string UserID { get; set; }
        [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string IssueFlag { get; set; }
        [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string LikesCount { get; set; }
        [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string DislikeCount { get; set; }
        [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
        public string Label { get; set; }
    }
}
