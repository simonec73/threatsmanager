using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using Newtonsoft.Json;
using ThreatsManager.Packaging;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms.Dialogs;

namespace ThreatsManager.Extensions.Dialogs
{
    public partial class MergeDialog : Form, IExecutionModeSupport
    {
        private IThreatModel _model;
        private IThreatModel _comparison;
        private bool _fullComparison;
        private static readonly IEnumerable<IPropertySchemasExtractor> _extractors;
        private ExecutionMode _executionMode = ExecutionMode.Expert;
        private FilterMode _filterMode = FilterMode.All;

        private const string ThreatModelFile = "threatmodel.json";

        private enum FilterMode
        {
            All,
            Added,
            Changed,
            Removed
        }

        static MergeDialog()
        {
            _extractors = ExtensionUtils.GetExtensions<IPropertySchemasExtractor>();
        }

        public MergeDialog()
        {
            InitializeComponent();

            InitializeGrid(_gridExternalInteractors);
            InitializeGrid(_gridProcesses);
            InitializeGrid(_gridDataStores);
            InitializeGrid(_gridFlows);
            InitializeGrid(_gridTrustBoundaries);
            InitializeGrid(_gridDiagrams);
            InitializeGrid(_gridThreatTypes);
            InitializeGrid(_gridMitigations);
            InitializeGrid(_gridItemTemplates);
            InitializeGrid(_gridPropertySchemas);
            InitializeGrid(_gridThreatActors);
            InitializeGrid(_gridSeverities);
            InitializeGrid(_gridStrengths);
        }

