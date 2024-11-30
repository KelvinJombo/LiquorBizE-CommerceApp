namespace Odering.Domain.ValueObjects
{
    public record ProductId
    {
        public Guid Value { get; }

        private ProductId(Guid value) => Value = value;

        public static ProductId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("ProductId Cannot be Empty.");
            }

            return new ProductId(value);
        }
    }
}
