namespace WorkloadPlanner.Exceptions.Auth
{
    public class UserNotFoundException : AuthException
    {
        public UserNotFoundException() : base("User was not found.") {}
    }
}