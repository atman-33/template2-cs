namespace Template2.Domain.ValueObjects
{
    public sealed class ImageLink : ValueObject<ImageLink>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name=""value""></param>
        public ImageLink(string? value)
        {
            Value = value;
        }

        public string? Value { get; }

        protected override bool EqualsCore(ImageLink other)
        {
            return Value == other.Value;
        }

        protected override int GetHashCodeCore()
        {
            if (Value == null)
            {
                return 0;
            }

            return Value.GetHashCode();
        }

        public override string ToString()
        {
            if (Value == null)
            {
                return String.Empty;
            }

            return Value.ToString();
        }
    }
}
