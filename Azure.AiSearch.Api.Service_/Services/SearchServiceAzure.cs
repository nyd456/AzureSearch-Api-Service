using Azure;
using Azure.AISearch.Api.Service.Models;
using Azure.Search.Documents;
using AzureSearch.Api.Service.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;



namespace Azure.AISearch.Api.Service.Services
{
    public class SearchServiceAzure: ISearchServiceAzure
    {
        private const int DEFAULT_LIMIT = 10;
        private readonly ILogger<SearchServiceAzure> _logger;
        private readonly AzureSearchClient _searchClient;
        public SearchServiceAzure(AzureSearchClient searchClient, ILogger<SearchServiceAzure> logger)
        {
            _logger = logger;
            _searchClient = searchClient;
        }
        // Search
        public async Task<ActionResult<IEnumerable<AppReviewAI>>> Search(Filter request)
        {
            // Start the stopwatch to measure performance
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                // Construct search filters based on request parameters
                var filters = new List<string>();

                if (!string.IsNullOrEmpty(request.Platform))
                {
                    filters.Add($"Platform eq '{request.Platform}'");
                }

                if (!string.IsNullOrEmpty(request.Country))
                {
                    filters.Add($"Country eq '{request.Country}'");
                }

                if (request.Star.HasValue)
                {
                    filters.Add($"Star eq {request.Star.Value}");
                }

                if (!string.IsNullOrEmpty(request.Label))
                {
                    filters.Add($"Label eq '{request.Label}'");
                }

                // Combine filters into one search expression
                string filterExpression = filters.Count > 0 ? string.Join(" and ", filters) : null;

                // Build search options
                var options = new SearchOptions
                {
                    Filter = filterExpression,
                    Size = request.Limit ?? DEFAULT_LIMIT // Default limit to 10 if not provided
                };

                // Execute search using Azure Cognitive Search
                var searchResults = await _searchClient.SearchAsync<AppReviewAI>("*", options);
                var resultsList = new List<AppReviewAI>();

                // Retrieve results from the response
                await foreach (var result in searchResults.GetResultsAsync())
                {
                    resultsList.Add(result.Document);
                }

                // Log the result count and return it as the ActionResult
                _logger.LogInformation($"SearchService::Search found {resultsList.Count} reviews.");
                return resultsList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"SearchService::Search Exception: {ex.StackTrace}");
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
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                // Search distinct platforms in Azure Cognitive Search
                var facets = await _searchClient.SearchAsync<AppReviewAI>("*", new SearchOptions
                {
                    Facets = { "Platform" }
                });

                var platforms = facets.Facets["Platform"].Select(facet => facet.Value.ToString()).ToList();
                return platforms;
            }
            catch (Exception ex)
            {
                _logger.LogError($"SearchService::GetPlatforms Exception: {ex.StackTrace}");
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
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                // Search distinct countries in Azure Cognitive Search
                var facets = await _searchClient.SearchAsync<AppReviewAI>("*", new SearchOptions
                {
                    Facets = { "Country" }
                });

                var countries = facets.Facets["Country"].Select(facet => facet.Value.ToString()).ToList();
                return countries;
            }
            catch (Exception ex)
            {
                _logger.LogError($"SearchService::GetCountries Exception: {ex.StackTrace}");
                throw new Exception("Error retrieving countries", ex);
            }
            finally
            {
                sw.Stop();
                _logger.LogInformation($"SearchService::GetCountries completed in {sw.ElapsedMilliseconds} ms.");
            }
        }

        // Get Stars
        public async Task<ActionResult<IEnumerable<int>>> GetStars()
        {
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                // Search distinct star ratings in Azure Cognitive Search
                var facets = await _searchClient.SearchAsync<AppReviewAI>("*", new SearchOptions
                {
                    Facets = { "Star" }
                });

                var stars = facets.Facets["Star"].Select(facet => int.Parse(facet.Value.ToString())).ToList();
                return stars;
            }
            catch (Exception ex)
            {
                _logger.LogError($"SearchService::GetStars Exception: {ex.StackTrace}");
                throw new Exception("Error retrieving star ratings", ex);
            }
            finally
            {
                sw.Stop();
                _logger.LogInformation($"SearchService::GetStars completed in {sw.ElapsedMilliseconds} ms.");
            }
        }

        // Get Labels
        public async Task<ActionResult<IEnumerable<string>>> GeLabels()
        {
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                // Search distinct labels in Azure Cognitive Search
                var facets = await _searchClient.SearchAsync<AppReviewAI>("*", new SearchOptions
                {
                    Facets = { "Label" }
                });

                var labels = facets.Facets["Label"].Select(facet => facet.Value.ToString()).ToList();
                return labels;
            }
            catch (Exception ex)
            {
                _logger.LogError($"SearchService::GeLabels Exception: {ex.StackTrace}");
                throw new Exception("Error retrieving labels", ex);
            }
            finally
            {
                sw.Stop();
                _logger.LogInformation($"SearchService::GeLabels completed in {sw.ElapsedMilliseconds} ms.");
            }
        }
    }
}
