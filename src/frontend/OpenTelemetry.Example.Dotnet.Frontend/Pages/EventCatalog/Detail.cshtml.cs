
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenTelemetry.Example.Dotnet.Frontend.Extensions;
using OpenTelemetry.Example.Dotnet.Frontend.Models;
using OpenTelemetry.Example.Dotnet.Frontend.Models.Api;
using OpenTelemetry.Example.Dotnet.Frontend.Services.EventCatalog;
using OpenTelemetry.Example.Dotnet.Frontend.Services.ShoppingBasket;

namespace OpenTelemetry.Example.Dotnet.Frontend.Pages.EventCatalog;

public class Detail : PageModel
{
    public Event Event { get; set; }

    [BindProperty]
    public BasketLineForCreation BasketLineForCreation { get; set; }
    
    private IEventCatalogService _eventCatalogService;
    private IShoppingBasketService _basketService;
    private Settings _settings;
    
    public Detail(IEventCatalogService eventCatalogService , IShoppingBasketService basketService , Settings settings)
    { 
        _eventCatalogService = eventCatalogService;
        _basketService = basketService;
        _settings = settings;
    }

    public async Task OnGet(Guid eventId)
    {
        Event = await _eventCatalogService.GetEvent(eventId);
    }

    public async Task<IActionResult> OnPostAddToBasket()
    {
        if (ModelState.IsValid)
        {
            var basketId = Request.Cookies.GetCurrentBasketId(_settings);
            var newLine = await _basketService.AddToBasket(basketId, BasketLineForCreation);
            Response.Cookies.Append(_settings.BasketIdCookieName, newLine.BasketId.ToString());
            return RedirectToPage("/ShoppingBasket/List");
        }
        
        
        //Event = await _eventCatalogService.GetEvent(BasketLineForCreation.EventId);
        return Page();
    }
}