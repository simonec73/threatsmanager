using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.QuantitativeRisk.Facts;

namespace ThreatsManager.QuantitativeRisk.Initializers
{
    public class BaseInitializer
    {
        protected void DoInitialize(IThreatModel model)
        {
            FactsManager.RegisterThreatModel(model);
        }
    }
}
