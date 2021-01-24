using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Quality.Annotations;
using ThreatsManager.Quality.Schemas;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality.Initializers
{
    [Extension("199E885E-0CCF-4F77-9A00-E9B88909F46C", "Annotations Initializer", 10, ExecutionMode.Business)]
    public class AnnotationsInitializer : IExtensionInitializer
    {
        public void Initialize()
        {
            KnownTypesBinder.AddKnownType(typeof(Annotations.Annotations));
        }
    }
}
