namespace WorkloadPlanner.Exceptions.Scrumboard
{
    public class ScrumBoardNotFoundException : Exception
    {
        public ScrumBoardNotFoundException() : base("Scrumboard was not found.") {}
    }
}