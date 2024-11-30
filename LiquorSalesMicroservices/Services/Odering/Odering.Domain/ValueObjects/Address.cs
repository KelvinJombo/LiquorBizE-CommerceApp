namespace Odering.Domain.ValueObjects
{
    public record Address 
    {
        public string? FirstName { get; set; } = default!;
        public string? LastName { get; set; } = default!;
        public string? EmailAddress { get; set; } = default!;
        public string AddressLine {  get; set; } = default!;
        public string Country { get; set; } = default!;
        public string State { get; set; } = default!;
        public string Town { get; set; } = default!;


        protected Address()
        {

        }

        private Address(string firstName, string lastName, string emailAddress, string addressLine, string country, string state, string town)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            AddressLine = addressLine;
            Country = country;
            State = state;
            Town = town;
        }

        public static Address Of(string firstName, string lastName, string emailAddress, string addressLine, string country, string state, string town)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(emailAddress);
            ArgumentException.ThrowIfNullOrWhiteSpace(addressLine);

            return new Address(firstName, lastName, emailAddress, addressLine, country, state, town);
        }

    }
}
