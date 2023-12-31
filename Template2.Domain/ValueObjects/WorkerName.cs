namespace Template2.Domain.ValueObjects
{
    public sealed class WorkerName : ValueObject<WorkerName>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value"></param>
        public WorkerName(string value)
        {
            Value = value;
        }

        public string Value { get; }

        protected override bool EqualsCore(WorkerName other)
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
                return string.Empty;
            }

            return Value.ToString();
        }
    }
}
