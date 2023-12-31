using Template2.Domain.Exceptions;

namespace Template2.Domain.Modules.Helpers
{
    public static class Guard
    {
        public static void IsNull(object o, string message)
        {
            if (o == null)
            {
                throw new InputException(message);
            }
        }

        public static void IsNullOrEmpty(object o, string message)
        {
            if (o == null || Convert.ToString(o) == string.Empty)
            {
                throw new InputException(message);
            }
        }

        public static int IsInt(string text, string message)
        {
            int intValue;
            if (!int.TryParse(text, out intValue))
            {
                throw new InputException("int数値の入力に誤りがあります");
            }

            return intValue;
        }

        public static long IsLong(string text, string message)
        {
            long longValue;
            if (!long.TryParse(text, out longValue))
            {
                throw new InputException("long数値の入力に誤りがあります");
            }

            return longValue;
        }

        public static float IsFloat(string text, string message)
        {
            float floatValue;
            if (!float.TryParse(text, out floatValue))
            {
                throw new InputException("float数値の入力に誤りがあります");
            }

            return floatValue;
        }
    }
}
