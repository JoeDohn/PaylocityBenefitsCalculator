using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ApiTests;

public class IntegrationTest : IDisposable
{
    private HttpClient? _httpClient;

    public HttpClient HttpClient
    {
        get
        {
            if (_httpClient == default)
            {
                _httpClient = new WebApplicationFactory<Program>().CreateClient();
                _httpClient.BaseAddress = new Uri("https://localhost:7124");
                _httpClient.DefaultRequestHeaders.Add("accept", "text/plain");
            }

            return _httpClient;
        }
    }

    public void Dispose()
    {
        HttpClient.Dispose();
    }
}

