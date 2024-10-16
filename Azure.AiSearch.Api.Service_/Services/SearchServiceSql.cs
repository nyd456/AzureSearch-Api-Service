using Azure.AISearch.Api.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Azure.AISearch.Api.Service.Services
{
    public class SearchServiceSqlL : ISearchServiceSql
    {
        private readonly AppReviewContext _context;
        private readonly ILogger<SearchServiceSqlL> _logger;
        public SearchServiceSqlL(AppReviewContext context, ILogger<SearchServiceSqlL> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Search
        public async Task<ActionResult<IEnumerable<AppReview>>> Search(Filter request)
        {
            // Start the stopwatch to measure performance
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                // Simulate a database or repository query using IQueryable for filtering
                IQueryable<AppReview> query = _context.AppReviews.AsQueryable();

                // Apply filters based on the provided request fields
                if (!string.IsNullOrEmpty(request.Platform))
                {
                    query = query.Where(r => r.Platform == request.Platform);
                }

                if (!string.IsNullOrEmpty(request.Country))
                {
                    query = query.Where(r => r.Country == request.Country);
                }

                if (request.Star.HasValue)
                {
                    query = query.Where(r => r.Star == request.Star.Value);
                }

                if (!string.IsNullOrEmpty(request.Label))
                {
                    query = query.Where(r => r.Label == request.Label);
                }

                query = query.Take(request.Limit.Value);

                // Execute the query asynchronously and retrieve the filtered list of AppReviews
                var result = await query.ToListAsync();

                // Log the result count and return it as the ActionResult
                _logger.LogInformation($"SearchService::Search found {result.Count} reviews.");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"SearchService::Search Exception: {ex.StackTrace}");
                throw new Exception("Error retrieving app reviews", ex);
            }
            finally
            {
                sw.Stop();
                _logger.LogInformation($"SearchService::Search completed in {sw.ElapsedMilliseconds} ms.");
            }
        }

        // Get Platforms
        public async Task<ActionResult<IEnumerable<string>>> GetPlatforms()
        {
            // Start the stopmatch to measure performance
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                var res = await _context.AppReviews.Select(r => r.Platform).Distinct().ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"SearchService::GetPlatforms Exception: {ex.StackTrace}");
                throw new Exception("Error retrieving platforms", ex);
            }
            finally
            {
                sw.Stop();
                _logger.LogInformation($"SearchService::GetPlatforms completed in {sw.ElapsedMilliseconds} ms.");
            }
        }

        // Get Countries
        public async Task<ActionResult<IEnumerable<string>>> GetCountries()
        {
            // Start the stopmatch to measure performance
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                var res = await _context.AppReviews.Select(r => r.Country).Distinct().ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"SearchService::GetCountries Exception: {ex.StackTrace}");
                throw new Exception("Error retrieving counties", ex);
            }
            finally
            {
                sw.Stop();
                _logger.LogInformation($"SearchService::GetCountries completed in {sw.ElapsedMilliseconds} ms.");
            }
        }

        // Get Stars
        public async Task<ActionResult<IEnumerable<int?>>> GetStars()
        {
            // Start the stopmatch to measure performance
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                var res = await _context.AppReviews.Select(r => r.Star).Distinct().ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"SearchService::GetStars Exception: {ex.StackTrace}");
                throw new Exception("Error retrieving GetStars", ex);
            }
            finally
            {
                sw.Stop();
                _logger.LogInformation($"SearchService::GetStars completed in {sw.ElapsedMilliseconds} ms.");
            }
        }

        // Get GeLabels
        public async Task<ActionResult<IEnumerable<string>>> GeLabels()
        {
            // Start the stopmatch to measure performance
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                var res = await _context.AppReviews.Select(r => r.Label).Distinct().ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"SearchService::GeLabels Exception: {ex.StackTrace}");
                throw new Exception("Error retrieving GeLabels", ex);
            }
            finally
            {
                sw.Stop();
                _logger.LogInformation($"SearchService::GeLabels completed in {sw.ElapsedMilliseconds} ms.");
            }
        }

    }
}
