using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenTelemetry.Example.Dotnet.Frontend.Extensions;
using OpenTelemetry.Example.Dotnet.Frontend.Models;
using OpenTelemetry.Example.Dotnet.Frontend.Models.Api;
using OpenTelemetry.Example.Dotnet.Frontend.Models.View;
using OpenTelemetry.Example.Dotnet.Frontend.Services.EventCatalog;
using OpenTelemetry.Example.Dotnet.Frontend.Services.ShoppingBasket;

namespace OpenTelemetry.Example.Dotnet.Frontend.Pages.EventCatalog;

public class List : PageModel
{
    //public IEnumerable<Event> Events { get; private set; }
    public EventListModel ListModel { get; set; }
    private IEventCatalogService _eventCatalogService;
    private IShoppingBasketService _shoppingBasketService;
    private Settings _settings;
    private ILogger<List> _logger;
    public List(IEventCatalogService eventCatalogService,IShoppingBasketService shoppingBasketService,Settings settings,ILogger<List> logger)
    {
        
        _eventCatalogService = eventCatalogService;
        _shoppingBasketService = shoppingBasketService;
        _settings = settings;
        _logger = logger;
    }
    public async Task OnGet()
    {
        var currentBasketId = Request.Cookies.GetCurrentBasketId(_settings);
        var getBasket = _shoppingBasketService.GetBasket(currentBasketId);
        var getEvents = _eventCatalogService.GetAll();
        var errorMessage = string.Empty;
        try
        {
            await Task.WhenAll(new Task[] { getBasket, getEvents });
        } catch(Exception ex)
        {
            // could be due an mDNS failure to talk to event catalog service when running locally
            // https://github.com/dapr/dapr/issues/3256
            _logger.LogError(ex, "Failure fetching data for event catalog page");
            errorMessage = $"Unable to load data: {ex.Message}";
        }
        var numberOfItems = getBasket.IsCompletedSuccessfully ? getBasket.Result.NumberOfItems : 0;
        var events = getEvents.IsCompletedSuccessfully ? getEvents.Result : Array.Empty<Event>();
        //_logger.LogInformation( JsonSerializer.Serialize(events));

        ListModel =
            new EventListModel
            {
                Events = events,
                NumberOfItems = numberOfItems,
                ErrorMessage = errorMessage
            };

    }
}