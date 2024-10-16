using Azure;
using Azure.AISearch.Api.Service.Models;
using Azure.AISearch.Api.Service.Services;
using Azure.Search.Documents;
using AzureSearch.Api.Service.Helpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add DB Context
builder.Services.AddDbContext<AppReviewContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("AppReviewContext")));

// Add Azure Search Client
builder.Services.AddSingleton<AzureSearchClient>(opt =>
{
    string serviceName = Utilities.GetAppSetting(Constants.AZURE_SEARCH_SERVICE_NAME);
    string indexName = Utilities.GetAppSetting(Constants.AZURE_SEARCH_INDEX_NAME);
    Uri serviceEndpoint = new Uri($"https://{serviceName}.search.windows.net/");
    var apiKey = Utilities.GetAppSetting(Constants.AZURE_SEARCH_API_KEY);
    AzureKeyCredential credential = new AzureKeyCredential(apiKey);

    // Create a AzureSearchClient to load and query documents
    var srchclient = new SearchClient(serviceEndpoint, indexName, credential);
    return new AzureSearchClient(srchclient);
});

// Add Service
builder.Services.AddScoped<ISearchServiceSql, SearchServiceSqlL>();
builder.Services.AddScoped<ISearchServiceAzure, SearchServiceAzure>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
