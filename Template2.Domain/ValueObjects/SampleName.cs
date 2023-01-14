namespace Template2.Domain.ValueObjects
{
    public sealed class SampleName : ValueObject<SampleName>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value"></param>
        public SampleName(string value)
        {
            Value = value;
        }

        public string Value { get; }

        protected override bool EqualsCore(SampleName other)
        {
            return Value == other.Value;
        }

        protected override int GetHashCodeCore()
        {
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
