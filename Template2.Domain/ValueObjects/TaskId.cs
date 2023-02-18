namespace Template2.Domain.ValueObjects
{
    public sealed class TaskId : ValueObject<TaskId>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name=""value""></param>
        public TaskId(int value)
        {
            Value = value;
        }

        public int Value { get; }

        protected override bool EqualsCore(TaskId other)
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
