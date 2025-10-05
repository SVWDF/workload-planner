namespace WorkloadPlanner.Exceptions.Auth
{
    public class UserAlreadyExistsException : AuthException
    {
        public UserAlreadyExistsException(string email) : base($"An user with email '{email}' already exists.") { }
    }
}
