using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ThreatsManager.Extensions.Dialogs
{
    internal class AdjustFactorParams
    {
        public AdjustFactorParams(Guid diagramId, int adjustmentFactor) 
        {
            DiagramId = diagramId;
            Factor = adjustmentFactor;
        }

        public AdjustFactorParams(int adjustmentFactor)
        {
            EveryDiagram = true;
            Factor = adjustmentFactor;
        }

        public bool EveryDiagram { get; private set; }
        public Guid DiagramId { get; private set; }
        public int Factor { get; private set; }
    }
}
