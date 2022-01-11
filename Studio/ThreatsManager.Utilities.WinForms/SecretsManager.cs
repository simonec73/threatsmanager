using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Utilities.WinForms
{
    /// <summary>
    /// Class to protect secrets locally in the Registry.
    /// </summary>
    /// <remarks>Secrets are encrypted using DPAPI for the user, and then stored in a location which is only accessible by the user itself.</remarks>
    public class SecretsManager
    {
        public IEnumerable<string> Existing => Application.UserAppDataRegistry?.GetValueNames();

        /// <summary>
        /// Get a secret.
        /// </summary>
        /// <param name="key">Key representing the secret.</param>
        /// <returns>Secret, unencrypted.</returns>
        public string GetSecret([Required] string key)
        {
            string result = null;

            key = key.Trim('/', ' ');
            var encryptedText = Application.UserAppDataRegistry?.GetValue(key) as string;
            if (!string.IsNullOrEmpty(encryptedText))
            {
                var encryptedArray = Convert.FromBase64String(encryptedText);
                var decryptedArray = ProtectedData.Unprotect(encryptedArray, null, DataProtectionScope.CurrentUser);
                result = Encoding.UTF8.GetString(decryptedArray);
            }

            return result;
        }

        /// <summary>
        /// Set a secret.
        /// </summary>
        /// <param name="key">Key representing the secret.</param>
        /// <param name="secret">Secret, unencrypted.</param>
        public void SetSecret([Required] string key, string secret)
        {
            key = key.Trim('/', ' ');
            if (!string.IsNullOrWhiteSpace(key))
            {
                var encryptedArray =
                    ProtectedData.Protect(Encoding.UTF8.GetBytes(secret), null, DataProtectionScope.CurrentUser);
                Application.UserAppDataRegistry?.SetValue(key, Convert.ToBase64String(encryptedArray));
            }
            else
            {
                Application.UserAppDataRegistry?.DeleteValue(key, false);
            }
        }
    }
}
