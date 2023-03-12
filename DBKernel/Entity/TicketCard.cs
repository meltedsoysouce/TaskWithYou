namespace DBKernel.Entity
{
    public class TicketCard
    {
        public Guid Gid { get; set; }

        public float XCoordinate { get; set; }

        public float YCorrdinate { get; set; }

        public Guid TaskTicket { get; set; }
    }
}
