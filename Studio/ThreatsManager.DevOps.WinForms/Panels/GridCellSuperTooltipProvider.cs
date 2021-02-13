using System;
using System.ComponentModel;
using System.Drawing;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.DevOps.Panels
{
    public class GridCellSuperTooltipProvider : Component, DevComponents.DotNetBar.ISuperTooltipInfoProvider
    {
        private GridCell _cell;

       public GridCellSuperTooltipProvider([NotNull] GridCell cell)
        {
            _cell = cell;
        }

        public void Show()
        {
            DisplayTooltip?.Invoke(this, new EventArgs());
        }

        public void Hide()
        {
            HideTooltip?.Invoke(this, new EventArgs());
        }

        #region ISuperTooltipInfoProvider Members
        public System.Drawing.Rectangle ComponentRectangle
        {
            get
            {
                Rectangle r = _cell.Bounds;
                r.Location = _cell.SuperGrid.PointToScreen(_cell.LocationRelative);
                return r;
            }
        }

        
        public event EventHandler DisplayTooltip;
        
        public event EventHandler HideTooltip;
        #endregion
    }
}