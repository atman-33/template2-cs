namespace Template2.Domain.ValueObjects
{
    public sealed class WorkerCode : ValueObject<WorkerCode>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value"></param>
        public WorkerCode(string value)
        {
            Value = value;
        }

        public string Value { get; }

        protected override bool EqualsCore(WorkerCode other)
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
