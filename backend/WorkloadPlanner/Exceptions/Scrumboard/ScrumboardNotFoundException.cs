namespace WorkloadPlanner.Exceptions.Scrumboard
{
    public class ScrumboardNotFoundException : ScrumboardException
    {
        public ScrumboardNotFoundException() : base("Scrumboard was not found.") {}
    }
}