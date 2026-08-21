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
    public async Task ShouldParseCsvInvalidBase64()
    {
        var request = new RestRequest("/api/v1/parse-content", Method.Post);
        request.AddJsonBody(new
        {
            Type = "csv",
            Content = "abc"
        });

        RestResponse response = await _restClient.ExecuteAsync(request);
        var jsonResponse = JsonSerializer.Deserialize<JsonElement>(response.Content);
        var detail = jsonResponse.GetProperty("detail").GetString();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        Assert.That(detail, Is.EqualTo("Invalid Base64 content."));
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _restClient?.Dispose();
        _httpClient?.Dispose();
        _factory?.Dispose();
    }
}