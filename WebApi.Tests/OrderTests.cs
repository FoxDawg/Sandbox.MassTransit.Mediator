using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using WebApi.Abstractions;
using WebApi.Model;
using Xunit;

namespace WebApi.Tests;

public class OrderTests
{
    [Fact]
    public async Task Reads_Existing_Order()
    {
        var webApplicationFactory = new WebApplicationFactory<Program>();
        var client = webApplicationFactory.CreateClient();

        var response = await client.GetAsync("/orders?orderId=1");
        var result = await response.Content.ReadFromJsonAsync<OrderStatusResult>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().BeOfType<OrderStatusResult>();
        result.As<OrderStatusResult>().OrderId.Should().Be(1);
    }

    [Fact]
    public async Task NonExisting_Order_Gives_500()
    {
        var webApplicationFactory = new WebApplicationFactory<Program>();
        var client = webApplicationFactory.CreateClient();

        var response = await client.GetAsync("/orders?orderId=666");

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task Validates_OrderId()
    {
        var webApplicationFactory = new WebApplicationFactory<Program>();
        var client = webApplicationFactory.CreateClient();

        var response = await client.GetAsync("/orders?orderId=0");
        var result = await response.Content.ReadFromJsonAsync<ValidationErrors>();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Should().BeOfType<ValidationErrors>();
        result.As<ValidationErrors>().Errors.Should().NotBeEmpty();
    }
}