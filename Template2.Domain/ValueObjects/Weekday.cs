namespace Template2.Domain.ValueObjects
{
    public sealed class Weekday : ValueObject<Weekday>
    {
        public static readonly Weekday Sunday = new Weekday(1);
        public static readonly Weekday Monday = new Weekday(2);
        public static readonly Weekday Tuesday = new Weekday(3);
        public static readonly Weekday Wednesday = new Weekday(4);
        public static readonly Weekday Thursday = new Weekday(5);
        public static readonly Weekday Friday = new Weekday(6);
        public static readonly Weekday Saturday = new Weekday(7);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value"></param>
        public Weekday(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public string DisplayValue
        {
            get
            {
                if (this == Sunday)
                {
                    return "日曜";
                }
                if (this == Monday)
                {
                    return "月曜";
                }
                if (this == Tuesday)
                {
                    return "火曜";
                }
                if (this == Wednesday)
                {
                    return "水曜";
                }
                if (this == Thursday)
                {
                    return "木曜";
                }
                if (this == Friday)
                {
                    return "金曜";
                }
                if (this == Saturday)
                {
                    return "土曜";
                }
                return "不明";
            }
        }

        protected override bool EqualsCore(Weekday other)
        {
            return Value == other.Value;
        }

        protected override int GetHashCodeCore()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            if (Value as int? == null)
            {
                return string.Empty;
            }

            return Value.ToString();
        }

        public static IList<Weekday> ToList()
        {
            return new List<Weekday>
            {
                Sunday,
                Monday,
                Tuesday,
                Wednesday,
                Thursday,
                Friday,
                Saturday
            };
        }
    }
}
