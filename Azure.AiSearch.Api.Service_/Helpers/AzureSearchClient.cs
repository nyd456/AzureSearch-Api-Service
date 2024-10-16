using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace AzureSearch.Api.Service.Helpers
{
    public class AzureSearchClient
    {
        SearchClient _client;

        public AzureSearchClient(SearchClient client)
        {
            _client = client;
        }
        public async Task<SearchResults<T>> SearchAsync<T>(SearchOptions options) where T : class
        {
            return await SearchAsync<T>(null, options);
        }
        public async Task<SearchResults<T>> SearchAsync<T>(string? search, SearchOptions options) where T : class
        {
            var response = await _client.SearchAsync<T>(search, options);

            return response.Value;
        }
    }
}
