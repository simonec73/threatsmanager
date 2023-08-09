using System;
using System.Drawing;
using System.IO;
using Northwoods.Go;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    /// <summary>
    /// Summary description for GraphDoc.
    /// </summary>
    [Serializable]
    public class GraphDoc : GoDocument
    {
        public GraphDoc()
        {
            LinksLayer = Layers.CreateNewLayerAfter(Layers.Default);
            LinksLayer.Identifier = "Links";
            LinksLayer.AllowSelect = true;
            Layers.CreateNewLayerBefore(Layers.Default).Identifier = "bottom";
        }
    }
}