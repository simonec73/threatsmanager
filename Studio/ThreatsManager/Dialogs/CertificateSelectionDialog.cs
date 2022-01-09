using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using ThreatsManager.Engine.Config;

namespace ThreatsManager.Dialogs
{
    // ReSharper disable CoVariantArrayConversion
    public partial class CertificateSelectionDialog : Form
    {
        private CertificateConfig _certificate;

        public CertificateSelectionDialog()
        {
            InitializeComponent();
            _storeName.Items.AddRange(Enum.GetNames(typeof(StoreName)));
            _storeLocation.Items.AddRange(Enum.GetNames(typeof(StoreLocation)));
        }

        public CertificateConfig Certificate
        {
            get => _certificate;

            set
            {
                _certificate = value;

                _subject.Text = _certificate?.Subject;
                _issuer.Text = _certificate?.Issuer;
                _expiration.Text = _certificate?.ExpirationDate;

                _ok.Enabled = _certificate != null;
            }
        }

        private void _selectFromStore_Click(object sender, EventArgs e)
        {
            if (Enum.TryParse<StoreName>((string) _storeName.SelectedItem, out var storeName) &&
                Enum.TryParse<StoreLocation>((string) _storeLocation.SelectedItem, out var storeLocation))
            {
                X509Store store = new X509Store(storeName, storeLocation);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

                X509Certificate2Collection found = store.Certificates
                        .Find(X509FindType.FindByApplicationPolicy, "1.3.6.1.5.5.7.3.3", false)
                        .Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, false)
                        .Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                X509Certificate2Collection selected = X509Certificate2UI.SelectFromCollection(found,
                    "Certificate to be trusted",
                    "Select a certificate from the following list, to trust the Extensions signed with that certificate.",
                    X509SelectionFlag.SingleSelection);

                var certificate = selected.OfType<X509Certificate2>().FirstOrDefault();
                if (certificate != null)
                    Certificate = new CertificateConfig(certificate);
            }
        }

        private void _selectFromFile_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "dll",
                Filter = "Assemblies (*.dll)|*.dll|Executables (*.exe)|*.exe|All files (*.*)|*.*",
                Multiselect = false,
                Title = "Select the signed assembly signed with the trusted certificate",
                RestoreDirectory = true
            };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    var assembly = Assembly.ReflectionOnlyLoadFrom(dialog.FileName);
#pragma warning disable SecurityIntelliSenseCS // MS Security rules violation
                    var certificate = assembly.GetModules().First().GetSignerCertificate();
#pragma warning restore SecurityIntelliSenseCS // MS Security rules violation
                    if (certificate != null)
                    {
                        _fileName.Text = dialog.FileName;
                        Certificate = new CertificateConfig(certificate);
                    }
                }
                catch (BadImageFormatException)
                {
                    MessageBox.Show(
                        "The file is not a valid assembly containing a certificate. Please select a different file. You most typically want to select an Extension library.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void EnableControls()
        {
            var store = _store.Checked;
            var file = _file.Checked;

            _storeName.Enabled = store;
            _storeLocation.Enabled = store;
            _selectFromStore.Enabled = store && _storeName.SelectedItem != null && _storeLocation.SelectedItem != null;
            _selectFromFile.Enabled = file;
        }

        private void _store_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void _file_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void _storeName_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void _storeLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableControls();
        }
    }
}
