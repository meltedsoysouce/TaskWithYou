using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskWithYou.Shared.Model
{
    public class TaskState
    {
        public Guid Gid { get; set; }

        public string Name { get; set; }

        public State State { get; set; }
    }

    public enum State
    {
        BeforeDoing = 100,
        Doing = 200,
        Finished = 300
    };
}
