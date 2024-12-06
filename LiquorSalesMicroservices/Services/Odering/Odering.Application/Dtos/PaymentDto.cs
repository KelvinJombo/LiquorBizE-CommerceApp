namespace Odering.Application.Dtos
{
    public record PaymentDto(string CardName, string CardNumber, string ExpiryDate, string Cvv, int PaymentMethod, string PaymentRefNumber);
        
}
