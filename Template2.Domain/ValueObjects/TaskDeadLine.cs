using System;
using System.Diagnostics;

namespace Template2.Domain.ValueObjects
{
    public sealed class TaskDeadline : ValueObject<TaskDeadline>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name=""value""></param>
        public TaskDeadline(DateTime? value)
        {
            Value = value;
        }

        public DateTime? Value { get; }

        protected override bool EqualsCore(TaskDeadline other)
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
            Debug.WriteLine(Value);

            if (Value == null || Value == new DateTime(0))
            {
                return String.Empty;
            }

            return Value.ToString();
        }
    }
}
