using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odering.Domain.ValueObjects
{
    public record Payment
    {
        public string CardName { get; } = default!;
        public string CardNumber { get; } = default!;
        public string ExpiryDate { get; } = default!;
        public string CVV { get; } = default!;
        public int PaymentMethod { get; } = default!;
        public string PaymentRefNumber { get; } = default!;


        protected Payment()
        {

        }


        private Payment(string cardName, string cardNumber, string expiryDate, string cvv, int paymentMethod, string paymentRefNumber)
        {
            CardName = cardName;
            CardNumber = cardNumber;
            ExpiryDate = expiryDate;
            CVV = cvv;
            PaymentMethod = paymentMethod;
            PaymentRefNumber = paymentRefNumber;

        }

        public static Payment Of(string cardName, string cardNumber, string expiryDate, string cvv, int paymentMethod, string paymentRefNumber)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(cardName);
            ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber);
            //ArgumentException.ThrowIfNullOrWhiteSpace(expiryDate);
            ArgumentException.ThrowIfNullOrWhiteSpace(cvv);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3);

            return new Payment(cardName, cardNumber, expiryDate, cvv, paymentMethod, paymentRefNumber);

        }

    }
}
