
namespace Template2.Domain.Modules.Helpers 
{ 
    /// <summary>
    /// Floatヘルパー
    /// </summary>
    public static class FloatHelper
    {
        /// <summary>
        /// 指定した桁数で四捨五入し、ゼロを埋める
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="decimalPoint">小数点以下桁数</param>
        /// <returns>値</returns>
        public static string RoundString(this float value, int decimalPoint)
        {
            value = Convert.ToSingle(Math.Round(value, decimalPoint));
            return value.ToString(decimalPoint == 0 ?
                string.Empty : "." + string.Concat(Enumerable.Repeat("0", decimalPoint)));
        }
    }
}
