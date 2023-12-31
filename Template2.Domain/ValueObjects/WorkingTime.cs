namespace Template2.Domain.ValueObjects
{
    public sealed class WorkingTime : ValueObject<WorkingTime>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value"></param>
        public WorkingTime(float? value)
        {
            Value = value;
        }

        public float? Value { get; }

        protected override bool EqualsCore(WorkingTime other)
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
