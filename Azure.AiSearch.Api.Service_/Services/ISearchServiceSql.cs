using Azure.AISearch.Api.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace Azure.AISearch.Api.Service.Services
{
    public interface ISearchServiceSql
    {
        Task<ActionResult<IEnumerable<AppReview>>> Search(Filter request);
        Task<ActionResult<IEnumerable<string>>> GetPlatforms();
        Task<ActionResult<IEnumerable<string>>> GetCountries();
        Task<ActionResult<IEnumerable<int?>>> GetStars();
        Task<ActionResult<IEnumerable<string>>> GeLabels();
    }
}
