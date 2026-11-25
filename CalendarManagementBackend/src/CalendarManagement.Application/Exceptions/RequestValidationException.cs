namespace CalendarManagement.Application.Exceptions
{
    public class RequestValidationException : Exception // det är en exception som används för att signalera valideringsfel i inkommande requests
    {
        public IDictionary<string, string[]> Errors { get; }

        public RequestValidationException(IDictionary<string, string[]> errors)
            : base("En eller flera valideringsfel uppstod.")
        {
            Errors = errors;
        }
    }
}