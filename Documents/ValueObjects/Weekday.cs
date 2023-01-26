namespace Template2.Domain.ValueObjects
{
    public sealed class Weekday : ValueObject<Weekday>
    {
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name=""value""></param>
        public Weekday(int value)
        {
            Value = value;
        }

        public int Value { get; }

        protected override bool EqualsCore(Weekday other)
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
