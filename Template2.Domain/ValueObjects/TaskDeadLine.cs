namespace Template2.Domain.ValueObjects
{
    public sealed class TaskDeadLine : ValueObject<TaskDeadLine>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name=""value""></param>
        public TaskDeadLine(DateTime? value)
        {
            Value = value;
        }

        public DateTime? Value { get; }

        protected override bool EqualsCore(TaskDeadLine other)
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
