using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using RestSharp;

namespace Parser.Tests;

[TestFixture]
public class ParserApiTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _httpClient;
    private RestClient _restClient;

    [OneTimeSetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
        _httpClient = _factory.CreateClient();
        _restClient = new RestClient(_httpClient);
    }

    [Test]
    public async Task ShouldParseCsvSuccessfully()
    {
        var request = new RestRequest("/api/v1/parse-content", Method.Post);
        request.AddJsonBody(new
        {
            Type = "csv",
            Content = "bmFtZSxhZ2UKSm9obiwzMA=="
        });

        RestResponse response = await _restClient.ExecuteAsync(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.Content, Does.Contain("John"));
    }
    
    [Test]
    public async Task InvalidBase64()
    {
        var request = new RestRequest("/api/v1/parse-content", Method.Post);
        request.AddJsonBody(new
        {
            Type = "csv",
            Content = "InvalidBase64"
        });

        RestResponse response = await _restClient.ExecuteAsync(request);
        var jsonResponse = JsonSerializer.Deserialize<JsonElement>(response.Content);
        var detail = jsonResponse.GetProperty("detail").GetString();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        Assert.That(detail, Is.EqualTo("Invalid Base64 content."));
    }
    
    [Test]
    public async Task InvalidCsv()
    {
        var request = new RestRequest("/api/v1/parse-content", Method.Post);
        request.AddJsonBody(new
        {
            Type = "csv",
            Content = "IldpbGxpYW0gIkJpbGwiIEpvbmVzIiwiMzQ1IENhY3R1cyBEciIsIkNhbGlmb3JuaWEiCgo="
        });

        RestResponse response = await _restClient.ExecuteAsync(request);
        var jsonResponse = JsonSerializer.Deserialize<JsonElement>(response.Content);
        var detail = jsonResponse.GetProperty("detail").GetString();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        Assert.That(detail, Is.EqualTo("Invalid CSV content."));
    }
    
    [Test]
    public async Task ShouldParseJson()
    {
        var request = new RestRequest("/api/v1/parse-content", Method.Post);
        request.AddJsonBody(new
        {
            Type = "internal_json",
            Content = "ewogICJuYW1lIjogIkpvaG4iLAogICJhZ2UiOiAzMCwKICAiY2l0eSI6ICJOZXcgWW9yayIsCiAgImlzU3R1ZGVudCI6IHRydWUKfQoK"
        });

        RestResponse response = await _restClient.ExecuteAsync(request);
        var jsonResponse = JsonSerializer.Deserialize<JsonElement>(response.Content);
        var city = jsonResponse.GetProperty("records")[0].GetProperty("city").GetString();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(city, Does.Contain("New York"));
    }
    
    [Test]
    public async Task InvalidJson()
    {
        var request = new RestRequest("/api/v1/parse-content", Method.Post);
        request.AddJsonBody(new
        {
            Type = "internal_json",
            Content = "bmFtZSxhZ2UKSm9obiwzMA=="
        });

        RestResponse response = await _restClient.ExecuteAsync(request);
        var jsonResponse = JsonSerializer.Deserialize<JsonElement>(response.Content);
        var detail = jsonResponse.GetProperty("detail").GetString();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        Assert.That(detail, Is.EqualTo("Invalid JSON content."));
    }


    [OneTimeTearDown]
    public void TearDown()
    {
        _restClient?.Dispose();
        _httpClient?.Dispose();
        _factory?.Dispose();
    }
}