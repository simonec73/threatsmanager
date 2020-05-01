using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Utilities
{
    public class SeverityComparer : IComparer<ISeverity>
    {
        public int Compare(ISeverity x, ISeverity y)
        {
            int result;

            if (x == null)
            {
                if (y == null)
                {
                    result = 0;
                }
                else
                {
                    result = -1;
                }
            }
            else
            {
                if (y == null)
                {
                    result = 1;
                }
                else
                {
                    result = x.Id - y.Id;
                }
            }

            return result;
        }
    }
}
