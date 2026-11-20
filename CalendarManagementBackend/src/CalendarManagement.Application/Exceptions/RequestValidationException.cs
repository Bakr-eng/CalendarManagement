namespace CalendarManagement.Application.Exceptions
{
    public class RequestValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public RequestValidationException(IDictionary<string, string[]> errors)
            : base("En eller flera valideringsfel uppstod.")
        {
            Errors = errors;
        }
    }
}