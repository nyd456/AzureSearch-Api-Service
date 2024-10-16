using Microsoft.EntityFrameworkCore;

namespace Azure.AISearch.Api.Service.Models
{
    public class AppReviewContext : DbContext
    {
        public AppReviewContext(DbContextOptions<AppReviewContext> options) : base(options) { }

        //AppReviews will be Database table name
        public DbSet<AppReview> AppReviews { get; set; } = null!;
    }
}
