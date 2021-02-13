using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Threading;
using ThreatsManager.Extensions.Properties;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.ThreatSources
{
    public partial class MitreCatalogControl : UserControl
    {
        private ThreatSource _threatSource;
        private IntPtr _toast;
        private readonly Dictionary<string, IEnumerable<EditToken>> _associatedKeywords = new Dictionary<string, IEnumerable<EditToken>>();

        public MitreCatalogControl()
        {
            InitializeComponent();
            _properties.SelectedKeyword += CreateKeyword;
            _keywords.RemovingToken += KeywordRemoved;
            _keywords.ValidateToken += KeywordAdded;
            _keywords.EditTextBox.KeyPress += OnKeywordsKeyPress;
        }

        public IThreatModel Model { get; set; }
        public string SourceUrl { get; set; }
        public string FileName { get; set; }

        private string Source =>
            $"{_threatSource.CatalogName} v{_threatSource.CatalogVersion} ({_threatSource.CatalogDate.ToShortDateString()})";

        #region Public members to retrieve the status of the control.
        public IEnumerable<ThreatSourceNode> CheckedNodes => _nodes.CheckedNodes.Select(x => x.Tag)
            .OfType<ThreatSourceNodeParentChild>().Select(x => x.Node);

        public IEnumerable<ThreatSourceNode> SelectedNodes => _nodes.SelectedNodes.OfType<Node>()
            .Select(x => x.Tag).OfType<ThreatSourceNodeParentChild>().Select(x => x.Node);

        public IEnumerable<string> HiddenProperties => _properties.HiddenProperties;

        public IEnumerable<string> GetKeywords([Required] string nodeId)
        {
            return _associatedKeywords.FirstOrDefault(x => string.CompareOrdinal(nodeId, x.Key) == 0)
                .Value?.Select(x => x.Value);
        }
        #endregion

        #region Public members to [re]set the status of the control.
        public void ResetSelections()
        {
            _associatedKeywords.Clear();
            _keywords.SelectedTokens.Clear();
            var checkedNodes = _nodes.CheckedNodes.ToArray();
            foreach (var node in checkedNodes)
            {
                node.Checked = false;
            }
            _properties.ClearHiddenProperties();
        }

        public void CheckAll()
        {
            var nodes = _nodes.Nodes.OfType<Node>().ToArray();
            foreach (var node in nodes)
            {
                CheckBranch(node);
            }
        }

        public void CheckBranch()
        {
            var nodes = _nodes.SelectedNodes.OfType<Node>().ToArray();
            foreach (var node in nodes)
            {
                CheckBranch(node);
            }
        }

        private void CheckBranch([NotNull] Node node)
        {
            if (!node.Checked)
                node.Checked = true;
            var children = node.Nodes?.OfType<Node>().ToArray();
            if (children?.Any() ?? false)
            {
                foreach (var child in children)
                    CheckBranch(child);
            }
        }

        public void SetHiddenProperties([NotNull] IEnumerable<string> hiddenProperties)
        {
            _properties.SetHiddenProperties(hiddenProperties);
        }
        #endregion

        #region Autoloading.
        public void AutoLoad()
        {
            AutoLoadAsync();
        }

        [Background]
        private void AutoLoadAsync()
        {
#pragma warning disable SecurityIntelliSenseCS // MS Security rules violation
            string destFile = Path.Combine(Application.UserAppDataPath, FileName);
#pragma warning restore SecurityIntelliSenseCS // MS Security rules violation
            if (File.Exists(destFile))
            {
                try
                {
                    _threatSource = new ThreatSource(ThreatSourceManager.GetCapecCatalog(destFile));
                    SuccessAutoLoad();
                }
                catch
                {
#pragma warning disable SCS0018 // Path traversal: injection possible in {1} argument passed to '{0}'
                    File.Delete(destFile);
#pragma warning restore SCS0018 // Path traversal: injection possible in {1} argument passed to '{0}'
                }
                FinalizeAutoLoad();
            }
            else
                NoAutoload();
        }

        [Dispatched]
        private void SuccessAutoLoad()
        {
            _source.Text = Source;
        }

        [Dispatched]
        private void NoAutoload()
        {
            _download.Enabled = true;
        }

        [Dispatched]
        private void FinalizeAutoLoad()
        {
            _views.Items.Clear();
            if (_threatSource?.Views != null)
            {
                // ReSharper disable once CoVariantArrayConversion
                _views.Items.AddRange(_threatSource.Views.ToArray());
                _views.SelectedIndex = 0;
            }
        }
        #endregion

        #region File download.
        private void _download_Click(object sender, EventArgs e)
        {
            _download.Enabled = false;

            ToastNotification.ToastBackColor = Color.White;
            ToastNotification.ToastForeColor = ThreatModelManager.StandardColor;
            _toast = ToastNotification.Show(this, "Download in progress...", 
                Resources.cloud_computing_download, 60000, eToastGlowColor.Blue,
                eToastPosition.MiddleCenter);

            Download();
        }

        [Background]
        private void Download()
        {
            if (string.IsNullOrWhiteSpace(SourceUrl))
                throw new InvalidOperationException(string.Format(Resources.MissingParameterError, nameof(SourceUrl)));

            string fileName = Path.GetFileName(SourceUrl);
#pragma warning disable SecurityIntelliSenseCS // MS Security rules violation
            string fileWithPath = Path.Combine(Application.UserAppDataPath, fileName);
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(SourceUrl);
            string destFile = Path.Combine(Application.UserAppDataPath, FileName);
#pragma warning restore SecurityIntelliSenseCS // MS Security rules violation

#if CWE
            bool cwe = fileNameWithoutExt.StartsWith("cwec_");
#endif
            bool capec = fileNameWithoutExt.StartsWith("capec_");

#if CWE
            if (!cwe && !capec)
#else
            if (!capec)
#endif
                throw new InvalidOperationException(string.Format(Resources.UnsupportedFileTypeError, fileName));

            if (!File.Exists(fileWithPath))
            {
                using (var client = new WebClient())
                {
#pragma warning disable SecurityIntelliSenseCS // MS Security rules violation
                    client.DownloadFile(new Uri(SourceUrl), fileWithPath);
#pragma warning restore SecurityIntelliSenseCS // MS Security rules violation
                }
            }

            if (SourceUrl.EndsWith(".zip"))
            {
                ZipFile.ExtractToDirectory(fileWithPath, Application.UserAppDataPath);
#pragma warning disable SCS0018 // Path traversal: injection possible in {1} argument passed to '{0}'
#pragma warning disable SecurityIntelliSenseCS // MS Security rules violation
                File.Move(Path.Combine(Application.UserAppDataPath, fileNameWithoutExt), destFile);
#pragma warning restore SecurityIntelliSenseCS // MS Security rules violation
#pragma warning restore SCS0018 // Path traversal: injection possible in {1} argument passed to '{0}'
            }
            else
            {
                if (string.CompareOrdinal(fileName, FileName) != 0)
                {
#pragma warning disable SCS0018 // Path traversal: injection possible in {1} argument passed to '{0}'
#pragma warning disable SecurityIntelliSenseCS // MS Security rules violation
                    File.Move(Path.Combine(Application.UserAppDataPath, fileName), destFile);
#pragma warning restore SecurityIntelliSenseCS // MS Security rules violation
#pragma warning restore SCS0018 // Path traversal: injection possible in {1} argument passed to '{0}'
                }
            }

            try
            {
                _threatSource = capec
                    ? new ThreatSource(ThreatSourceManager.GetCapecCatalog(destFile))
                    :
#if CWE
                    new ThreatSource(ThreatSourceManager.GetCweCatalog(destFile));
#else
                    null;
#endif
                FinalizeDownload(true);
            }
            catch (Exception exc)
            {
#if DEBUG
                Debug.WriteLine(exc.ToString());
#endif

                if (File.Exists(destFile))
#pragma warning disable SCS0018 // Path traversal: injection possible in {1} argument passed to '{0}'
                    File.Delete(destFile);
#pragma warning restore SCS0018 // Path traversal: injection possible in {1} argument passed to '{0}'
                FinalizeDownload(false);
            }

        }

        [Dispatched]
        private void FinalizeDownload(bool success)
        {
            ToastNotification.Close(this, _toast);
            _views.Items.Clear();

            if (success)
            {
                _source.Text = Source;
                // ReSharper disable once CoVariantArrayConversion
                _views.Items.AddRange(_threatSource.Views.ToArray());
                _views.SelectedIndex = 0;
            }
            _download.Enabled = !success;
        }
        #endregion

        #region Tree management.
        private void _views_SelectedIndexChanged(object sender, EventArgs e)
        {
            _nodes.ClearAndDisposeAllNodes();

            var nodes = _threatSource.GetNodes((string)_views.SelectedItem);

            foreach (var node in nodes)
            {
                AddTree(node);
            }
        }

        private void AddTree([NotNull] ThreatSourceNodeParentChild node, Node parent = null)
        {
            var treeNode = new Node(node.ToString()) {Tag = node, CheckBoxVisible = true, CheckBoxStyle = eCheckBoxStyle.CheckBox, Checked = false };
            if (parent == null)
                _nodes.Nodes.Add(treeNode);
            else
                parent.Nodes.Add(treeNode);

            var children = node.Children;
            if (children != null)
            {
                foreach (var child in children)
                    AddTree(child, treeNode);
            }
        }

        private void _nodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_nodes.SelectedNode?.Tag is ThreatSourceNodeParentChild node)
            {
                _properties.Initialize(node.Node);
                _keywords.SelectedTokens.Clear();
                _keywords.Enabled = true;
                if (_associatedKeywords.TryGetValue(node.Node.Id, out var keywords))
                {
                    foreach (var k in keywords)
                    {
                        _keywords.SelectedTokens.Add(k);
                    }
                }
            }
            else
            {
                _properties.Clear();
            }
        }
        #endregion

        #region Tree filtering management.
        private void _applyFilter_Click(object sender, EventArgs e)
        {
            if (_views.SelectedItem != null)
            {
                _nodes.ClearAndDisposeAllNodes();

                var nodes = _threatSource.GetNodes((string) _views.SelectedItem);

                List<string> include;

                foreach (var node in nodes)
                {
                    include = new List<string>();
                    if (Check(node, _filter.Text, include))
                        AddFilteredTree(node, include);
                }
            }
        }

        private void AddFilteredTree([NotNull] ThreatSourceNodeParentChild node, 
            [NotNull] List<string> include, Node parent = null)
        {
            if (include.Contains(node.Node.Id))
            {
                var treeNode = new Node(node.ToString())
                {
                    Tag = node, CheckBoxVisible = true, CheckBoxStyle = eCheckBoxStyle.CheckBox, Checked = false
                };
                if (parent == null)
                    _nodes.Nodes.Add(treeNode);
                else
                    parent.Nodes.Add(treeNode);

                var children = node.Children;
                if (children != null)
                {
                    foreach (var child in children)
                        AddFilteredTree(child, include, treeNode);
                }
            }
        }

        private bool Check([NotNull] ThreatSourceNodeParentChild node, 
            string filter, List<string> include)
        {
            bool result = false;

            var children = node.Children;
            if (children != null)
            {
                foreach (var child in children)
                {
                    result |= Check(child, filter, include);
                }
            }

            result = result | node.Node.Properties.Any(x => x.Value?.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0);

            if (result)
                include.Add(node.Node.Id);

            return result;
        }

        private void _filter_TextChanged(object sender, EventArgs e)
        {
            _applyFilter.Enabled = _filter.Text.Length > 2;
        }

        private void _filter_KeyDown(object sender, KeyEventArgs e)
        {
            if (_applyFilter.Enabled && (e.KeyCode == Keys.Enter))
            {
                _applyFilter_Click(sender, e);
            }
        }

        private void _filter_ButtonCustomClick(object sender, EventArgs e)
        {
            _filter.Text = string.Empty;
            _applyFilter_Click(sender, e);
        }
        #endregion

        #region Keyword management.
        private void CreateKeyword(string text)
        {
            if (_nodes.SelectedNode?.Tag is ThreatSourceNodeParentChild node)
            {
                var token = new EditToken(text);
                if (_keywords.Tokens.All(x => string.Compare(text, x.Value, StringComparison.InvariantCultureIgnoreCase) != 0))
                    _keywords.Tokens.Add(token);
                if (_keywords.SelectedTokens.All(x => string.Compare(text, x.Value, StringComparison.InvariantCultureIgnoreCase) != 0))
                {
                    _keywords.SelectedTokens.Add(token);
                    _associatedKeywords[node.Node.Id] = _keywords.SelectedTokens.ToArray();
                }
            }
        }
        
        private void KeywordAdded(object sender, ValidateTokenEventArgs ea)
        {
            if (_nodes.SelectedNode?.Tag is ThreatSourceNodeParentChild node)
            {
                var list = new List<EditToken>(_keywords.SelectedTokens);
                list.Add(ea.Token);
                _associatedKeywords[node.Node.Id] = list.ToArray();
            }
        }

        private void KeywordRemoved(object sender, RemovingTokenEventArgs ea)
        {
            if (_nodes.SelectedNode?.Tag is ThreatSourceNodeParentChild node)
            {
                var list = new List<EditToken>(_keywords.SelectedTokens);
                list.Remove(ea.Token);
                _associatedKeywords[node.Node.Id] = list.ToArray();
            }
        }
        
        private void OnKeywordsKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                e.Handled = true;
                e.KeyChar = ';';
            }
        }
        #endregion
    }
}

