namespace Template2.Domain.ValueObjects
{
    public sealed class PageName : ValueObject<PageName>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name=""value""></param>
        public PageName(string value)
        {
            Value = value;
        }

        public string Value { get; }

        protected override bool EqualsCore(PageName other)
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
