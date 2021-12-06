using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.DevOps
{
    class AccessTokenManager
    {
        public IEnumerable<string> Urls => Application.UserAppDataRegistry?.GetValueNames();

        public string GetToken([Required] string url)
        {
            string result = null;

            url = url.Trim('/');
            var encryptedText = Application.UserAppDataRegistry?.GetValue(url) as string;
            if (!string.IsNullOrEmpty(encryptedText))
            {
                var encryptedArray = Convert.FromBase64String(encryptedText);
                var decryptedArray = ProtectedData.Unprotect(encryptedArray, null, DataProtectionScope.CurrentUser);
                result = Encoding.UTF8.GetString(decryptedArray);
            }

            return result;
        }

        public void SetToken([Required] string url, string token)
        {
            url = url.Trim('/');
            if (!string.IsNullOrWhiteSpace(token))
            {
                var encryptedArray =
                    ProtectedData.Protect(Encoding.UTF8.GetBytes(token), null, DataProtectionScope.CurrentUser);
                Application.UserAppDataRegistry?.SetValue(url, Convert.ToBase64String(encryptedArray));
            }
            else
            {
                Application.UserAppDataRegistry?.DeleteValue(url, false);   
            }
        }
    }
}
