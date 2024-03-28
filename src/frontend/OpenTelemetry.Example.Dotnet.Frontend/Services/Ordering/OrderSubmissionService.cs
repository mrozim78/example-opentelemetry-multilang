

using OpenTelemetry.Example.Dotnet.Frontend.Models.Api;
using OpenTelemetry.Example.Dotnet.Frontend.Models.View;
using OpenTelemetry.Example.Dotnet.Frontend.Services.ShoppingBasket;
using OpenTelemetry.Example.Dotnet.Frontend.Telemetry;
using System.Diagnostics;
using System.Text.Json;

namespace OpenTelemetry.Example.Dotnet.Frontend.Services.Ordering;

public class OrderSubmissionService:IOrderSubmissionService
{
    private IShoppingBasketService _shoppingBasketService;
    private readonly HttpClient _client;
    private readonly IHttpClientFactory _httpClientFactory;
    private ILogger<OrderSubmissionService> _logger;
    private IAppActivitySource _activitySource;
    public OrderSubmissionService(IShoppingBasketService shoppingBasketService, IHttpClientFactory httpClientFactory, 
        ILogger<OrderSubmissionService> logger, IAppActivitySource activitySource)
    {
        _shoppingBasketService = shoppingBasketService;
        _httpClientFactory = httpClientFactory;
        _client = _httpClientFactory.CreateClient("ordering");
        _logger = logger;
        _activitySource = activitySource;
    }
        
    public async Task<Guid> SubmitOrder(CheckoutViewModel checkoutViewModel)
    {
        using Activity? activity = _activitySource.ActivitySource.StartActivity(ActivityKind.Client);
        activity?.AddEvent(new("Start sumbit order"));

        var lines = await _shoppingBasketService.GetLinesForBasket(checkoutViewModel.BasketId);
        var order = new OrderForCreation();
        order.Date = DateTimeOffset.Now;
        order.OrderId = Guid.NewGuid();
        order.Lines = lines.Select(line => new OrderLine() { 
            EventId = line.EventId, Price = line.Price, 
            TicketCount = line.TicketAmount }).ToList();
        order.CustomerDetails = new CustomerDetails()
        {
            Address = checkoutViewModel.Address,
            CreditCardNumber = checkoutViewModel.CreditCard,
            Email = checkoutViewModel.Email,
            Name = checkoutViewModel.Name,
            PostalCode = checkoutViewModel.PostalCode,
            Town = checkoutViewModel.Town,
            CreditCardExpiryDate = checkoutViewModel.CreditCardDate
        };
        await _client.PutAsJsonAsync("ordering", order);
        activity?.AddEvent(new("End sumbit order"));
        return order.OrderId;
    }
}