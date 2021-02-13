using System;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Extensions.Panels.ThreatSources
{
    public partial class CapecImportPanel : UserControl, IShowThreatModelPanel<Form>, ICustomRibbonExtension, IDesktopAlertAwareExtension, IInitializableObject
    {
        private readonly Guid _id = Guid.NewGuid();
        private IThreatModel _model;

        public CapecImportPanel()
        {
            InitializeComponent();
            _catalog.SourceUrl = ThreatsManager.Extensions.Properties.Resources.CapecDefaultUrl;
        }

        
        public event Action<string> ShowMessage;
        
        public event Action<string> ShowWarning;

        #region Implementation of interface IShowThreatModelPanel.
        public Guid Id => _id;

        public Form PanelContainer { get; set; }

        public void SetThreatModel([NotNull] IThreatModel threatModel)
        {
            _model = threatModel;
            _catalog.Model = threatModel;

            var propertySchemaManager = new CapecPropertySchemaManager(threatModel);
            var schema = propertySchemaManager.GetSchema();
            var propertyType = propertySchemaManager.GetHiddenPropertiesPropertyType();
            if (propertyType != null)
            {
                var property = _model.GetProperty(propertyType);
                if (property is IPropertyTokens propertyTokens)
                {
                    var hiddenProperties = propertyTokens.Value?.ToArray();
                    if (hiddenProperties?.Any() ?? false)
                    {
                        _catalog.SetHiddenProperties(hiddenProperties);
                    }
                }
            }

            _catalog.AutoLoad();
        }
        #endregion

        public bool IsInitialized => _model != null;
    }
}
