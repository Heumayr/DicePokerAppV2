using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBase.Structures
{
    public interface IAccessableByKey<out TKey>
    {
        public TKey Key { get; }
    }
}
