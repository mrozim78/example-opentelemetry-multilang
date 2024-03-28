

using OpenTelemetry.Example.Dotnet.Frontend.Models.Api;
using OpenTelemetry.Example.Dotnet.Frontend.Models.View;
using OpenTelemetry.Example.Dotnet.Frontend.Services.ShoppingBasket;
using System.Text.Json;

namespace OpenTelemetry.Example.Dotnet.Frontend.Services.Ordering.InMemory;

public class InMemoryOrderSubmissionService:IOrderSubmissionService
{
    private IShoppingBasketService _shoppingBasketService;
 
    private ILogger<InMemoryOrderSubmissionService> _logger;
    
    public InMemoryOrderSubmissionService(IShoppingBasketService shoppingBasketService,
        ILogger<InMemoryOrderSubmissionService> logger)
    {
        _shoppingBasketService = shoppingBasketService;
        _logger = logger;
    }
    
    public async Task<Guid> SubmitOrder(CheckoutViewModel checkoutViewModel)
    {
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
        _logger.LogInformation("Posting order event to Dapr pubsub");
        //await _daprClient.PublishEventAsync("pubsub", "orders", order);
        var options = new JsonSerializerOptions { WriteIndented = true };
        _logger.LogInformation(JsonSerializer.Serialize(order,options));
        return order.OrderId;
    }
}