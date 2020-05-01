using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Utilities
{
    public class StrengthComparer : IComparer<IStrength>
    {
        public int Compare(IStrength x, IStrength y)
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
