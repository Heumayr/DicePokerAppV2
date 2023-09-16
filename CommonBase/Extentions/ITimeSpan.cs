using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBase.Extentions
{
    public interface ITimeSpan 
    {
        public DateTime Begin { get;  set; }

        public DateTime End { get;  set; }

        public TimeSpan Duration => End - Begin;
    }
}
