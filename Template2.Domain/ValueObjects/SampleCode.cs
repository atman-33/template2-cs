using Template2.Domain.ValueObjects;

namespace Template2.Domain.ValueObjects
{
    public sealed class SampleCode : ValueObject<SampleCode>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value"></param>
        public SampleCode(string value)
        {
            Value = value;
        }

        public string Value { get; }

        protected override bool EqualsCore(SampleCode other)
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
