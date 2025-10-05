namespace WorkloadPlanner.Exceptions.Auth
{
    public class PasswordPolicyException : AuthException
    {
        public PasswordPolicyException(IEnumerable<string> errors) : base(errors) { }
    }
}
