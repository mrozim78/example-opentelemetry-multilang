using OpenTelemetry.Example.Dotnet.Frontend.Models.Api;

namespace OpenTelemetry.Example.Dotnet.Frontend.Services.ShoppingBasket;

public interface IShoppingBasketService
{
    Task<BasketLine> AddToBasket(Guid basketId, BasketLineForCreation basketLine);
    Task<IEnumerable<BasketLine>> GetLinesForBasket(Guid basketId);
    Task<Basket> GetBasket(Guid basketId);
    Task UpdateLine(Guid basketId, BasketLineForUpdate basketLineForUpdate);
    Task RemoveLine(Guid basketId, Guid lineId);
    Task ClearBasket(Guid currentBasketId);
}