namespace WorkloadPlanner.Exceptions.Auth
{
    public class InvalidCredentialException : AuthException
    {
        public InvalidCredentialException() : base("Invalid email or password.") { }
    }
}
