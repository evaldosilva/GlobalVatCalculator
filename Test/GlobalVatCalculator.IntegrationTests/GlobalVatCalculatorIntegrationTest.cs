using GlobalVatCalculator.API.Requests;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NFluent;
using System.Net;
using System.Text;

namespace GlobalVatCalculator.IntegrationTests;

public class GlobalVatCalculatorIntegrationTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory = factory;
    private static readonly string _priceCalculatorEndpoint = "http://localhost:5044/api/v1/VatCalculator/PriceCalculator";
    private const string _jsonMediaType = "application/json";

    private static HttpRequestMessage CreatePriceCalculatorEndpointHttpRequest(string jsonPayload) 
        => new()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_priceCalculatorEndpoint),
                Content = new StringContent(jsonPayload ?? string.Empty, Encoding.UTF8, _jsonMediaType)
            };

    [Fact]
    public async Task Should_return_OK_When_send_valid_incomplete_payload()
    {
        PriceRequest priceRequest = new()
        {
            VATRate = 13,
            NetValue = 100
        };

        var json = JsonConvert.SerializeObject(priceRequest);

        HttpClient client = _factory.CreateClient();
        HttpRequestMessage request = CreatePriceCalculatorEndpointHttpRequest(json);

        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string body = await response.Content.ReadAsStringAsync();

        Check.That(response.IsSuccessStatusCode).IsTrue();
        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        Check.That(body).IsEqualIgnoringCase("{\"vatTaxRate\":13,\"netValue\":100.0,\"vatValue\":13.000,\"grossValue\":113.000}");
    }

    [Fact]
    public async Task Should_return_OK_When_send_valid_complete_payload()
    {
        PriceRequest priceRequest = new()
        {
            VATRate = 10,
            NetValue = 1000,
            GrossValue = null,
            VATValue = null
        };

        var json = JsonConvert.SerializeObject(priceRequest);

        HttpClient client = _factory.CreateClient();
        HttpRequestMessage request = CreatePriceCalculatorEndpointHttpRequest(json);

        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string body = await response.Content.ReadAsStringAsync();

        Check.That(response.IsSuccessStatusCode).IsTrue();
        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        Check.That(body).IsEqualIgnoringCase("{\"vatTaxRate\":10,\"netValue\":1000.0,\"vatValue\":100.00,\"grossValue\":1100.00}");
    }

    [Fact]
    public async Task Should_return_BadRequest_When_send_null_or_empty_payload()
    {
        HttpClient client = _factory.CreateClient();
        HttpRequestMessage request = CreatePriceCalculatorEndpointHttpRequest(null);

        using var response = await client.SendAsync(request);
        string body = await response.Content.ReadAsStringAsync();

        Check.That(response.IsSuccessStatusCode).IsFalse();
        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        Check.That(body).Contains("A non-empty request body is required.");
    }

    [Theory, MemberData(nameof(WrongPriceRequests))]
    public async Task Should_return_BadRequest_When_send_multiple_invalid_payloads(PriceRequest price)
    {
        var json = JsonConvert.SerializeObject(price);

        HttpClient client = _factory.CreateClient();
        HttpRequestMessage request = CreatePriceCalculatorEndpointHttpRequest(json);

        using var response = await client.SendAsync(request);

        Check.That(response.IsSuccessStatusCode).IsFalse();
        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
    }

    public static TheoryData<PriceRequest> WrongPriceRequests
    {
        get => new()
    {
        { new()
            {
                VATRate = 20,
                GrossValue = 0m,
                NetValue = 0m,
                VATValue = 0m
            }
        },
        { new()
            {
                VATRate = 20,
                GrossValue = null,
                NetValue = null,
                VATValue = null
            }
        },
        { new()
            {
                VATRate = 20,
                GrossValue = 120.00m,
                NetValue = 100.00m,
                VATValue = 20.00m
            }
        },
        { new()
            {
                VATRate = 20,
                GrossValue = null,
                NetValue = 100.00m,
                VATValue = 20.00m
            }
        },
        { new()
            {
                VATRate = 20,
                GrossValue = 120.00m,
                NetValue = null,
                VATValue = 20.00m
            }
        },
        { new()
            {
                VATRate = 20,
                GrossValue = 120.00m,
                NetValue = 100.00m,
                VATValue = null
            }
        },
        { new()
            {
                VATRate = 99,
                GrossValue = 120.00m,
                NetValue = null,
                VATValue = null
            }
        },
        { new()
            {
                VATRate = 1,
                GrossValue = 120.00m,
                NetValue = null,
                VATValue = null
            }
        },
        { new()
            {
                VATRate = 0,
                GrossValue = 120.00m,
                NetValue = null,
                VATValue = null
            }
        }
    };
    }
}