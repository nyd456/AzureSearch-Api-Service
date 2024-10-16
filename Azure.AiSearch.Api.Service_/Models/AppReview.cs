
namespace Azure.AISearch.Api.Service.Models
{
    public class AppReview
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Platform { get; set; }
        public string? Country { get; set; }
        public string? Review { get; set; }
        public int? Star { get; set; }
        public string? UserID { get; set; }
        public bool? IssueFlag { get; set; } = false;
        public int? LikesCount { get; set; }
        public int? DislikeCount { get; set; }
        public string? Label { get; set; }
    }
}