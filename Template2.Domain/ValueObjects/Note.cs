namespace Template2.Domain.ValueObjects
{
    public sealed class Note : ValueObject<Note>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name=""value""></param>
        public Note(string? value)
        {
            Value = value;
        }

        public string? Value { get; }

        protected override bool EqualsCore(Note other)
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
                return string.Empty;
            }

            return Value.ToString();
        }
    }
}
