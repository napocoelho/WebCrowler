using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowlerLib
{
    public class SafeCounter
    {
        private object LOCK = new object();
        private int counter;

        public SafeCounter()
        {
            this.counter = 0;
        }

        public void Increase(int value = 1)
        {
            lock (LOCK)
            {
                counter += value;
            }
        }

        public void Decrease(int value = 1)
        {
            lock (LOCK)
            {
                counter -= value;
            }
        }

        public int Value
        {
            get
            {
                lock (LOCK)
                {
                    return this.counter;
                }
            }
        }
    }
}