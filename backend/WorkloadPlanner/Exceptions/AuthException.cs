
namespace WorkloadPlanner.Exceptions
{
    public class AuthException : Exception
    {
        public List<string> Errors { get; }

        //Single error constructor
        public AuthException(string error) : base(error) 
        {
            Errors = [error];
        }

        //Multiple error constructor
        public AuthException(IEnumerable<string> errors) : base("One or more authentication errors occured.") 
        {
            Errors = [.. errors];
        }
    }
}
