
using Template2.Domain.Exceptions;

namespace SliTemplate2deshow.Domain.Exceptions
{
    public sealed class InputException : ExceptionBase
    {
        public InputException(string message) : base(message)
        {

        }

        public override ExceptionKind Kind => ExceptionKind.Info;
    }
}
