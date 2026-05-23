namespace WorkloadPlanner.Exceptions.Ticket
{
    public class TicketNotFoundException : Exception
    {
        public TicketNotFoundException() : base("Ticket was not found.") {}
    }
}