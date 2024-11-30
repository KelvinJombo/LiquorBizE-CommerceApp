namespace Odering.Domain.Models
{
    public class Customer : Entity<CustomerId>
    {
        public string Name { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string PhoneNumber { get; private set; } = default!;

        public static Customer Create(CustomerId id, string name, string email, string phoneNumber)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(phoneNumber);

            var customer = new Customer
            {
                Id = id,
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber

            };

            return customer;

        }
    }
}
