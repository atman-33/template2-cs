
namespace Template2.Domain.Exceptions
{
    public sealed class CsvException : ExceptionBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="exception">元になった例外</param>
        public CsvException(string message, Exception exception)
            : base(message, exception)
        {
            //// インナーエクセプション
        }

        /// <summary>
        /// 例外区分
        /// </summary>
        public override ExceptionKind Kind => ExceptionKind.Error;
    }
}
