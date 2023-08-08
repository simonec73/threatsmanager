using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreatsManager.Extensions.Diagrams
{
    [Flags]
    public enum TooltipAction
    {
        None = 0,
        Edit = 1,
        Remove = 2
    }
}
