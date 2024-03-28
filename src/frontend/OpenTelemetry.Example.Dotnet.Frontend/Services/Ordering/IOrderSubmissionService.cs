using OpenTelemetry.Example.Dotnet.Frontend.Models.View;

namespace OpenTelemetry.Example.Dotnet.Frontend.Services.Ordering;

public interface IOrderSubmissionService
{
    Task<Guid> SubmitOrder(CheckoutViewModel checkoutViewModel);
}