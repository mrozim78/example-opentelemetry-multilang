namespace OpenTelemetry.Example.Dotnet.Frontend.Models.View;

public class CheckoutViewModel
{
    public Guid BasketId { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Address { get; set; } = String.Empty;
    public string Town { get; set; } = String.Empty;
    public string PostalCode { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string CreditCard { get; set; } = String.Empty;
    public string CreditCardDate { get; set; } = String.Empty;
}