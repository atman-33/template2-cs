namespace Template2.Domain.ValueObjects
{
    public sealed class PageId : ValueObject<PageId>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name=""value""></param>
        public PageId(int value)
        {
            Value = value;
        }

        public int Value { get; }

        protected override bool EqualsCore(PageId other)
        {
            return Value == other.Value;
        }

        protected override int GetHashCodeCore()
        {
            if (Value as int? == null)
            {
                return 0;
            }

            return Value.GetHashCode();
        }

        public override string ToString()
        {
            if (Value as int? == null)
            {
                return string.Empty;
            }

            return Value.ToString();
        }
    }
}
