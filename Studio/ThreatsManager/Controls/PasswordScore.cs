using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreatsManager.Interfaces;

namespace ThreatsManager.Controls
{
    public enum PasswordScore
    {
        Negligible = 0,
        [EnumLabel("Very weak")]
        VeryWeak = 1,
        Weak = 2,
        Medium = 3,
        Strong = 4,
        [EnumLabel("Very strong")]
        VeryStrong = 5
    }
}
