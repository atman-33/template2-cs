
using Template2.Domain.Exceptions;

namespace Template2.Domain.Exceptions
{
    public sealed class InputException : ExceptionBase
    {
        public InputException(string message) : base(message)
        {

        }

        public override ExceptionKind Kind => ExceptionKind.Info;
    }
}
