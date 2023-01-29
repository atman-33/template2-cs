namespace Template2.Domain.ValueObjects
{
    public sealed class ImagePageNo : ValueObject<ImagePageNo>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name=""value""></param>
        public ImagePageNo(int? value)
        {
            Value = value;
        }

        public int? Value { get; }

        protected override bool EqualsCore(ImagePageNo other)
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
