using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowlerLib
{
    public class SafeValue<T>
    {
        private object LOCK = new object();
        private T value;

        public T Value
        {
            get
            {
                lock (LOCK)
                {
                    return this.value;
                }
            }
            set
            {
                lock (LOCK)
                {
                    this.value = value;
                }
            }
        }

        public SafeValue()
        {
            this.value = default(T);
        }

        public SafeValue(T initialValue)
        {
            this.value = initialValue;
        }
    }
}