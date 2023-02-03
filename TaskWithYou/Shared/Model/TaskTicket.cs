using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskWithYou.Shared.Model
{
    public class TaskTicket
    {
        public Guid Gid { get; set; }

        public int TourokuBi { get; set; }

        public string Name { get; set; }

        public int KigenBi { get; set; }

        public string Detail { get; set; }

        public TaskState State { get; set; } = new();
    }

    public class TaskState
    {
        public Guid Gid { get; set;}

        public string StateName { get; set; }

        public State State { get; set; }
    }

    public enum State
    {
        BeforeDoing = 100,
        Doing = 200,
        Finished = 300
    };
}