        #region Loading.
        public bool Initialize([NotNull] IThreatModel model, [Required] string comparisonSourceFile)
        {
            bool result;

            _model = model;

            if (comparisonSourceFile.EndsWith(".tm", StringComparison.InvariantCultureIgnoreCase))
            {
                _fullComparison = true;

                var latest = GetLatest(comparisonSourceFile, out var dateTime);
                if (latest != null && 
                    MessageBox.Show(Form.ActiveForm, 
                        $"A newer version is available, created on {dateTime.ToString("g")}.\nDo you want to open the newest version instead?",
                        "Open newest version", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    comparisonSourceFile = latest;
                }

                _sourceFile.Text = comparisonSourceFile;
                result = LoadThreatModel(comparisonSourceFile);
            } else if (comparisonSourceFile.EndsWith(".tmt", StringComparison.InvariantCultureIgnoreCase) ||
                       comparisonSourceFile.EndsWith(".tmk", StringComparison.InvariantCultureIgnoreCase))
            {
                _fullComparison = false;
                _sourceFile.Text = comparisonSourceFile;
                result = LoadTemplate(comparisonSourceFile);
            }
            else
            {
                result = false;
            }

            return result;
        }

        public string GetLatest([Required] string location, out DateTime dateTime)
        {
            string result = null;
            dateTime = DateTime.MinValue;

            var directory = Path.GetDirectoryName(location);
            var filter = string.Concat(Path.GetFileNameWithoutExtension(location), "_??????????????", ".tm");

            if (!string.IsNullOrWhiteSpace(directory))
            {
                result = Directory
                    .GetFiles(directory, filter, SearchOption.TopDirectoryOnly)
                    .OrderByDescending(x => x)
                    .FirstOrDefault();

                if (result != null)
                    dateTime = GetDateTime(result);
            }

            return result;
        }

        private DateTime GetDateTime([Required] string text)
        {
            DateTime result = DateTime.MinValue;

            Regex regex = new Regex("(_[0-9]{14})");
            var match = regex.Match(text);
            if (match.Success)
            {
                var capture = match.Captures.OfType<Capture>().FirstOrDefault();
                if (capture != null)
                {
                    result = DateTime.ParseExact(capture.Value.Substring(1), "yyyyMMddHHmmss", 
                        System.Globalization.CultureInfo.InvariantCulture);
                }
            }

            return result;
        }

        private bool LoadThreatModel([Required] string sourceFile)
        {
            bool result = false;

            if (File.Exists(sourceFile))
            {
                var package = new Package(sourceFile);

                var threatModelContent = package.Read(ThreatModelFile);
                if (threatModelContent != null)
                {
                    try
                    {
                        _comparison = ThreatModelManager.Deserialize(threatModelContent, true, Guid.NewGuid());
                        result = DoComparison();
                    }
                    catch
                    {
                    }
                }
            }

            return result;
        }

        private bool LoadTemplate([Required] string sourceFile)
        {
            bool result = false;

            if (File.Exists(sourceFile))
            {
                try
                {
                    _comparison = TemplateManager.OpenTemplate(sourceFile);
                    result = DoComparison();
                }
                catch (JsonSerializationException)
                {
                }
            }

            return result;
        }

        private bool DoComparison()
        {
            if (_fullComparison)
            {
                _tabExternalInteractors.Visible = AddItems(_gridExternalInteractors, 
                    Compare(_comparison?.Entities?.OfType<IExternalInteractor>(),
                        _model?.Entities?.OfType<IExternalInteractor>()));
                _tabProcesses.Visible = AddItems(_gridProcesses,
                    Compare(_comparison?.Entities?.OfType<IProcess>(),
                        _model?.Entities?.OfType<IProcess>()));
                _tabDataStores.Visible = AddItems(_gridDataStores,
                    Compare(_comparison?.Entities?.OfType<IDataStore>(),
                        _model?.Entities?.OfType<IDataStore>()));
                _tabFlows.Visible = AddItems(_gridFlows, Compare(_comparison?.DataFlows, _model?.DataFlows));
                _tabTrustBoundaries.Visible = AddItems(_gridTrustBoundaries, Compare(_comparison?.Groups?.OfType<ITrustBoundary>(), 
                    _model?.Groups?.OfType<ITrustBoundary>()));
                _tabDiagrams.Visible = AddItems(_gridDiagrams, Compare(_comparison?.Diagrams, _model?.Diagrams));
            }
            else
            {
                _tabExternalInteractors.Visible = false;
                _tabProcesses.Visible = false;
                _tabDataStores.Visible = false;
                _tabFlows.Visible = false;
                _tabTrustBoundaries.Visible = false;
                _tabDiagrams.Visible = false;
            }

            _tabThreatTypes.Visible = AddItems(_gridThreatTypes, Compare(_comparison?.ThreatTypes, _model?.ThreatTypes));
            _tabMitigations.Visible = AddItems(_gridMitigations, Compare(_comparison?.Mitigations, _model?.Mitigations));
            bool templatesVisible = AddItems(_gridItemTemplates,
                Compare(_comparison?.EntityTemplates, _model?.EntityTemplates));
            templatesVisible |=
                AddItems(_gridItemTemplates, Compare(_comparison?.FlowTemplates, _model?.FlowTemplates));
            templatesVisible |= AddItems(_gridItemTemplates,
                Compare(_comparison?.TrustBoundaryTemplates, _model?.TrustBoundaryTemplates));
            _tabItemTemplates.Visible = templatesVisible;
            _tabPropertySchemas.Visible = AddItems(_gridPropertySchemas, Compare(_comparison?.Schemas, _model?.Schemas));
            _tabThreatActors.Visible = AddItems(_gridThreatActors, Compare(_comparison?.ThreatActors, _model?.ThreatActors));
            _tabSeverities.Visible = AddItems(_gridSeverities, Compare(_comparison?.Severities, _model?.Severities));
            _tabStrengths.Visible = AddItems(_gridStrengths, Compare(_comparison?.Strengths, _model?.Strengths));

            return true;
        }

        private bool AddItems([NotNull] SuperGridControl control, IEnumerable<ComparedObject> objects)
        {
            var result = false;

            var items = objects?.OrderBy(x => x.ToString()).ToArray();
            if (items?.Any() ?? false)
            {
                result = true;

                foreach (var item in items)
                {
                    var differences = item.Differences ??
                                      (item.Source != null ? "Create Item" : "Remove Item");
                    var row = new GridRow(false, item.Action, item.ToString(), differences)
                    {
                        Tag = item
                    };

                    control.PrimaryGrid.Rows.Add(row);
                }
            }

            return result;
        }
        #endregion

        #region Grids Initialization.
        private void InitializeGrid([NotNull] SuperGridControl grid)
        {
            lock (grid)
            {
                GridPanel panel = grid.PrimaryGrid;
                panel.ShowTreeButtons = false;
                panel.AllowRowDelete = false;
                panel.AllowRowInsert = false;
                panel.ShowRowDirtyMarker = false;
                panel.InitialActiveRow = RelativeRow.None;

                panel.Columns.Add(new GridColumn("Merge")
                {
                    HeaderText = "Merge",
                    Width = 75,
                    DataType = typeof(bool),
                    EditorType = typeof(GridSwitchButtonEditControl),
                    AllowEdit = true
                });
                panel.Columns.Add(new GridColumn("Action")
                {
                    HeaderText = "Action",
                    Width = 50,
                    DataType = typeof(string),
                    AllowEdit = false
                });
                panel.Columns.Add(new GridColumn("Name")
                {
                    HeaderText = "Name",
                    AutoSizeMode = ColumnAutoSizeMode.Fill,
                    DataType = typeof(string),
                    AllowEdit = false
                });
                panel.Columns.Add(new GridColumn("Details")
                {
                    HeaderText = "Details",
                    DataType = typeof(string),
                    Width = 400,
                    EditorType = typeof(GridButtonXEditControl)
                });
                var bc = panel.Columns["Details"].EditControl as GridButtonXEditControl;
                if (bc != null)
                {
                    bc.Click += BcButtonClick;
                }
            }

        }

        private void BcButtonClick(object sender, EventArgs e)
        {
            if (sender is GridButtonXEditControl bc && 
                bc.EditorCell?.GridRow?.Tag is ComparedObject comparedObject)
            {
                if (comparedObject.Source != null && comparedObject.Target != null)
                {
                    using (var dialog = new ComparisonDialog())
                    {
                        dialog.SetExecutionMode(_executionMode);
                        dialog.Initialize(comparedObject.Source, comparedObject.Target);
                        dialog.ShowDialog(Form.ActiveForm);
                    }
                }
                else if (comparedObject.Source != null)
                {
                    using (var dialog = new ItemEditorDialog())
                    {
                        dialog.ReadOnly = true;
                        dialog.SetExecutionMode(_executionMode);
                        dialog.Item = comparedObject.Source;
                        dialog.Text = "Item to be created";
                        dialog.ShowDialog(Form.ActiveForm);
                    }
                }
                else if (comparedObject.Target != null)
                {
                    using (var dialog = new ItemEditorDialog())
                    {
                        dialog.ReadOnly = true;
                        dialog.SetExecutionMode(_executionMode);
                        dialog.Item = comparedObject.Target;
                        dialog.Text = "Item to be removed";
                        dialog.ShowDialog(Form.ActiveForm);
                    }
                }
            }
        }
        #endregion

        #region Comparison.
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int memcmp(IntPtr b1, IntPtr b2, UIntPtr count);

        private static bool Compare(Bitmap b1, Bitmap b2)
        {
            if ((b1 == null) != (b2 == null)) return false;
            if (b1 == null && b2 == null) return true;
            if (b1.Size != b2.Size) return false;

            var bd1 = b1.LockBits(new Rectangle(new Point(0, 0), b1.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var bd2 = b2.LockBits(new Rectangle(new Point(0, 0), b2.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            try
            {
                IntPtr bd1scan0 = bd1.Scan0;
                IntPtr bd2scan0 = bd2.Scan0;

                int stride = bd1.Stride;
                int len = stride * b1.Height;

                return memcmp(bd1scan0, bd2scan0, new UIntPtr((uint)len)) == 0;
            }
            finally
            {
                b1?.UnlockBits(bd1);
                b2?.UnlockBits(bd2);
            }
        }

        private IEnumerable<ComparedObject> Compare(IEnumerable<IEntity> sources, IEnumerable<IEntity> targets)
        {
            IEnumerable<ComparedObject> result = null;

            List<ComparedObject> list = new List<ComparedObject>();

            var sList = sources?.ToArray();
            var tList = new List<IEntity>();
            if (targets?.Any() ?? false)
                tList.AddRange(targets);

            if ((sList?.Any() ?? false) && tList.Any())
            {
                foreach (var s in sList)
                {
                    var t = tList.FirstOrDefault(x => x.Id == s.Id);
                    if (t == null)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                    else
                    {
                        var identities = CompareIdentities(s, t);
                        var propertiesContainers = ComparePropertiesContainers(s, t);
                        var parentGroups = CompareParentGroups(s, t);
                        var threatEventsContainers = CompareThreatEventsContainers(s, t);
                        var images = Compare(s.BigImage, t.BigImage) && Compare(s.Image, t.Image) &&
                                     Compare(s.SmallImage, t.SmallImage);
                        if (!identities || !propertiesContainers || !parentGroups || !threatEventsContainers || !images)
                        {
                            var comparedObject = new ComparedObject(s, t);
                            if (!identities)
                                comparedObject.AddDifference("Name or Description");
                            if (!propertiesContainers)
                                comparedObject.AddDifference("Properties");
                            if (!parentGroups)
                                comparedObject.AddDifference("Parent");
                            if (!threatEventsContainers)
                                comparedObject.AddDifference("Threat Events");
                            if (!images)
                                comparedObject.AddDifference("Images");
                            list.Add(comparedObject);
                            tList.Remove(t);
                        }
                        else
                        {
                            tList.Remove(t);
                        }
                    }
                }

                foreach (var t in tList)
                {
                    list.Add(new ComparedObject(null, t));
                }
            }
            else
            {
                if (sList?.Any() ?? false)
                {
                    foreach (var s in sList)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                } 
                else if (tList.Any())
                {
                    foreach (var t in tList)
                    {
                        list.Add(new ComparedObject(null, t));
                    }
                }
            }

            if (list.Any())
                result = list;

            return result;
        }

        private IEnumerable<ComparedObject> Compare(IEnumerable<IDataFlow> sources, IEnumerable<IDataFlow> targets)
        {
            IEnumerable<ComparedObject> result = null;

            List<ComparedObject> list = new List<ComparedObject>();

            var sList = sources?.ToArray();
            var tList = new List<IDataFlow>();
            if (targets?.Any() ?? false)
                tList.AddRange(targets);

            if ((sList?.Any() ?? false) && tList.Any())
            {
                foreach (var s in sList)
                {
                    var t = tList.FirstOrDefault(x => x.Id == s.Id);
                    if (t == null)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                    else if (s.SourceId == t.SourceId && s.TargetId == t.TargetId)
                    {
                        var identities = CompareIdentities(s, t);
                        var propertiesContainers = ComparePropertiesContainers(s, t);
                        var threatEventsContainers = CompareThreatEventsContainers(s, t);
                        var flowType = s.FlowType == t.FlowType;
                        if (!identities || !propertiesContainers || !threatEventsContainers || !flowType)
                        {
                            var comparedObject = new ComparedObject(s, t);
                            if (!identities)
                                comparedObject.AddDifference("Name or Description");
                            if (!propertiesContainers)
                                comparedObject.AddDifference("Properties");
                            if (!threatEventsContainers)
                                comparedObject.AddDifference("Threat Events");
                            if (!flowType)
                                comparedObject.AddDifference("Flow Type");
                            list.Add(comparedObject);
                            tList.Remove(t);
                        }
                        else
                        {
                            tList.Remove(t);
                        }
                    }
                    else
                    {
                        // Note: if Source or Target do not match, then the two Flows are not the same even if the Id corresponds.
                        // TODO: Given that this is an anomalous situation, the flow will be ignored but some telemetry should be generated.
                    }
                }

                foreach (var t in tList)
                {
                    list.Add(new ComparedObject(null, t));
                }
            }
            else
            {
                if (sList?.Any() ?? false)
                {
                    foreach (var s in sList)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                } 
                else if (tList.Any())
                {
                    foreach (var t in tList)
                    {
                        list.Add(new ComparedObject(null, t));
                    }
                }
            }

            if (list.Any())
                result = list;

            return result;
        }

        private IEnumerable<ComparedObject> Compare(IEnumerable<IGroup> sources, IEnumerable<IGroup> targets)
        {
            IEnumerable<ComparedObject> result = null;

            List<ComparedObject> list = new List<ComparedObject>();

            var sList = sources?.ToArray();
            var tList = new List<IGroup>();
            if (targets?.Any() ?? false)
                tList.AddRange(targets);

            if ((sList?.Any() ?? false) && tList.Any())
            {
                foreach (var s in sList)
                {
                    var t = tList.FirstOrDefault(x => x.Id == s.Id);
                    if (t == null)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                    else
                    {
                        var identities = CompareIdentities(s, t);
                        var propertiesContainers = ComparePropertiesContainers(s, t);
                        var parentGroups = s is IGroupElement sGE && t is IGroupElement tGE && 
                                           CompareParentGroups(sGE, tGE);
                        if (!identities || !propertiesContainers || !parentGroups)
                        {
                            var comparedObject = new ComparedObject(s, t);
                            if (!identities)
                                comparedObject.AddDifference("Name or Description");
                            if (!propertiesContainers)
                                comparedObject.AddDifference("Properties");
                            if (!parentGroups)
                                comparedObject.AddDifference("Parent");
                            list.Add(comparedObject);
                            tList.Remove(t);
                        }
                        else
                        {
                            tList.Remove(t);
                        }
                    }
                }

                foreach (var t in tList)
                {
                    list.Add(new ComparedObject(null, t));
                }
            }
            else
            {
                if (sList?.Any() ?? false)
                {
                    foreach (var s in sList)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                } 
                else if (tList.Any())
                {
                    foreach (var t in tList)
                    {
                        list.Add(new ComparedObject(null, t));
                    }
                }
            }

            if (list.Any())
                result = list;

            return result;
        }

        private IEnumerable<ComparedObject> Compare(IEnumerable<IDiagram> sources, IEnumerable<IDiagram> targets)
        {
            IEnumerable<ComparedObject> result = null;

            List<ComparedObject> list = new List<ComparedObject>();

            var sList = sources?.ToArray();
            var tList = new List<IDiagram>();
            if (targets?.Any() ?? false)
                tList.AddRange(targets);

            if ((sList?.Any() ?? false) && tList.Any())
            {
                foreach (var s in sList)
                {
                    var t = tList.FirstOrDefault(x => x.Id == s.Id);
                    if (t == null)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                    else
                    {
                        var identities = CompareIdentities(s, t);
                        var propertiesContainers = ComparePropertiesContainers(s, t);
                        var entityShapes = CompareEntityShapesContainers(s, t);
                        var groupShapes = CompareGroupShapesContainers(s, t);
                        var links = CompareLinksContainer(s, t);
                        var order = s.Order == t.Order;
                        if (!identities || !propertiesContainers || !entityShapes || !groupShapes || !links || !order)
                        {
                            var comparedObject = new ComparedObject(s, t);
                            if (!identities)
                                comparedObject.AddDifference("Name or Description");
                            if (!propertiesContainers)
                                comparedObject.AddDifference("Properties");
                            if (!entityShapes)
                                comparedObject.AddDifference("Entity Shapes");
                            if (!groupShapes)
                                comparedObject.AddDifference("Group Shapes");
                            if (!links)
                                comparedObject.AddDifference("Links");
                            if (!order)
                                comparedObject.AddDifference("Order");
                            list.Add(comparedObject);
                            tList.Remove(t);
                        }
                        else
                        {
                            tList.Remove(t);
                        }
                    }
                }

                foreach (var t in tList)
                {
                    list.Add(new ComparedObject(null, t));
                }
            }
            else
            {
                if (sList?.Any() ?? false)
                {
                    foreach (var s in sList)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                } 
                else if (tList.Any())
                {
                    foreach (var t in tList)
                    {
                        list.Add(new ComparedObject(null, t));
                    }
                }
            }

            if (list.Any())
                result = list;

            return result;
        }

        private IEnumerable<ComparedObject> Compare(IEnumerable<IThreatType> sources, IEnumerable<IThreatType> targets)
        {
            IEnumerable<ComparedObject> result = null;

            List<ComparedObject> list = new List<ComparedObject>();

            var sList = sources?.ToArray();
            var tList = new List<IThreatType>();
            if (targets?.Any() ?? false)
                tList.AddRange(targets);

            if ((sList?.Any() ?? false) && tList.Any())
            {
                foreach (var s in sList)
                {
                    var t = tList.FirstOrDefault(x => x.Id == s.Id);
                    if (t == null)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                    else
                    {
                        var identities = CompareIdentities(s, t);
                        var propertiesContainers = ComparePropertiesContainers(s, t);
                        var threatTypeMitigations = CompareThreatTypeMitigationsContainers(s, t);
                        var severities = (s.Severity?.Id ?? -1) == (t.Severity?.Id ?? -1);
                        if (!identities || !propertiesContainers || !threatTypeMitigations || !severities)
                        {
                            var comparedObject = new ComparedObject(s, t);
                            if (!identities)
                                comparedObject.AddDifference("Name or Description");
                            if (!propertiesContainers)
                                comparedObject.AddDifference("Properties");
                            if (!threatTypeMitigations)
                                comparedObject.AddDifference("Threat Type Mitigations");
                            if (!severities)
                                comparedObject.AddDifference("Severities");
                            list.Add(comparedObject);
                            tList.Remove(t);
                        }
                        else
                        {
                            tList.Remove(t);
                        }
                    }
                }

                foreach (var t in tList)
                {
                    list.Add(new ComparedObject(null, t));
                }
            }
            else
            {
                if (sList?.Any() ?? false)
                {
                    foreach (var s in sList)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                } 
                else if (tList.Any())
                {
                    foreach (var t in tList)
                    {
                        list.Add(new ComparedObject(null, t));
                    }
                }
            }

            if (list.Any())
                result = list;

            return result;
        }

        private IEnumerable<ComparedObject> Compare(IEnumerable<IPropertySchema> sources, IEnumerable<IPropertySchema> targets)
        {
            IEnumerable<ComparedObject> result = null;

            List<ComparedObject> list = new List<ComparedObject>();

            var sList = sources?.ToArray();
            var tList = new List<IPropertySchema>();
            if (targets?.Any() ?? false)
                tList.AddRange(targets);

            if ((sList?.Any() ?? false) && tList.Any())
            {
                foreach (var s in sList)
                {
                    var t = tList.FirstOrDefault(x => (string.CompareOrdinal(x.Name, s.Name) == 0) &&
                                                      (string.CompareOrdinal(x.Namespace, s.Namespace) == 0));
                    if (t == null)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                    else
                    {
                        var identities = CompareIdentities(s, t);
                        var propertyTypesContainers = ComparePropertyTypesContainers(s, t);
                        var appliesTo = s.AppliesTo == t.AppliesTo;
                        var autoApply = s.AutoApply == t.AutoApply;
                        var priority = s.Priority == t.Priority;
                        var visible = s.Visible == t.Visible;
                        var system = s.System == t.System;
                        if (!identities || !propertyTypesContainers || !appliesTo ||
                            !autoApply || !priority || !visible || !system)
                        {
                            var comparedObject = new ComparedObject(s, t);
                            if (!identities)
                                comparedObject.AddDifference("Description");
                            if (!propertyTypesContainers)
                                comparedObject.AddDifference("Property Types");
                            if (!appliesTo)
                                comparedObject.AddDifference("Applies To");
                            if (!autoApply)
                                comparedObject.AddDifference("Auto Apply");
                            if (!priority)
                                comparedObject.AddDifference("Priority");
                            if (!visible)
                                comparedObject.AddDifference("Visible");
                            if (!system)
                                comparedObject.AddDifference("System");
                            list.Add(comparedObject);
                            tList.Remove(t);
                        }
                        else
                        {
                            tList.Remove(t);
                        }
                    }
                }

                foreach (var t in tList)
                {
                    list.Add(new ComparedObject(null, t));
                }
            }
            else
            {
                if (sList?.Any() ?? false)
                {
                    foreach (var s in sList)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                } 
                else if (tList.Any())
                {
                    foreach (var t in tList)
                    {
                        list.Add(new ComparedObject(null, t));
                    }
                }
            }

            if (list.Any())
                result = list;

            return result;
        }

        private IEnumerable<ComparedObject> Compare(IEnumerable<IEntityTemplate> sources, IEnumerable<IEntityTemplate> targets)
        {
            IEnumerable<ComparedObject> result = null;

            List<ComparedObject> list = new List<ComparedObject>();

            var sList = sources?.ToArray();
            var tList = new List<IEntityTemplate>();
            if (targets?.Any() ?? false)
                tList.AddRange(targets);

            if ((sList?.Any() ?? false) && tList.Any())
            {
                foreach (var s in sList)
                {
                    var t = tList.FirstOrDefault(x => x.Id == s.Id);
                    if (t == null)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                    else
                    {
                        var identities = CompareIdentities(s, t);
                        var propertiesContainers = ComparePropertiesContainers(s, t);
                        var images = Compare(s.BigImage, t.BigImage) && Compare(s.Image, t.Image) &&
                                     Compare(s.SmallImage, t.SmallImage);
                        var entityType = s.EntityType == t.EntityType;
                        if (!identities || !propertiesContainers || !images || !entityType)
                        {
                            var comparedObject = new ComparedObject(s, t);
                            if (!identities)
                                comparedObject.AddDifference("Name or Description");
                            if (!propertiesContainers)
                                comparedObject.AddDifference("Properties");
                            if (!images)
                                comparedObject.AddDifference("Images");
                            if (!entityType)
                                comparedObject.AddDifference("Entity Type");
                            list.Add(comparedObject);
                            tList.Remove(t);
                        }
                        else
                        {
                            tList.Remove(t);
                        }
                    }
                }

                foreach (var t in tList)
                {
                    list.Add(new ComparedObject(null, t));
                }
            }
            else
            {
                if (sList?.Any() ?? false)
                {
                    foreach (var s in sList)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                } 
                else if (tList.Any())
                {
                    foreach (var t in tList)
                    {
                        list.Add(new ComparedObject(null, t));
                    }
                }
            }

            if (list.Any())
                result = list;

            return result;
        }

        private IEnumerable<ComparedObject> Compare(IEnumerable<IFlowTemplate> sources, IEnumerable<IFlowTemplate> targets)
        {
            IEnumerable<ComparedObject> result = null;

            List<ComparedObject> list = new List<ComparedObject>();

            var sList = sources?.ToArray();
            var tList = new List<IFlowTemplate>();
            if (targets?.Any() ?? false)
                tList.AddRange(targets);

            if ((sList?.Any() ?? false) && tList.Any())
            {
                foreach (var s in sList)
                {
                    var t = tList.FirstOrDefault(x => x.Id == s.Id);
                    if (t == null)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                    else
                    {
                        var identities = CompareIdentities(s, t);
                        var propertiesContainers = ComparePropertiesContainers(s, t);
                        if (!identities || !propertiesContainers)
                        {
                            var comparedObject = new ComparedObject(s, t);
                            if (!identities)
                                comparedObject.AddDifference("Name or Description");
                            if (!propertiesContainers)
                                comparedObject.AddDifference("Properties");
                            list.Add(comparedObject);
                            tList.Remove(t);
                        }
                        else
                        {
                            tList.Remove(t);
                        }
                    }
                }

                foreach (var t in tList)
                {
                    list.Add(new ComparedObject(null, t));
                }
            }
            else
            {
                if (sList?.Any() ?? false)
                {
                    foreach (var s in sList)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                }
                else if (tList.Any())
                {
                    foreach (var t in tList)
                    {
                        list.Add(new ComparedObject(null, t));
                    }
                }
            }

            if (list.Any())
                result = list;

            return result;
        }

        private IEnumerable<ComparedObject> Compare(IEnumerable<ITrustBoundaryTemplate> sources, IEnumerable<ITrustBoundaryTemplate> targets)
        {
            IEnumerable<ComparedObject> result = null;

            List<ComparedObject> list = new List<ComparedObject>();

            var sList = sources?.ToArray();
            var tList = new List<ITrustBoundaryTemplate>();
            if (targets?.Any() ?? false)
                tList.AddRange(targets);

            if ((sList?.Any() ?? false) && tList.Any())
            {
                foreach (var s in sList)
                {
                    var t = tList.FirstOrDefault(x => x.Id == s.Id);
                    if (t == null)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                    else
                    {
                        var identities = CompareIdentities(s, t);
                        var propertiesContainers = ComparePropertiesContainers(s, t);
                        if (!identities || !propertiesContainers)
                        {
                            var comparedObject = new ComparedObject(s, t);
                            if (!identities)
                                comparedObject.AddDifference("Name or Description");
                            if (!propertiesContainers)
                                comparedObject.AddDifference("Properties");
                            list.Add(comparedObject);
                            tList.Remove(t);
                        }
                        else
                        {
                            tList.Remove(t);
                        }
                    }
                }

                foreach (var t in tList)
                {
                    list.Add(new ComparedObject(null, t));
                }
            }
            else
            {
                if (sList?.Any() ?? false)
                {
                    foreach (var s in sList)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                }
                else if (tList.Any())
                {
                    foreach (var t in tList)
                    {
                        list.Add(new ComparedObject(null, t));
                    }
                }
            }

            if (list.Any())
                result = list;

            return result;
        }

        private IEnumerable<ComparedObject> Compare(IEnumerable<IMitigation> sources, IEnumerable<IMitigation> targets)
        {
            IEnumerable<ComparedObject> result = null;

            List<ComparedObject> list = new List<ComparedObject>();

            var sList = sources?.ToArray();
            var tList = new List<IMitigation>();
            if (targets?.Any() ?? false)
                tList.AddRange(targets);

            if ((sList?.Any() ?? false) && tList.Any())
            {
                foreach (var s in sList)
                {
                    var t = tList.FirstOrDefault(x => x.Id == s.Id);
                    if (t == null)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                    else
                    {
                        var identities = CompareIdentities(s, t);
                        var propertiesContainers = ComparePropertiesContainers(s, t);
                        var controlType = s.ControlType == t.ControlType;
                        if (!identities || !propertiesContainers || !controlType)
                        {
                            var comparedObject = new ComparedObject(s, t);
                            if (!identities)
                                comparedObject.AddDifference("Name or Description");
                            if (!propertiesContainers)
                                comparedObject.AddDifference("Properties");
                            if (!controlType)
                                comparedObject.AddDifference("Control Type");
                            list.Add(comparedObject);
                            tList.Remove(t);
                        }
                        else
                        {
                            tList.Remove(t);
                        }
                    }
                }

                foreach (var t in tList)
                {
                    list.Add(new ComparedObject(null, t));
                }
            }
            else
            {
                if (sList?.Any() ?? false)
                {
                    foreach (var s in sList)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                } 
                else if (tList.Any())
                {
                    foreach (var t in tList)
                    {
                        list.Add(new ComparedObject(null, t));
                    }
                }
            }

            if (list.Any())
                result = list;

            return result;
        }

        private IEnumerable<ComparedObject> Compare(IEnumerable<IThreatActor> sources, IEnumerable<IThreatActor> targets)
        {
            IEnumerable<ComparedObject> result = null;

            List<ComparedObject> list = new List<ComparedObject>();

            var sList = sources?.ToArray();
            var tList = new List<IThreatActor>();
            if (targets?.Any() ?? false)
                tList.AddRange(targets);

            if ((sList?.Any() ?? false) && tList.Any())
            {
                foreach (var s in sList)
                {
                    var t = tList.FirstOrDefault(x => string.CompareOrdinal(x.Name, s.Name) == 0);
                    if (t == null)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                    else if (s.ActorType == t.ActorType)
                    {
                        var identities = CompareIdentities(s, t);
                        var propertiesContainers = ComparePropertiesContainers(s, t);
                        var actorType = s.ActorType == t.ActorType;
                        if (!identities || !propertiesContainers || !actorType)
                        {
                            var comparedObject = new ComparedObject(s, t);
                            if (!identities)
                                comparedObject.AddDifference("Description");
                            if (!propertiesContainers)
                                comparedObject.AddDifference("Properties");
                            if (!actorType)
                                comparedObject.AddDifference("Actor Type");
                            list.Add(comparedObject);
                            tList.Remove(t);
                        }
                        else
                        {
                            tList.Remove(t);
                        }
                    }
                    else
                    {
                        // Note: if Actor Type do not match, then the two Actors are not the same even if the Id corresponds.
                        // TODO: Given that this is an anomalous situation, the flow will be ignored but some telemetry will be generated.
                    }
                }

                foreach (var t in tList)
                {
                    list.Add(new ComparedObject(null, t));
                }
            }
            else
            {
                if (sList?.Any() ?? false)
                {
                    foreach (var s in sList)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                } 
                else if (tList.Any())
                {
                    foreach (var t in tList)
                    {
                        list.Add(new ComparedObject(null, t));
                    }
                }
            }

            if (list.Any())
                result = list;

            return result;
        }

        private IEnumerable<ComparedObject> Compare(IEnumerable<ISeverity> sources, IEnumerable<ISeverity> targets)
        {
            IEnumerable<ComparedObject> result = null;

            List<ComparedObject> list = new List<ComparedObject>();

            var sList = sources?.ToArray();
            var tList = new List<ISeverity>();
            if (targets?.Any() ?? false)
                tList.AddRange(targets);

            if ((sList?.Any() ?? false) && tList.Any())
            {
                foreach (var s in sList)
                {
                    var t = tList.FirstOrDefault(x => x.Id == s.Id);
                    if (t == null)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                    else
                    {
                        var propertiesContainers = ComparePropertiesContainers(s, t);
                        var name = string.CompareOrdinal(s.Name, t.Name) == 0;
                        var description = string.CompareOrdinal(s.Description, t.Description) == 0;
                        var visible = s.Visible == t.Visible;
                        if (!propertiesContainers || !name || !description || !visible)
                        {
                            var comparedObject = new ComparedObject(s, t);
                            if (!propertiesContainers)
                                comparedObject.AddDifference("Properties");
                            if (!name || !description)
                                comparedObject.AddDifference("Name or Description");
                            if (!visible)
                                comparedObject.AddDifference("Visible");
                            list.Add(comparedObject);
                            tList.Remove(t);
                        }
                        else
                        {
                            tList.Remove(t);
                        }
                    }
                }

                foreach (var t in tList)
                {
                    list.Add(new ComparedObject(null, t));
                }
            }
            else
            {
                if (sList?.Any() ?? false)
                {
                    foreach (var s in sList)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                } 
                else if (tList.Any())
                {
                    foreach (var t in tList)
                    {
                        list.Add(new ComparedObject(null, t));
                    }
                }
            }

            if (list.Any())
                result = list;

            return result;
        }

        private IEnumerable<ComparedObject> Compare(IEnumerable<IStrength> sources, IEnumerable<IStrength> targets)
        {
            IEnumerable<ComparedObject> result = null;

            List<ComparedObject> list = new List<ComparedObject>();

            var sList = sources?.ToArray();
            var tList = new List<IStrength>();
            if (targets?.Any() ?? false)
                tList.AddRange(targets);

            if ((sList?.Any() ?? false) && tList.Any())
            {
                foreach (var s in sList)
                {
                    var t = tList.FirstOrDefault(x => x.Id == s.Id);
                    if (t == null)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                    else
                    {
                        var propertiesContainers = ComparePropertiesContainers(s, t);
                        var name = string.CompareOrdinal(s.Name, t.Name) == 0;
                        var description = string.CompareOrdinal(s.Description, t.Description) == 0;
                        var visible = s.Visible == t.Visible;
                        if (!propertiesContainers || !name || !description || !visible)
                        {
                            var comparedObject = new ComparedObject(s, t);
                            if (!propertiesContainers)
                                comparedObject.AddDifference("Properties");
                            if (!name || !description)
                                comparedObject.AddDifference("Name or Description");
                            if (!visible)
                                comparedObject.AddDifference("Visible");
                            list.Add(comparedObject);
                            tList.Remove(t);
                        }
                        else
                        {
                            tList.Remove(t);
                        }
                    }
                }

                foreach (var t in tList)
                {
                    list.Add(new ComparedObject(null, t));
                }
            }
            else
            {
                if (sList?.Any() ?? false)
                {
                    foreach (var s in sList)
                    {
                        list.Add(new ComparedObject(s, null));
                    }
                } 
                else if (tList.Any())
                {
                    foreach (var t in tList)
                    {
                        list.Add(new ComparedObject(null, t));
                    }
                }
            }

            if (list.Any())
                result = list;

            return result;
        }

        private bool CompareIdentities([NotNull] IIdentity source, [NotNull] IIdentity target)
        {
            return (string.CompareOrdinal(source.Name, target.Name) == 0) &&
                   (string.CompareOrdinal(source.Description, target.Description) == 0);
        }

        private bool ComparePropertiesContainers([NotNull] IPropertiesContainer source, [NotNull] IPropertiesContainer target)
        {
            bool result = false;

            if (!(source.Properties?.Any() ?? false) && !(target.Properties?.Any() ?? false))
                result = true;
            else if (source.Properties?.Any() ?? false)
            {
                if (source.Properties.Count() == (target.Properties?.Count() ?? 0))
                {
                    var properties = source.Properties.ToArray();
                    result = true;
                    foreach (var property in properties)
                    {
                        var targetProperty =
                            target.Properties?.FirstOrDefault(x => x.PropertyTypeId == property.PropertyTypeId);

                        if ((targetProperty == null) ||
                            string.CompareOrdinal(property.StringValue, targetProperty.StringValue) != 0)
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private bool CompareParentGroups([NotNull] IGroupElement source, [NotNull] IGroupElement target)
        {
            return source.ParentId == target.ParentId;
        }

        private bool CompareThreatEventsContainers([NotNull] IThreatEventsContainer source, [NotNull] IThreatEventsContainer target)
        {
            bool result = false;

            if (!(source.ThreatEvents?.Any() ?? false) && !(target.ThreatEvents?.Any() ?? false))
                result = true;
            else if (source.ThreatEvents?.Any() ?? false)
            {
                if (source.ThreatEvents.Count() == (target.ThreatEvents?.Count() ?? 0))
                {
                    var threatEvents = source.ThreatEvents.ToArray();
                    result = true;
                    foreach (var threatEvent in threatEvents)
                    {
                        var targetThreatEvent =
                            target.ThreatEvents?.FirstOrDefault(x => x.ThreatTypeId == threatEvent.ThreatTypeId);

                        if ((targetThreatEvent == null) || !CompareThreatEvents(threatEvent, targetThreatEvent))
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private bool CompareThreatEvents([NotNull] IThreatEvent source, [NotNull] IThreatEvent target)
        {
            return CompareIdentities(source, target) && ComparePropertiesContainers(source, target) &&
                   CompareThreatEventScenariosContainers(source, target) &&
                   CompareThreatEventMitigationsContainers(source, target) &&
                ((source.Severity?.Id ?? 0) == (target.Severity?.Id)) ;
        }

        private bool CompareThreatEventMitigationsContainers([NotNull] IThreatEventMitigationsContainer source, [NotNull] IThreatEventMitigationsContainer target)
        {
            bool result = false;

            if (!(source.Mitigations?.Any() ?? false) && !(target.Mitigations?.Any() ?? false))
                result = true;
            else if (source.Mitigations?.Any() ?? false)
            {
                if (source.Mitigations.Count() == (target.Mitigations?.Count() ?? 0))
                {
                    var mitigations = source.Mitigations.ToArray();
                    result = true;
                    foreach (var mitigation in mitigations)
                    {
                        var targetMitigation =
                            target.Mitigations?.FirstOrDefault(x => x.MitigationId == mitigation.MitigationId);

                        if ((targetMitigation == null) || !CompareMitigations(mitigation, targetMitigation))
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private bool CompareMitigations([NotNull] IThreatEventMitigation source, [NotNull] IThreatEventMitigation target)
        {
            return source.MitigationId == target.MitigationId &&
                   (string.CompareOrdinal(source.Directives, target.Directives) == 0) &&
                   source.StrengthId == target.StrengthId && source.Status == target.Status;
        }

        private bool CompareThreatEventScenariosContainers([NotNull] IThreatEventScenariosContainer source, [NotNull] IThreatEventScenariosContainer target)
        {
            bool result = false;

            if (!(source.Scenarios?.Any() ?? false) && !(target.Scenarios?.Any() ?? false))
                result = true;
            else if (source.Scenarios?.Any() ?? false)
            {
                if (source.Scenarios.Count() == (target.Scenarios?.Count() ?? 0))
                {
                    var scenarios = source.Scenarios.ToArray();
                    result = true;
                    foreach (var scenario in scenarios)
                    {
                        var targetScenario =
                            target.Scenarios?.FirstOrDefault(x => x.Id == scenario.Id);

                        if ((targetScenario == null) || !CompareIdentities(scenario, targetScenario) ||
                            !ComparePropertiesContainers(scenario, targetScenario) ||
                            (scenario.Severity?.Id ?? 0) != (targetScenario.Severity?.Id ?? 0) ||
                            (scenario.ActorId != targetScenario.ActorId) ||
                            string.CompareOrdinal(scenario.Motivation, targetScenario.Motivation) != 0)
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private bool CompareEntityShapesContainers([NotNull] IEntityShapesContainer source, [NotNull] IEntityShapesContainer target)
        {
            bool result = false;

            if (!(source.Entities?.Any() ?? false) && !(target.Entities?.Any() ?? false))
                result = true;
            else if (source.Entities?.Any() ?? false)
            {
                if (source.Entities.Count() == (target.Entities?.Count() ?? 0))
                {
                    var entities = source.Entities.ToArray();
                    result = true;
                    foreach (var entity in entities)
                    {
                        var targetEntity =
                            target.Entities?.FirstOrDefault(x => x.AssociatedId == entity.AssociatedId);

                        if ((targetEntity == null) || !CompareShapes(entity, targetEntity))
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private bool CompareShapes([NotNull] IShape source, [NotNull] IShape target)
        {
            return source.AssociatedId == target.AssociatedId && source.Position == target.Position &&
                ComparePropertiesContainers(source, target);
        }

        private bool CompareGroupShapesContainers([NotNull] IGroupShapesContainer source, [NotNull] IGroupShapesContainer target)
        {
            bool result = false;

            if (!(source.Groups?.Any() ?? false) && !(target.Groups?.Any() ?? false))
                result = true;
            else if (source.Groups?.Any() ?? false)
            {
                if (source.Groups.Count() == (target.Groups?.Count() ?? 0))
                {
                    var groups = source.Groups.ToArray();
                    result = true;
                    foreach (var group in groups)
                    {
                        var targetGroup =
                            target.Groups?.FirstOrDefault(x => x.AssociatedId == group.AssociatedId);

                        if ((targetGroup == null) || !CompareShapes(group, targetGroup))
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private bool CompareLinksContainer([NotNull] ILinksContainer source, [NotNull] ILinksContainer target)
        {
            bool result = false;

            if (!(source.Links?.Any() ?? false) && !(target.Links?.Any() ?? false))
                result = true;
            else if (source.Links?.Any() ?? false)
            {
                if (source.Links.Count() == (target.Links?.Count() ?? 0))
                {
                    var links = source.Links.ToArray();
                    result = true;
                    foreach (var link in links)
                    {
                        var targetLink =
                            target.Links?.FirstOrDefault(x => x.AssociatedId == link.AssociatedId);

                        if ((targetLink == null) || !CompareLinks(link, targetLink))
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private bool CompareLinks([NotNull] ILink source, [NotNull] ILink target)
        {
            return source.AssociatedId == target.AssociatedId && ComparePropertiesContainers(source, target);
        }

        private bool CompareThreatTypeMitigationsContainers([NotNull] IThreatTypeMitigationsContainer source, [NotNull] IThreatTypeMitigationsContainer target)
        {
            bool result = false;

            if (!(source.Mitigations?.Any() ?? false) && !(target.Mitigations?.Any() ?? false))
                result = true;
            else if (source.Mitigations?.Any() ?? false)
            {
                if (source.Mitigations.Count() == (target.Mitigations?.Count() ?? 0))
                {
                    var mitigations = source.Mitigations.ToArray();
                    result = true;
                    foreach (var mitigation in mitigations)
                    {
                        var targetMitigation =
                            target.Mitigations?.FirstOrDefault(x => x.MitigationId == mitigation.MitigationId);

                        if ((targetMitigation == null) || !CompareMitigations(mitigation, targetMitigation))
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private bool CompareMitigations([NotNull] IThreatTypeMitigation source, [NotNull] IThreatTypeMitigation target)
        {
            return source.ThreatTypeId == target.ThreatTypeId && source.MitigationId == target.MitigationId &&
                   source.StrengthId == target.StrengthId;
        }

        private bool ComparePropertyTypesContainers([NotNull] IPropertyTypesContainer source, [NotNull] IPropertyTypesContainer target)
        {
            bool result = false;

            if (!(source.PropertyTypes?.Any() ?? false) && !(target.PropertyTypes?.Any() ?? false))
                result = true;
            else if (source.PropertyTypes?.Any() ?? false)
            {
                if (source.PropertyTypes.Count() == (target.PropertyTypes?.Count() ?? 0))
                {
                    var propertyTypes = source.PropertyTypes.ToArray();
                    result = true;
                    foreach (var propertyType in propertyTypes)
                    {
                        var targetPropertyType =
                            target.PropertyTypes?.FirstOrDefault(x => string.CompareOrdinal(x.Name, propertyType.Name) == 0);

                        if ((targetPropertyType == null) || !ComparePropertyTypes(propertyType, targetPropertyType))
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private bool ComparePropertyTypes([NotNull] IPropertyType source, [NotNull] IPropertyType target)
        {
            return CompareIdentities(source, target) &&
                   source.Priority == target.Priority && source.Visible == target.Visible;
        }
        #endregion

        #region Form events management.
        private void MergeDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_comparison != null)
            {
                if (_fullComparison)
                    ThreatModelManager.Remove(_comparison.Id);
                else
                    TemplateManager.CloseTemplate(_comparison.Id);
            }
        }
        #endregion

        #region Button management.
        private void _showAdded_Click(object sender, EventArgs e)
        {
            _filterMode = FilterMode.Added;
            ReviseVisibility();
        }

        private void _showChanged_Click(object sender, EventArgs e)
        {
            _filterMode = FilterMode.Changed;
            ReviseVisibility();
        }

        private void _showRemoved_Click(object sender, EventArgs e)
        {
            _filterMode = FilterMode.Removed;
            ReviseVisibility();

        }

        private void _showAll_Click(object sender, EventArgs e)
        {
            _filterMode = FilterMode.All;
            ReviseVisibility();

        }

        private void _applyFilter_Click(object sender, EventArgs e)
        {
            ReviseVisibility();
        }

        private void ReviseVisibility()
        {
            if (_fullComparison)
            {
                ReviseVisibility(_gridExternalInteractors);
                ReviseVisibility(_gridProcesses);
                ReviseVisibility(_gridDataStores);
                ReviseVisibility(_gridFlows);
                ReviseVisibility(_gridTrustBoundaries);
                ReviseVisibility(_gridDiagrams);
            }

            ReviseVisibility(_gridThreatTypes);
            ReviseVisibility(_gridMitigations);
            ReviseVisibility(_gridItemTemplates);
            ReviseVisibility(_gridPropertySchemas);
            ReviseVisibility(_gridThreatActors);
            ReviseVisibility(_gridSeverities);
            ReviseVisibility(_gridStrengths);
        }

        private void ReviseVisibility([NotNull] SuperGridControl grid)
        {
            var rows = grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            if (rows.Any())
            {
                foreach (var row in rows)
                {
                    if (row.Tag is ComparedObject compared)
                    {
                        var visible = (string.IsNullOrWhiteSpace(_filter.Text) || compared.ToString().ToLowerInvariant().Contains(_filter.Text.ToLowerInvariant()));

                        if (visible)
                        {
                            switch (_filterMode)
                            {
                                case FilterMode.Added:
                                    visible = compared.Action == ActionType.Add;
                                    break;
                                case FilterMode.Changed:
                                    visible = compared.Action == ActionType.Change;
                                    break;
                                case FilterMode.Removed:
                                    visible = compared.Action == ActionType.Remove;
                                    break;
                            }
                        }

                        row.Visible = visible;
                    }
                }
            }
        }

        private void _checkAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this,
                "Checking All is not recommended, because it is possible to include undesired actions. The problem may be particularly severe for removals and undesired changes.\nAre you sure you want to proceed?",
                "Check All", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                CheckAll(_gridExternalInteractors);
                CheckAll(_gridProcesses);
                CheckAll(_gridDataStores);
                CheckAll(_gridFlows);
                CheckAll(_gridTrustBoundaries);
                CheckAll(_gridDiagrams);
                CheckAll(_gridThreatTypes);
                CheckAll(_gridMitigations);
                CheckAll(_gridItemTemplates);
                CheckAll(_gridPropertySchemas);
                CheckAll(_gridThreatActors);
                CheckAll(_gridSeverities);
                CheckAll(_gridStrengths);
            }
        }

        private void CheckAll([NotNull] SuperGridControl grid)
        {
            var rows = grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            if (rows.Any())
            {
                foreach (var row in rows)
                    SetToBeMerged(row, true);
            }
        }

        private void _checkAdded_Click(object sender, EventArgs e)
        {
            CheckAdded(_gridExternalInteractors);
            CheckAdded(_gridProcesses);
            CheckAdded(_gridDataStores);
            CheckAdded(_gridFlows);
            CheckAdded(_gridTrustBoundaries);
            CheckAdded(_gridDiagrams);
            CheckAdded(_gridThreatTypes);
            CheckAdded(_gridMitigations);
            CheckAdded(_gridItemTemplates);
            CheckAdded(_gridPropertySchemas);
            CheckAdded(_gridThreatActors);
            CheckAdded(_gridSeverities);
            CheckAdded(_gridStrengths);
        }

        private void CheckAdded([NotNull] SuperGridControl grid)
        {
            var rows = grid.PrimaryGrid.Rows.OfType<GridRow>()
                .Where(x => (x.Tag is ComparedObject comparedObject) && 
                            (comparedObject.Source != null && comparedObject.Target == null))
                .ToArray();
            if (rows.Any())
            {
                foreach (var row in rows)
                    SetToBeMerged(row, true);
            }
        }

        private void _checkChanged_Click(object sender, EventArgs e)
        {
            CheckChanged(_gridExternalInteractors);
            CheckChanged(_gridProcesses);
            CheckChanged(_gridDataStores);
            CheckChanged(_gridFlows);
            CheckChanged(_gridTrustBoundaries);
            CheckChanged(_gridDiagrams);
            CheckChanged(_gridThreatTypes);
            CheckChanged(_gridMitigations);
            CheckChanged(_gridItemTemplates);
            CheckChanged(_gridPropertySchemas);
            CheckChanged(_gridThreatActors);
            CheckChanged(_gridSeverities);
            CheckChanged(_gridStrengths);
        }

        private void CheckChanged([NotNull] SuperGridControl grid)
        {
            var rows = grid.PrimaryGrid.Rows.OfType<GridRow>()
                .Where(x => (x.Tag is ComparedObject comparedObject) && 
                            (comparedObject.Source != null && comparedObject.Target != null))
                .ToArray();
            if (rows.Any())
            {
                foreach (var row in rows)
                    SetToBeMerged(row, true);
            }
        }

        private void _checkRemoved_Click(object sender, EventArgs e)
        {
            CheckRemoved(_gridExternalInteractors);
            CheckRemoved(_gridProcesses);
            CheckRemoved(_gridDataStores);
            CheckRemoved(_gridFlows);
            CheckRemoved(_gridTrustBoundaries);
            CheckRemoved(_gridDiagrams);
            CheckRemoved(_gridThreatTypes);
            CheckRemoved(_gridMitigations);
            CheckRemoved(_gridItemTemplates);
            CheckRemoved(_gridPropertySchemas);
            CheckRemoved(_gridThreatActors);
            CheckRemoved(_gridSeverities);
            CheckRemoved(_gridStrengths);
        }

        private void CheckRemoved([NotNull] SuperGridControl grid)
        {
            var rows = grid.PrimaryGrid.Rows.OfType<GridRow>()
                .Where(x => (x.Tag is ComparedObject comparedObject) && 
                            (comparedObject.Source == null && comparedObject.Target != null))
                .ToArray();
            if (rows.Any())
            {
                foreach (var row in rows)
                    SetToBeMerged(row, true);
            }
        }

        private void _uncheckAll_Click(object sender, EventArgs e)
        {
            UncheckAll(_gridExternalInteractors);
            UncheckAll(_gridProcesses);
            UncheckAll(_gridDataStores);
            UncheckAll(_gridFlows);
            UncheckAll(_gridTrustBoundaries);
            UncheckAll(_gridDiagrams);
            UncheckAll(_gridThreatTypes);
            UncheckAll(_gridMitigations);
            UncheckAll(_gridItemTemplates);
            UncheckAll(_gridPropertySchemas);
            UncheckAll(_gridThreatActors);
            UncheckAll(_gridSeverities);
            UncheckAll(_gridStrengths);
        }

        private void UncheckAll([NotNull] SuperGridControl grid)
        {
            var rows = grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            if (rows.Any())
            {
                foreach (var row in rows)
                    SetToBeMerged(row, false);
            }
        }

        private void _uncheckAdded_Click(object sender, EventArgs e)
        {
            UncheckAdded(_gridExternalInteractors);
            UncheckAdded(_gridProcesses);
            UncheckAdded(_gridDataStores);
            UncheckAdded(_gridFlows);
            UncheckAdded(_gridTrustBoundaries);
            UncheckAdded(_gridDiagrams);
            UncheckAdded(_gridThreatTypes);
            UncheckAdded(_gridMitigations);
            UncheckAdded(_gridItemTemplates);
            UncheckAdded(_gridPropertySchemas);
            UncheckAdded(_gridThreatActors);
            UncheckAdded(_gridSeverities);
            UncheckAdded(_gridStrengths);
        }

        private void UncheckAdded([NotNull] SuperGridControl grid)
        {
            var rows = grid?.PrimaryGrid.Rows.OfType<GridRow>()
                .Where(x => (x.Tag is ComparedObject comparedObject) && 
                            (comparedObject.Source != null && comparedObject.Target == null))
                .ToArray();
            if (rows.Any())
            {
                foreach (var row in rows)
                    SetToBeMerged(row, false);
            }
        }

        private void _uncheckChanged_Click(object sender, EventArgs e)
        {
            UncheckChanged(_gridExternalInteractors);
            UncheckChanged(_gridProcesses);
            UncheckChanged(_gridDataStores);
            UncheckChanged(_gridFlows);
            UncheckChanged(_gridTrustBoundaries);
            UncheckChanged(_gridDiagrams);
            UncheckChanged(_gridThreatTypes);
            UncheckChanged(_gridMitigations);
            UncheckChanged(_gridItemTemplates);
            UncheckChanged(_gridPropertySchemas);
            UncheckChanged(_gridThreatActors);
            UncheckChanged(_gridSeverities);
            UncheckChanged(_gridStrengths);
        }

        private void UncheckChanged([NotNull] SuperGridControl grid)
        {
            var rows = grid.PrimaryGrid.Rows.OfType<GridRow>()
                .Where(x => (x.Tag is ComparedObject comparedObject) && 
                            (comparedObject.Source != null && comparedObject.Target != null))
                .ToArray();
            if (rows.Any())
            {
                foreach (var row in rows)
                    SetToBeMerged(row, false);
            }
        }

        private void _uncheckRemoved_Click(object sender, EventArgs e)
        {
            UncheckRemoved(_gridExternalInteractors);
            UncheckRemoved(_gridProcesses);
            UncheckRemoved(_gridDataStores);
            UncheckRemoved(_gridFlows);
            UncheckRemoved(_gridTrustBoundaries);
            UncheckRemoved(_gridDiagrams);
            UncheckRemoved(_gridThreatTypes);
            UncheckRemoved(_gridMitigations);
            UncheckRemoved(_gridItemTemplates);
            UncheckRemoved(_gridPropertySchemas);
            UncheckRemoved(_gridThreatActors);
            UncheckRemoved(_gridSeverities);
            UncheckRemoved(_gridStrengths);
        }

        private void UncheckRemoved([NotNull] SuperGridControl grid)
        {
            var rows = grid.PrimaryGrid.Rows.OfType<GridRow>()
                .Where(x => (x.Tag is ComparedObject comparedObject) && 
                            (comparedObject.Source == null && comparedObject.Target != null))
                .ToArray();
            if (rows.Any())
            {
                foreach (var row in rows)
                    SetToBeMerged(row, false);
            }
        }
        #endregion

        #region Do merge.
        private void _ok_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("The selected merge actions are about to be performed.\nPlease confirm.",
                "Merge", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                bool dirty = false;

                if (_tabPropertySchemas.Visible)
                    dirty |= MergeGrid(_gridPropertySchemas);
                if (_tabSeverities.Visible)
                    dirty |= MergeGrid(_gridSeverities);
                if (_tabStrengths.Visible)
                    dirty |= MergeGrid(_gridStrengths);
                if (_tabThreatActors.Visible)
                    dirty |= MergeGrid(_gridThreatActors);
                if (_tabMitigations.Visible)
                    dirty |= MergeGrid(_gridMitigations);
                if (_tabThreatTypes.Visible)
                    dirty |= MergeGrid(_gridThreatTypes);
                if (_tabItemTemplates.Visible)
                    dirty |= MergeGrid(_gridItemTemplates);
                if (_tabTrustBoundaries.Visible)
                    dirty |= MergeGrid(_gridTrustBoundaries);
                if (_tabExternalInteractors.Visible)
                    dirty |= MergeGrid(_gridExternalInteractors);
                if (_tabProcesses.Visible)
                    dirty |= MergeGrid(_gridProcesses);
                if (_tabDataStores.Visible)
                    dirty |= MergeGrid(_gridDataStores);
                if (_tabFlows.Visible)
                    dirty |= MergeGrid(_gridFlows);
                if (_tabDiagrams.Visible)
                    dirty |= MergeGrid(_gridDiagrams);

                if (dirty)
                    _model.SetDirty();
            }
            else
            {
                DialogResult = DialogResult.Abort;
            }
        }

        private void SetToBeMerged([NotNull] GridRow row, bool toBeMerged)
        {
            row.Cells[0].Value = toBeMerged;
        }

        private bool MergeGrid([NotNull] SuperGridControl grid)
        {
            bool result = false;

            var rows = grid.PrimaryGrid.Rows.OfType<GridRow>().Where(x => IsToBeMerged(x)).ToArray();
            if (rows.Any())
            {
                foreach (var row in rows)
                {
                    if (row.Tag is ComparedObject comparedObject)
                    {
                        result |= MergeComparedObject(comparedObject);
                    }
                }
            }

            return result;
        }

        private bool MergeComparedObject([NotNull] ComparedObject comparedObject)
        {
            bool result = false;

            if (comparedObject.Source == null)
            {
                if (comparedObject.Target == null)
                {
                    // Nothing to do
                }
                else
                {
                    Remove(comparedObject.Target);
                    result = true;
                }
            }
            else
            {
                if (comparedObject.Target == null)
                {
                    Add(comparedObject.Source);
                    result = true;
                }
                else
                {
                    Change(comparedObject.Source, comparedObject.Target);
                    result = true;
                }
            }

            return result;
        }

        private void Add([NotNull] object source)
        {
            if (source is IEntity entity)
            {
                entity.Clone(_model);
            }
            else if (source is IDataFlow dataFlow)
            {
                dataFlow.Clone(_model);
            }
            else if (source is IGroup group)
            {
                group.Clone(_model);
            }
            else if (source is IDiagram diagram)
            {
                diagram.Clone(_model);
            }
            else if (source is IThreatType threatType)
            {
                threatType.Clone(_model);
            }
            else if (source is IMitigation mitigation)
            {
                mitigation.Clone(_model);
            }
            else if (source is IEntityTemplate entityTemplate)
            {
                entityTemplate.Clone(_model);
            }
            else if (source is IPropertySchema propertySchema)
            {
                propertySchema.Clone(_model);
            }
            else if (source is IThreatActor threatActor)
            {
                threatActor.Clone(_model);
            }
            else if (source is ISeverity severity)
            {
                severity.Clone(_model);
            }
            else if (source is IStrength strength)
            {
                strength.Clone(_model);
            }
        }

        private void Remove([NotNull] object target)
        {
            if (target is IEntity entity)
            {
                _model.RemoveEntity(entity.Id);
            }
            else if (target is IDataFlow dataFlow)
            {
                _model.RemoveDataFlow(dataFlow.Id);
            }
            else if (target is IGroup group)
            {
                _model.RemoveGroup(group.Id);
            }
            else if (target is IDiagram diagram)
            {
                _model.RemoveDiagram(diagram.Id);
            }
            else if (target is IThreatType threatType)
            {
                _model.RemoveThreatType(threatType.Id, true);
            }
            else if (target is IMitigation mitigation)
            {
                _model.RemoveMitigation(mitigation.Id, true);
            }
            else if (target is IEntityTemplate entityTemplate)
            {
                _model.RemoveEntityTemplate(entityTemplate.Id);
            }
            else if (target is IPropertySchema propertySchema)
            {
                _model.RemoveSchema(propertySchema.Id, true);
            }
            else if (target is IThreatActor threatActor)
            {
                _model.RemoveThreatActor(threatActor.Id, true);
            }
            else if (target is ISeverity severity)
            {
                _model.RemoveSeverity(severity.Id);
            }
            else if (target is IStrength strength)
            {
                _model.RemoveStrength(strength.Id);
            }
        }

        private void Change([NotNull] object source, [NotNull] object target)
        {
            if (source is IExternalInteractor sourceEI && target is IExternalInteractor targetEI)
            {
                ApplyIdentity(sourceEI, targetEI);
                ApplyImagesContainer(sourceEI, targetEI);
                ApplyParents(sourceEI, targetEI);
                ApplyProperties(sourceEI, targetEI);
                ApplyThreatEventsContainer(sourceEI, targetEI);
            }
            else if (source is IProcess sourceP && target is IProcess targetP)
            {
                ApplyIdentity(sourceP, targetP);
                ApplyImagesContainer(sourceP, targetP);
                ApplyParents(sourceP, targetP);
                ApplyProperties(sourceP, targetP);
                ApplyThreatEventsContainer(sourceP, targetP);
            }
            else if (source is IDataStore sourceDS && target is IDataStore targetDS)
            {
                ApplyIdentity(sourceDS, targetDS);
                ApplyImagesContainer(sourceDS, targetDS);
                ApplyParents(sourceDS, targetDS);
                ApplyProperties(sourceDS, targetDS);
                ApplyThreatEventsContainer(sourceDS, targetDS);
            }
            else if (source is IDataFlow sourceF && target is IDataFlow targetF)
            {
                ApplyIdentity(sourceF, targetF);
                ApplyProperties(sourceF, targetF);
                ApplyThreatEventsContainer(sourceF, targetF);
                if (sourceF.FlowType != targetF.FlowType)
                    targetF.FlowType = sourceF.FlowType;
            }
            else if (source is IGroup sourceG && target is IGroup targetG)
            {
                ApplyIdentity(sourceG, targetG);
                if (sourceG is IGroupElement sourceGE && targetG is IGroupElement targetGE)
                    ApplyParents(sourceGE, targetGE);
                ApplyProperties(sourceG, targetG);
            }
            else if (source is IDiagram sourceD && target is IDiagram targetD)
            {
                ApplyIdentity(sourceD, targetD);
                ApplyProperties(sourceD, targetD);
                ApplyEntityShapesContainer(sourceD, targetD);
                ApplyGroupShapesContainer(sourceD, targetD);
                ApplyLinksContainer(sourceD, targetD);
                if (sourceD.Order != targetD.Order)
                    targetD.Order = sourceD.Order;
            }
            else if (source is IThreatType sourceTT && target is IThreatType targetTT)
            {
                ApplyIdentity(sourceTT, targetTT);
                ApplyProperties(sourceTT, targetTT);
                ApplyThreatTypeMitigationsContainer(sourceTT, targetTT);
                if (sourceTT.SeverityId != targetTT.SeverityId)
                {
                    var severity = _model.GetSeverity(sourceTT.SeverityId);
                    if (severity != null)
                        targetTT.Severity = severity;
                }
            }
            else if (source is IMitigation sourceM && target is IMitigation targetM)
            {
                ApplyIdentity(sourceM, targetM);
                ApplyProperties(sourceM, targetM);
                if (sourceM.ControlType != targetM.ControlType)
                    targetM.ControlType = sourceM.ControlType;
            }
            else if (source is IEntityTemplate sourceET && target is IEntityTemplate targetET)
            {
                ApplyIdentity(sourceET, targetET);
                ApplyImagesContainer(sourceET, targetET);
                ApplyProperties(sourceET, targetET);
                if (sourceET.EntityType != targetET.EntityType)
                    targetET.EntityType = sourceET.EntityType;
            }
            else if (source is IPropertySchema sourcePS && target is IPropertySchema targetPS)
            {
                ApplyPropertySchemas(sourcePS, targetPS);
            }
            else if (source is IThreatActor sourceA && target is IThreatActor targetA)
            {
                ApplyIdentity(sourceA, targetA);
                ApplyProperties(sourceA, targetA);
            }
            else if (source is ISeverity sourceSe && target is ISeverity targetSe)
            {
                ApplyProperties(sourceSe, targetSe);
                if (string.CompareOrdinal(sourceSe.Name, targetSe.Name) != 0)
                    targetSe.Name = sourceSe.Name;
                if (string.CompareOrdinal(sourceSe.Description, targetSe.Description) != 0)
                    targetSe.Description = sourceSe.Description;
                if (sourceSe.Visible != targetSe.Visible)
                    targetSe.Visible = sourceSe.Visible;
                if (sourceSe.TextColor != targetSe.TextColor)
                    targetSe.TextColor = sourceSe.TextColor;
                if (sourceSe.BackColor != targetSe.BackColor)
                    targetSe.BackColor = sourceSe.BackColor;
            }
            else if (source is IStrength sourceS && target is IStrength targetS)
            {
                ApplyProperties(sourceS, targetS);
                if (string.CompareOrdinal(sourceS.Name, targetS.Name) != 0)
                    targetS.Name = sourceS.Name;
                if (string.CompareOrdinal(sourceS.Description, targetS.Description) != 0)
                    targetS.Description = sourceS.Description;
                if (sourceS.Visible != targetS.Visible)
                    targetS.Visible = sourceS.Visible;
            }
        }

        private void ApplyIdentity([NotNull] IIdentity source, [NotNull] IIdentity target)
        {
            if (string.CompareOrdinal(source.Name, target.Name) != 0)
                target.Name = source.Name;
            if (string.CompareOrdinal(source.Description, target.Description) != 0)
                target.Description = source.Description;
        }

        private void ApplyImagesContainer([NotNull] IImagesContainer source,
            [NotNull] IImagesContainer target)
        {
            if (!Compare(source.BigImage, target.BigImage))
                target.BigImage = source.BigImage;
            if (!Compare(source.Image, target.Image))
                target.Image = source.Image;
            if (!Compare(source.SmallImage, target.SmallImage))
                target.SmallImage = source.SmallImage;
        }

        private void ApplyParents([NotNull] IGroupElement source, [NotNull] IGroupElement target)
        {
            if (CompareParentGroups(source, target))
            {
                if (source.Parent != null)
                {
                    var parent = _model.GetGroup(source.ParentId);
                    if (parent == null)
                    {
                        parent = source.Parent?.Clone(_model);
                    }

                    target.SetParent(parent);
                }
                else
                {
                    target.SetParent(null);
                }
            }
        }

        private void ApplyProperties([NotNull] IPropertiesContainer source, [NotNull] IPropertiesContainer target)
        {
            var sourceList = source.Properties?.ToArray();
            var targetList = target.Properties?.ToArray();
            if (sourceList?.Any() ?? false)
            {
                if (targetList?.Any() ?? false)
                {
                    // Merge of the two lists.
                    var toBeRemoved = new List<IProperty>(targetList);
                    foreach (var prop in sourceList)
                    {
                        var t = targetList.FirstOrDefault(x => x.PropertyTypeId == prop.PropertyTypeId);
                        if (t != null)
                        {
                            ApplyProperty(prop, t);
                            toBeRemoved.Remove(t);
                        }
                        else
                        {
                            AddProperty(target, prop);
                        }
                    }

                    if (toBeRemoved.Any())
                    {
                        foreach (var item in toBeRemoved)
                        {
                            target.RemoveProperty(item.PropertyTypeId);
                        }
                    }
                }
                else
                {
                    // All properties in sourceList must be added.
                    foreach (var prop in sourceList)
                    {
                        AddProperty(target, prop);
                    }
                }
            }
            else if (targetList?.Any() ?? false)
            {
                // All properties in targetList must be removed.
                foreach (var prop in targetList)
                {
                    target.RemoveProperty(prop.PropertyTypeId);
                }
            }
        }

        private void AddProperty(IPropertiesContainer target, IProperty prop)
        {
            var propertyType = _model.GetPropertyType(prop.PropertyTypeId);
            if (propertyType == null)
            {
                var schemaId = prop.PropertyType?.SchemaId;
                IPropertySchema targetSchema = null;
                if (schemaId.HasValue)
                {
                    var sourceSchema = _comparison.GetSchema(schemaId.Value);
                    targetSchema = _model.GetSchema(schemaId.Value);
                    if (sourceSchema != null)
                    {
                        if (targetSchema != null)
                        {
                            ApplyPropertyTypesContainer(sourceSchema, targetSchema);
                        }
                        else
                        {
                            targetSchema = sourceSchema.Clone(_model);
                        }
                    }
                }

                if (targetSchema != null)
                {
                    propertyType = prop.PropertyType?.Clone(targetSchema);
                }
            }

            if (propertyType != null)
                target.AddProperty(propertyType, prop.StringValue);
        }

        private void ApplyProperty([NotNull] IProperty source, [NotNull] IProperty target)
        {
            if (string.CompareOrdinal(source.StringValue, target.StringValue) != 0)
                target.StringValue = source.StringValue;
            if (source.ReadOnly != target.ReadOnly)
                target.ReadOnly = source.ReadOnly;
        }

        private void ApplyThreatEventsContainer([NotNull] IThreatEventsContainer source,
            [NotNull] IThreatEventsContainer target)
        {
            var sourceList = source.ThreatEvents?.ToArray();
            var targetList = target.ThreatEvents?.ToArray();
            if (sourceList?.Any() ?? false)
            {
                if (targetList?.Any() ?? false)
                {
                    // Merge of the two lists.
                    var toBeRemoved = new List<IThreatEvent>(targetList);
                    foreach (var threatEvent in sourceList)
                    {
                        var t = targetList.FirstOrDefault(x => x.ThreatTypeId == threatEvent.ThreatTypeId);
                        if (t != null)
                        {
                            ApplyThreatEvent(threatEvent, t);
                            toBeRemoved.Remove(t);
                        }
                        else
                        {
                            threatEvent.Clone(target);
                        }
                    }

                    if (toBeRemoved.Any())
                    {
                        foreach (var item in toBeRemoved)
                        {
                            target.RemoveThreatEvent(item.Id);
                        }
                    }
                }
                else
                {
                    // All Threat Events in sourceList must be added.
                    foreach (var threatEvent in sourceList)
                    {
                        threatEvent.Clone(target);
                    }
                }
            }
            else if (targetList?.Any() ?? false)
            {
                // All Threat Events in targetList must be removed.
                foreach (var threatEvent in targetList)
                {
                    target.RemoveThreatEvent(threatEvent.Id);
                }
            }
        }

        private void ApplyThreatEvent([NotNull] IThreatEvent source, [NotNull] IThreatEvent target)
        {
            ApplyIdentity(source, target);
            ApplyProperties(source, target);
            ApplyThreatEventMitigationsContainer(source, target);
            ApplyThreatEventScenariosContainer(source, target);
            if (source.SeverityId != target.SeverityId)
            {
                var severity = _model.GetSeverity(source.SeverityId) ?? source.Severity?.Clone(_model);
                if (severity != null)
                    target.Severity = severity;
            }
        }
        
        private void ApplyThreatEventMitigationsContainer([NotNull] IThreatEventMitigationsContainer source,
            [NotNull] IThreatEventMitigationsContainer target)
        {
            var sourceList = source.Mitigations?.ToArray();
            var targetList = target.Mitigations?.ToArray();
            if (sourceList?.Any() ?? false)
            {
                if (targetList?.Any() ?? false)
                {
                    // Merge of the two lists.
                    var toBeRemoved = new List<IThreatEventMitigation>(targetList);
                    foreach (var mitigation in sourceList)
                    {
                        var t = targetList.FirstOrDefault(x => x.MitigationId == mitigation.MitigationId);
                        if (t != null)
                        {
                            ApplyThreatEventMitigation(mitigation, t);
                            toBeRemoved.Remove(t);
                        }
                        else
                        {
                            mitigation.Clone(target);
                        }
                    }

                    if (toBeRemoved.Any())
                    {
                        foreach (var item in toBeRemoved)
                        {
                            target.RemoveMitigation(item.MitigationId);
                        }
                    }
                }
                else
                {
                    // All mitigations in sourceList must be added.
                    foreach (var mitigation in sourceList)
                    {
                        mitigation.Clone(target);
                    }
                }
            }
            else if (targetList?.Any() ?? false)
            {
                // All mitigations in targetList must be removed.
                foreach (var mitigation in targetList)
                {
                    target.RemoveMitigation(mitigation.MitigationId);
                }
            }
        }

        private void ApplyThreatEventMitigation([NotNull] IThreatEventMitigation source,
            [NotNull] IThreatEventMitigation target)
        {
            ApplyProperties(source, target);
            if (string.CompareOrdinal(source.Directives, target.Directives) != 0)
                target.Directives = source.Directives;
            if (source.Status != target.Status)
                target.Status = source.Status;
            if (source.StrengthId != target.StrengthId)
            {
                var strength = _model.GetStrength(source.StrengthId) ?? source.Strength?.Clone(_model);
                if (strength != null)
                    target.Strength = strength;
            }
        }

        private void ApplyThreatEventScenariosContainer([NotNull] IThreatEventScenariosContainer source,
            [NotNull] IThreatEventScenariosContainer target)
        {
            var sourceList = source.Scenarios?.ToArray();
            var targetList = target.Scenarios?.ToArray();
            if (sourceList?.Any() ?? false)
            {
                if (targetList?.Any() ?? false)
                {
                    // Merge of the two lists.
                    var toBeRemoved = new List<IThreatEventScenario>(targetList);
                    foreach (var scenario in sourceList)
                    {
                        var t = targetList.FirstOrDefault(x => x.Id == scenario.Id);
                        if (t != null)
                        {
                            ApplyThreatEventScenario(scenario, t);
                            toBeRemoved.Remove(t);
                        }
                        else
                        {
                            scenario.Clone(target);
                        }
                    }

                    if (toBeRemoved.Any())
                    {
                        foreach (var item in toBeRemoved)
                        {
                            target.RemoveScenario(item.Id);
                        }
                    }
                }
                else
                {
                    // All scenarios in sourceList must be added.
                    foreach (var scenario in sourceList)
                    {
                        scenario.Clone(target);
                    }
                }
            }
            else if (targetList?.Any() ?? false)
            {
                // All scenarios in targetList must be removed.
                foreach (var scenario in targetList)
                {
                    target.RemoveScenario(scenario.Id);
                }
            }
        }

        private void ApplyThreatEventScenario([NotNull] IThreatEventScenario source, [NotNull] IThreatEventScenario target)
        {
            ApplyIdentity(source, target);
            ApplyProperties(source, target);
            if (source.SeverityId != target.SeverityId)
            {
                var severity = _model.GetSeverity(source.SeverityId) ?? source.Severity?.Clone(_model);
                if (severity != null)
                    target.Severity = severity;
            }
            if (source.ActorId != target.ActorId)
            {
                var actor = _model.GetThreatActor(source.ActorId) ?? source.Actor?.Clone(_model);
                if (actor != null)
                    target.Actor = actor;
            }
            if (string.CompareOrdinal(source.Motivation, target.Motivation) != 0)
                target.Motivation = source.Motivation;
        }

        private void ApplyThreatTypeMitigationsContainer([NotNull] IThreatTypeMitigationsContainer source,
            [NotNull] IThreatTypeMitigationsContainer target)
        {
            var sourceList = source.Mitigations?.ToArray();
            var targetList = target.Mitigations?.ToArray();
            if (sourceList?.Any() ?? false)
            {
                if (targetList?.Any() ?? false)
                {
                    // Merge of the two lists.
                    var toBeRemoved = new List<IThreatTypeMitigation>(targetList);
                    foreach (var mitigation in sourceList)
                    {
                        var t = targetList.FirstOrDefault(x => x.MitigationId == mitigation.MitigationId);
                        if (t != null)
                        {
                            ApplyThreatTypeMitigation(mitigation, t);
                            toBeRemoved.Remove(t);
                        }
                        else
                        {
                            mitigation.Clone(target);
                        }
                    }

                    if (toBeRemoved.Any())
                    {
                        foreach (var item in toBeRemoved)
                        {
                            target.RemoveMitigation(item.MitigationId);
                        }
                    }
                }
                else
                {
                    // All mitigations in sourceList must be added.
                    foreach (var mitigation in sourceList)
                    {
                        mitigation.Clone(target);
                    }
                }
            }
            else if (targetList?.Any() ?? false)
            {
                // All mitigations in targetList must be removed.
                foreach (var mitigation in targetList)
                {
                    target.RemoveMitigation(mitigation.MitigationId);
                }
            }
        }

        private void ApplyThreatTypeMitigation([NotNull] IThreatTypeMitigation source,
            [NotNull] IThreatTypeMitigation target)
        {
            ApplyProperties(source, target);
            if (source.StrengthId != target.StrengthId)
            {
                var strength = _model.GetStrength(source.StrengthId) ?? source.Strength?.Clone(_model);
                if (strength != null)
                    target.Strength = strength;
            }
        }

        private void ApplyEntityShapesContainer([NotNull] IEntityShapesContainer source,
            [NotNull] IEntityShapesContainer target)
        {
            var sourceList = source.Entities?.ToArray();
            var targetList = target.Entities?.ToArray();
            if (sourceList?.Any() ?? false)
            {
                if (targetList?.Any() ?? false)
                {
                    // Merge of the two lists.
                    var toBeRemoved = new List<IEntityShape>(targetList);
                    foreach (var shape in sourceList)
                    {
                        var t = targetList.FirstOrDefault(x => x.AssociatedId == shape.AssociatedId);
                        if (t != null)
                        {
                            ApplyShape(shape, t);
                            toBeRemoved.Remove(t);
                        }
                        else
                        {
                            AddEntity(shape);
                            shape.Clone(target);
                        }
                    }

                    if (toBeRemoved.Any())
                    {
                        foreach (var item in toBeRemoved)
                        {
                            target.RemoveEntityShape(item.AssociatedId);
                        }
                    }
                }
                else
                {
                    // All shapes in sourceList must be added.
                    foreach (var shape in sourceList)
                    {
                        AddEntity(shape);
                        shape.Clone(target);
                    }
                }
            }
            else if (targetList?.Any() ?? false)
            {
                // All shapes in targetList must be removed.
                foreach (var shape in targetList)
                {
                    target.RemoveEntityShape(shape.AssociatedId);
                }
            }
        }

        private void AddEntity([NotNull] IEntityShape shape)
        {
            if (_model.GetEntity(shape.AssociatedId) == null &&
                shape.Identity is IEntity entity)
            {
                entity.Clone(_model);
                var flows = _comparison.DataFlows?
                    .Where(x =>
                        (x.SourceId == shape.AssociatedId && (_model.Entities?.Any(y => y.Id == x.TargetId) ?? false)) ||
                        (x.TargetId == shape.AssociatedId && (_model.Entities?.Any(y => y.Id == x.SourceId) ?? false)))
                    .ToArray();
                if (flows?.Any() ?? false)
                {
                    var targetFlows = _model.DataFlows?.ToArray();
                    foreach (var flow in flows)
                    {
                        if (!(targetFlows?.Any(x => x.Id == flow.Id) ?? false))
                        {
                            flow.Clone(_model);
                        }
                    }
                }
            }
        }

        private void ApplyShape([NotNull] IShape source, [NotNull] IShape target)
        {
            ApplyProperties(source, target);
            if (source.Position != target.Position)
                target.Position = source.Position;
        }

        private void ApplyGroupShapesContainer([NotNull] IGroupShapesContainer source,
            [NotNull] IGroupShapesContainer target)
        {
            var sourceList = source.Groups?.ToArray();
            var targetList = target.Groups?.ToArray();
            if (sourceList?.Any() ?? false)
            {
                if (targetList?.Any() ?? false)
                {
                    // Merge of the two lists.
                    var toBeRemoved = new List<IGroupShape>(targetList);
                    foreach (var shape in sourceList)
                    {
                        var t = targetList.FirstOrDefault(x => x.AssociatedId == shape.AssociatedId);
                        if (t != null)
                        {
                            ApplyGroupShape(shape, t);
                            toBeRemoved.Remove(t);
                        }
                        else
                        {
                            AddGroup(shape);
                            shape.Clone(target);
                        }
                    }

                    if (toBeRemoved.Any())
                    {
                        foreach (var item in toBeRemoved)
                        {
                            target.RemoveGroupShape(item.AssociatedId);
                        }
                    }
                }
                else
                {
                    // All shapes in sourceList must be added.
                    foreach (var shape in sourceList)
                    {
                        AddGroup(shape);
                        shape.Clone(target);
                    }
                }
            }
            else if (targetList?.Any() ?? false)
            {
                // All shapes in targetList must be removed.
                foreach (var shape in targetList)
                {
                    target.RemoveGroupShape(shape.AssociatedId);
                }
            }
        }
        
        private void AddGroup([NotNull] IGroupShape shape)
        {
            if (_model.GetGroup(shape.AssociatedId) == null &&
                shape.Identity is IGroup group)
            {
                group.Clone(_model);
            }
        }

        private void ApplyGroupShape([NotNull] IGroupShape source, [NotNull] IGroupShape target)
        {
            ApplyShape(source, target);
            if (source.Size != target.Size)
                target.Size = source.Size;
        }

        private void ApplyLinksContainer([NotNull] ILinksContainer source,
            [NotNull] ILinksContainer target)
        {
            var sourceList = source.Links?.ToArray();
            var targetList = target.Links?.ToArray();
            if (sourceList?.Any() ?? false)
            {
                if (targetList?.Any() ?? false)
                {
                    // Merge of the two lists.
                    var toBeRemoved = new List<ILink>(targetList);
                    foreach (var shape in sourceList)
                    {
                        var t = targetList.FirstOrDefault(x => x.AssociatedId == shape.AssociatedId);
                        if (t != null)
                        {
                            ApplyLink(shape, t);
                            toBeRemoved.Remove(t);
                        }
                        else
                        {
                            AddLink(shape);
                            shape.Clone(target);
                        }
                    }

                    if (toBeRemoved.Any())
                    {
                        foreach (var item in toBeRemoved)
                        {
                            target.RemoveLink(item.AssociatedId);
                        }
                    }
                }
                else
                {
                    // All links in sourceList must be added.
                    foreach (var link in sourceList)
                    {
                        AddLink(link);
                        link.Clone(target);
                    }
                }
            }
            else if (targetList?.Any() ?? false)
            {
                // All links in targetList must be removed.
                foreach (var link in targetList)
                {
                    target.RemoveLink(link.AssociatedId);
                }
            }
        }
        
        private void AddLink([NotNull] ILink link)
        {
            if (_model.GetDataFlow(link.AssociatedId) == null &&
                link.DataFlow is IDataFlow flow &&
                _model.GetEntity(flow.SourceId) is IEntity source &&
                _model.GetEntity(flow.TargetId) is IEntity target)
            {
                flow.Clone(_model);
            }
        }

        private void ApplyLink([NotNull] ILink source, [NotNull] ILink target)
        {
            ApplyProperties(source, target);
        }

        private void ApplyPropertySchemas([NotNull] IPropertySchema source, [NotNull] IPropertySchema target)
        {
            ApplyIdentity(source, target);
            ApplyPropertyTypesContainer(source, target);
            if (string.CompareOrdinal(source.Namespace, target.Namespace) != 0)
                target.Namespace = source.Namespace;
            if (source.AppliesTo != target.AppliesTo)
                target.AppliesTo = source.AppliesTo;
            if (source.AutoApply != target.AutoApply)
                target.AutoApply = source.AutoApply;
            if (source.Priority != target.Priority)
                target.Priority = source.Priority;
            if (source.Visible != target.Visible)
                target.Visible = source.Visible;
            if (source.System != target.System)
                target.System = source.System;

        }

        private void ApplyPropertyTypesContainer([NotNull] IPropertyTypesContainer source, [NotNull] IPropertyTypesContainer target)
        {
            var sourceList = source.PropertyTypes?.ToArray();
            var targetList = target.PropertyTypes?.ToArray();
            if (sourceList?.Any() ?? false)
            {
                if (targetList?.Any() ?? false)
                {
                    // Merge of the two lists.
                    var toBeRemoved = new List<IPropertyType>(targetList);
                    foreach (var propertyType in sourceList)
                    {
                        var t = targetList.FirstOrDefault(x => x.Id == propertyType.Id);
                        if (t != null)
                        {
                            ApplyPropertyType(propertyType, t);
                            toBeRemoved.Remove(t);
                        }
                        else
                        {
                            propertyType.Clone(target);
                        }
                    }

                    if (toBeRemoved.Any())
                    {
                        foreach (var item in toBeRemoved)
                        {
                            target.RemovePropertyType(item.Id);
                        }
                    }
                }
                else
                {
                    // All property types in sourceList must be added.
                    foreach (var propertyType in sourceList)
                    {
                        propertyType.Clone(target);
                    }
                }
            }
            else if (targetList?.Any() ?? false)
            {
                // All property types in targetList must be removed.
                foreach (var propertyType in targetList)
                {
                    target.RemovePropertyType(propertyType.Id);
                }
            }
        }

        private void ApplyPropertyType([NotNull] IPropertyType source, [NotNull] IPropertyType target)
        {
            ApplyIdentity(source, target);
            if (source.Priority != target.Priority)
                target.Priority = source.Priority;
            if (source.Visible != target.Visible)
                target.Visible = source.Visible;
        }
        #endregion

        #region Selection validation.
        private bool IsToBeMerged([NotNull] GridRow row)
        {
            bool result = (bool) row.Cells[0].Value;

            if (!result && row.Tag is ComparedObject comparedObject && comparedObject.Target == null)
            {
                if (comparedObject.Source is IEntity entity)
                {
                    result = SearchDiagrams(entity);
                }
                else if (comparedObject.Source is IDataFlow dataFlow)
                {
                    result = SearchDiagrams(dataFlow);
                }
                else if (comparedObject.Source is IGroup group)
                {
                    result = SearchDiagrams(group);
                }
                else if (comparedObject.Source is IThreatType threatType)
                {
                    result = SearchExternalInteractors(threatType) || SearchProcesses(threatType) ||
                             SearchDataStores(threatType) || SearchDataFlows(threatType);
                }
                else if (comparedObject.Source is IMitigation mitigation)
                {
                    result = SearchExternalInteractors(mitigation) || SearchProcesses(mitigation) ||
                             SearchDataStores(mitigation) || SearchDataFlows(mitigation) ||
                             SearchThreatTypes(mitigation);
                }
                else if (comparedObject.Source is IPropertySchema schema)
                {
                    result = SearchExternalInteractors(schema) || SearchProcesses(schema) ||
                             SearchDataStores(schema) || SearchDataFlows(schema) ||
                             SearchGroups(schema) || SearchDiagrams(schema) ||
                             SearchThreatTypes(schema) || SearchMitigations(schema) ||
                             SearchEntityTemplates(schema) || SearchThreatActors(schema) ||
                             SearchSeverities(schema) || SearchStrengths(schema) ;
                }
                else if (comparedObject.Source is IThreatActor threatActor)
                {
                    result = SearchExternalInteractors(threatActor) || SearchProcesses(threatActor) ||
                             SearchDataStores(threatActor) || SearchDataFlows(threatActor);
                }
                else if (comparedObject.Source is ISeverity severity)
                {
                    result = SearchExternalInteractors(severity) || SearchProcesses(severity) ||
                             SearchDataStores(severity) || SearchDataFlows(severity) ||
                             SearchThreatTypes(severity);
                }
                else if (comparedObject.Source is IStrength strength)
                {
                    result = SearchExternalInteractors(strength) || SearchProcesses(strength) ||
                             SearchDataStores(strength) || SearchDataFlows(strength) ||
                             SearchThreatTypes(strength);
                }
            }

            return result;
        }

        private bool SearchDiagrams([NotNull] IEntity entity)
        {
            return _gridDiagrams.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IDiagram diagram && 
                          diagram.GetEntityShape(entity.Id) != null);
        }

        private bool SearchDiagrams([NotNull] IGroup group)
        {
            return _gridDiagrams.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IDiagram diagram &&
                          diagram.GetGroupShape(group.Id) != null);
        }

        private bool SearchDiagrams([NotNull] IDataFlow flow)
        {
            return _gridDiagrams.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IDiagram diagram && 
                          diagram.GetLink(flow.Id) != null);
        }

        private bool SearchExternalInteractors([NotNull] IThreatType threatType)
        {
            return _gridExternalInteractors.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IThreatEventsContainer container &&
                          (container.ThreatEvents?.Any(y => y.ThreatTypeId == threatType.Id) ?? false));
        }

        private bool SearchProcesses([NotNull] IThreatType threatType)
        {
            return _gridProcesses.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IThreatEventsContainer container &&
                          (container.ThreatEvents?.Any(y => y.ThreatTypeId == threatType.Id) ?? false));
        }

        private bool SearchDataStores([NotNull] IThreatType threatType)
        {
            return _gridDataStores.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IThreatEventsContainer container &&
                          (container.ThreatEvents?.Any(y => y.ThreatTypeId == threatType.Id) ?? false));
        }

        private bool SearchDataFlows([NotNull] IThreatType threatType)
        {
            return _gridFlows.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IThreatEventsContainer container &&
                          (container.ThreatEvents?.Any(y => y.ThreatTypeId == threatType.Id) ?? false));
        }

        private bool SearchExternalInteractors([NotNull] IMitigation mitigation)
        {
            return _gridExternalInteractors.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IThreatEventsContainer container &&
                          (container.ThreatEvents?.Any(y => 
                              y.Mitigations?.Any(z => z.MitigationId == mitigation.Id) ?? false) ?? false));
        }

        private bool SearchProcesses([NotNull] IMitigation mitigation)
        {
            return _gridProcesses.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IThreatEventsContainer container &&
                          (container.ThreatEvents?.Any(y => 
                              y.Mitigations?.Any(z => z.MitigationId == mitigation.Id) ?? false) ?? false));
        }

        private bool SearchDataStores([NotNull] IMitigation mitigation)
        {
            return _gridDataStores.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IThreatEventsContainer container &&
                          (container.ThreatEvents?.Any(y => 
                              y.Mitigations?.Any(z => z.MitigationId == mitigation.Id) ?? false) ?? false));
        }

        private bool SearchDataFlows([NotNull] IMitigation mitigation)
        {
            return _gridFlows.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IThreatEventsContainer container &&
                          (container.ThreatEvents?.Any(y => 
                              y.Mitigations?.Any(z => z.MitigationId == mitigation.Id) ?? false) ?? false));
        }

        private bool SearchThreatTypes([NotNull] IMitigation mitigation)
        {
            return _gridThreatTypes.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IThreatType threatType &&
                          (threatType.Mitigations?.Any(y => y.MitigationId == mitigation.Id) ?? false));
        }

        private bool SearchExternalInteractors([NotNull] IPropertySchema schema)
        {
            return _gridExternalInteractors.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IPropertiesContainer container && 
                          SearchPropertySchema(schema, container));
        }

        private bool SearchProcesses([NotNull] IPropertySchema schema)
        {
            return _gridProcesses.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IPropertiesContainer container && 
                          SearchPropertySchema(schema, container));
        }

        private bool SearchDataStores([NotNull] IPropertySchema schema)
        {
            return _gridDataStores.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IPropertiesContainer container && 
                          SearchPropertySchema(schema, container));
        }

        private bool SearchDataFlows([NotNull] IPropertySchema schema)
        {
            return _gridFlows.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IPropertiesContainer container && 
                          SearchPropertySchema(schema, container));
        }

        private bool SearchGroups([NotNull] IPropertySchema schema)
        {
            return _gridTrustBoundaries.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IPropertiesContainer container && 
                          SearchPropertySchema(schema, container));
        }

        private bool SearchDiagrams([NotNull] IPropertySchema schema)
        {
            return _gridDiagrams.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IPropertiesContainer container && 
                          SearchPropertySchema(schema, container));
        }

        private bool SearchThreatTypes([NotNull] IPropertySchema schema)
        {
            return _gridThreatTypes.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IPropertiesContainer container && 
                          SearchPropertySchema(schema, container));
        }

        private bool SearchMitigations([NotNull] IPropertySchema schema)
        {
            return _gridMitigations.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IPropertiesContainer container && 
                          SearchPropertySchema(schema, container));
        }

        private bool SearchEntityTemplates([NotNull] IPropertySchema schema)
        {
            return _gridItemTemplates.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IPropertiesContainer container && 
                          SearchPropertySchema(schema, container));
        }

        private bool SearchThreatActors([NotNull] IPropertySchema schema)
        {
            return _gridThreatActors.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IPropertiesContainer container && 
                          SearchPropertySchema(schema, container));
        }

        private bool SearchSeverities([NotNull] IPropertySchema schema)
        {
            return _gridSeverities.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IPropertiesContainer container && 
                          SearchPropertySchema(schema, container));
        }

        private bool SearchStrengths([NotNull] IPropertySchema schema)
        {
            return _gridStrengths.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IPropertiesContainer container && 
                          SearchPropertySchema(schema, container));
        }

        private bool SearchPropertySchema([NotNull] IPropertySchema schema, [NotNull] IPropertiesContainer container)
        {
           return container.Properties?.Any(x => 
                (x.PropertyType is IPropertyType propertyType) && 
                (propertyType.SchemaId == schema.Id || 
                 (x is IPropertyJsonSerializableObject jsonObject && 
                  (_extractors?.Any(y => y.GetPropertySchemas(jsonObject)?.Any(z => z.Id == schema.Id) ?? false) ?? false)))) ?? false;
        }

        private bool SearchExternalInteractors([NotNull] IThreatActor threatActor)
        {
            return _gridExternalInteractors.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IThreatEventsContainer container &&
                          SearchThreatActor(threatActor, container));
        }

        private bool SearchProcesses([NotNull] IThreatActor threatActor)
        {
            return _gridProcesses.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IThreatEventsContainer container &&
                          SearchThreatActor(threatActor, container));
        }

        private bool SearchDataStores([NotNull] IThreatActor threatActor)
        {
            return _gridDataStores.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IThreatEventsContainer container &&
                          SearchThreatActor(threatActor, container));
        }

        private bool SearchDataFlows([NotNull] IThreatActor threatActor)
        {
            return _gridFlows.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IThreatEventsContainer container &&
                          SearchThreatActor(threatActor, container));
        }

        private bool SearchThreatActor([NotNull] IThreatActor threatActor, [NotNull] IThreatEventsContainer container)
        {
            bool result = false;

            var threatEvents = container.ThreatEvents?.ToArray();
            if (threatEvents?.Any() ?? false)
            {
                foreach (var te in threatEvents)
                {
                    if (te.Scenarios?.Any(x => x.ActorId == threatActor.Id) ?? false)
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        private bool SearchExternalInteractors([NotNull] ISeverity severity)
        {
            return _gridExternalInteractors.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IThreatEventsContainer container &&
                          (container.ThreatEvents?.Any(y => y.SeverityId == severity.Id) ?? false));
        }

        private bool SearchProcesses([NotNull] ISeverity severity)
        {
            return _gridProcesses.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IThreatEventsContainer container &&
                          (container.ThreatEvents?.Any(y => y.SeverityId == severity.Id) ?? false));
        }

        private bool SearchDataStores([NotNull] ISeverity severity)
        {
            return _gridDataStores.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IThreatEventsContainer container &&
                          (container.ThreatEvents?.Any(y => y.SeverityId == severity.Id) ?? false));
        }

        private bool SearchDataFlows([NotNull] ISeverity severity)
        {
            return _gridFlows.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IThreatEventsContainer container &&
                          (container.ThreatEvents?.Any(y => y.SeverityId == severity.Id) ?? false));
        }

        private bool SearchThreatTypes([NotNull] ISeverity severity)
        {
            return _gridThreatTypes.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject &&
                          comparedObject.Source is IThreatType threatType && threatType.SeverityId == severity.Id);
        }

        private bool SearchExternalInteractors([NotNull] IStrength strength)
        {
            return _gridExternalInteractors.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IThreatEventsContainer container &&
                          (container.ThreatEvents?.Any(y => y.Mitigations?.Any(z => z.StrengthId == strength.Id) ?? false) ?? false));
        }

        private bool SearchProcesses([NotNull] IStrength strength)
        {
            return _gridProcesses.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IThreatEventsContainer container &&
                          (container.ThreatEvents?.Any(y => y.Mitigations?.Any(z => z.StrengthId == strength.Id) ?? false) ?? false));
        }

        private bool SearchDataStores([NotNull] IStrength strength)
        {
            return _gridDataStores.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IThreatEventsContainer container &&
                          (container.ThreatEvents?.Any(y => y.Mitigations?.Any(z => z.StrengthId == strength.Id) ?? false) ?? false));
        }

        private bool SearchDataFlows([NotNull] IStrength strength)
        {
            return _gridFlows.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject && 
                          comparedObject.Source is IThreatEventsContainer container &&
                          (container.ThreatEvents?.Any(y => y.Mitigations?.Any(z => z.StrengthId == strength.Id) ?? false) ?? false));
        }

        private bool SearchThreatTypes([NotNull] IStrength strength)
        {
            return _gridThreatTypes.PrimaryGrid.Rows.OfType<GridRow>()
                .Any(x => IsToBeMerged(x) && x.Tag is ComparedObject comparedObject &&
                          comparedObject.Source is IThreatType threatType && 
                          (threatType.Mitigations?.Any(y => y.StrengthId == strength.Id) ?? false));
        }
        #endregion

        public void SetExecutionMode(ExecutionMode mode)
        {
            _executionMode = mode;
        }

        private void _filter_ButtonCustomClick(object sender, EventArgs e)
        {
            _filter.Text = null;
        }
    }
}