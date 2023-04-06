using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskWithYou.Shared.Model
{
    public class TicketCard
    {
        public Guid Gid { get; set; }

        public float XCoordinate { get; set; }

        public float YCoordinate { get; set; }
        
        public Guid TaskTicket { get; set; }
        //public TaskTicket Task { get; set; }
    }
}
