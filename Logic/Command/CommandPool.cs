using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render
{
    class CommandPool
    {
        CommandPool()
        {
            busys = new Queue<Command>();
            spares = new Queue<Command>();
        }
        Queue<Command> busys;
        Queue<Command> spares;

        private static CommandPool instance;
        public static CommandPool Instance
        {
            get
            {
                if (instance == null)
                    instance = new CommandPool();
                return instance;
            }
        }

        public int GetBusyCommandCount()
        {
            return busys.Count;
        }

        public void AddCommand(Delegate x,object[] param)
        {
            Command a;
            if (spares.Count > 0)
            {
                a = spares.Dequeue();
                a.SetCommand(x, param);
            }
            else
                a = new Command(x, param);
            busys.Enqueue(a);
        }

        public void AddCommand(Delegate x)
        {
            Command a;
            if (spares.Count > 0)
            {
                a = spares.Dequeue();
                a.SetCommand(x);
            }
            else
                a = new Command(x);
            busys.Enqueue(a);
        }

        public int Excute()
        {
            Command a;
            int count = GetBusyCommandCount();
            for(int i = 0; i < count; ++i)
            {
                a = busys.Dequeue();
                a.Excute();
                a.Clear();
                spares.Enqueue(a);
            }
            return count;
        }
    }
}
