namespace WorkloadPlanner.Exceptions.Scrumboard
{
    public class ScrumboardException : Exception
    {
        public List<string> Errors { get; }

        public ScrumboardException(string error) : base(error)
        {
            Errors = [error];
        }

        public ScrumboardException(IEnumerable<string> errors) : base("One or more scrumboard errors found.")
        {
            Errors = [.. errors];
        }
    }
}