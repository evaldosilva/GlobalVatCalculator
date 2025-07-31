using GlobalVatCalculator.API.Requests;
using GlobalVatCalculator.API.Results;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NFluent;
using System.Net;

namespace GlobalVatCalculator.IntegrationTests;

public class GlobalVatCalculatorIntegrationTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory = factory;
    private static readonly string _priceCalculatorEndpoint = "http://localhost:5044/api/v1/VatCalculator/PriceCalculator";

    private static HttpRequestMessage CreatePriceCalculatorEndpointHttpRequest(string queryParameter)
        => new()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(string.Concat(_priceCalculatorEndpoint, queryParameter)),
        };

    [Fact]
    public async Task Should_return_OK_When_send_valid_incomplete_payload()
    {
        PriceRequest priceRequest = new()
        {
            VATRate = 13,
            NetValue = 100
        };

        string query = $"?vatRate={priceRequest.VATRate}&netValue={priceRequest.NetValue}";
        HttpClient client = _factory.CreateClient();
        HttpRequestMessage request = CreatePriceCalculatorEndpointHttpRequest(query);

        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        Check.That(response.IsSuccessStatusCode).IsTrue();
        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

        string body = await response.Content.ReadAsStringAsync();
        var priceResult = JsonConvert.DeserializeObject<PriceResult>(body);

        Check.That(priceResult).IsNotNull();
        Check.That(priceResult?.VATTaxRate).IsEqualTo(13);
        Check.That(priceResult?.NetValue).IsEqualTo(100);
        Check.That(priceResult?.GrossValue).IsEqualTo(113);
        Check.That(priceResult?.VATValue).IsEqualTo(13);
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

        string query = $"?vatRate={priceRequest.VATRate}&netValue={priceRequest.NetValue}&grossValue={priceRequest.GrossValue}&vatValue={priceRequest.VATValue}";
        HttpClient client = _factory.CreateClient();
        HttpRequestMessage request = CreatePriceCalculatorEndpointHttpRequest(query);

        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        Check.That(response.IsSuccessStatusCode).IsTrue();
        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

        string body = await response.Content.ReadAsStringAsync();
        var priceResult = JsonConvert.DeserializeObject<PriceResult>(body);

        Check.That(priceResult).IsNotNull();
        Check.That(priceResult?.VATTaxRate).IsEqualTo(10);
        Check.That(priceResult?.NetValue).IsEqualTo(1000);
        Check.That(priceResult?.GrossValue).IsEqualTo(1100);
        Check.That(priceResult?.VATValue).IsEqualTo(100);
    }

    [Fact]
    public async Task Should_return_BadRequest_When_send_null_or_empty_payload()
    {
        HttpClient client = _factory.CreateClient();
        HttpRequestMessage request = CreatePriceCalculatorEndpointHttpRequest(string.Empty);

        using var response = await client.SendAsync(request);
        string body = await response.Content.ReadAsStringAsync();

        Check.That(response.IsSuccessStatusCode).IsFalse();
        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
    }

    [Theory, MemberData(nameof(WrongPriceRequests))]
    public async Task Should_return_BadRequest_When_send_multiple_invalid_payloads(PriceRequest priceRequest)
    {
        string query = $"?vatRate={priceRequest.VATRate}&netValue={priceRequest.NetValue}&grossValue={priceRequest.GrossValue}&vatValue={priceRequest.VATValue}";
        HttpClient client = _factory.CreateClient();
        HttpRequestMessage request = CreatePriceCalculatorEndpointHttpRequest(query);

        using var response = await client.SendAsync(request);

        Check.That(response.IsSuccessStatusCode).IsFalse();
        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
    }

    [Theory, MemberData(nameof(OverflowPriceRequests))]
    public async Task Should_return_InternalServerError_When_send_multiple_overflow_amount_payloads(PriceRequest priceRequest)
    {
        string query = $"?vatRate={priceRequest.VATRate}&netValue={priceRequest.NetValue}&grossValue={priceRequest.GrossValue}&vatValue={priceRequest.VATValue}";
        HttpClient client = _factory.CreateClient();
        HttpRequestMessage request = CreatePriceCalculatorEndpointHttpRequest(query);

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

    public static TheoryData<PriceRequest> OverflowPriceRequests
    {
        get => new()
        {
            { new()
                {
                    VATRate = 10,
                    GrossValue = 2342342342342343232432423423m,
                    NetValue = null,
                    VATValue = null
                }
            },
            { new()
                {
                    VATRate = 10,
                    GrossValue = null,
                    NetValue = null,
                    VATValue = 23423423423423432324324234239m
                }
            }
        };
    }
}