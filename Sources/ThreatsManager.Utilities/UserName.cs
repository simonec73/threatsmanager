using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Auxiliary class to provide the name of the current user.
    /// </summary>
    public class UserName
    {
        /// <summary>
        /// Available formats for the user name.
        /// </summary>
        public enum ExtendedNameFormat
        {
             /// <summary>
             /// An unknown name type.
             /// </summary>
             NameUnknown = 0,

             /// <summary>
             /// The fully qualified distinguished name
             /// (for example, CN=Jeff Smith,OU=Users,DC=Engineering,DC=Microsoft,DC=Com).
             /// </summary>
             NameFullyQualifiedDN = 1,

             /// <summary>
             /// A legacy account name (for example, Engineering\JSmith).
             /// The domain-only version includes trailing backslashes (\\).
             /// </summary>
             NameSamCompatible = 2,

             /// <summary>
             /// A "friendly" display name (for example, Jeff Smith).
             /// The display name is not necessarily the defining relative distinguished name (RDN).
             /// </summary>
             NameDisplay = 3,

             /// <summary>
             /// A GUID string that the IIDFromString function returns
             /// (for example, {4fa050f0-f561-11cf-bdd9-00aa003a77b6}).
             /// </summary>
             NameUniqueId = 6,

             /// <summary>
             /// The complete canonical name (for example, engineering.microsoft.com/software/someone).
             /// The domain-only version includes a trailing forward slash (/).
             /// </summary>
             NameCanonical = 7,

             /// <summary>
             /// The user principal name (for example, someone@example.com).
             /// </summary>
             NameUserPrincipal = 8,

             /// <summary>
             /// The same as NameCanonical except that the rightmost forward slash (/)
             /// is replaced with a new line character (\n), even in a domain-only case
             /// (for example, engineering.microsoft.com/software\nJSmith).
             /// </summary>
             NameCanonicalEx = 9,

             /// <summary>
             /// The generalized service principal name
             /// (for example, www/www.microsoft.com@microsoft.com).
             /// </summary>
             NameServicePrincipal = 10,

             /// <summary>
             /// The DNS domain name followed by a backward-slash and the SAM user name.
             /// </summary>
             NameDnsDomain = 12
        }

        [DllImport("secur32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        private static extern int GetUserNameEx (ExtendedNameFormat nameFormat,
            StringBuilder userName, ref int userNameSize);

        /// <summary>
        /// Get the name of the current user.
        /// </summary>
        /// <returns>Display Name of the current user.</returns>
        public static string GetDisplayName()
        {
            string result = null;

            StringBuilder userName = new StringBuilder(1024);
            int userNameSize = userName.Capacity;

            try
            {
                if(0 != GetUserNameEx(ExtendedNameFormat.NameDisplay, userName, ref userNameSize))
                {
                    result = userName.ToString();
                }
                else
                {
                    result = Environment.UserName;
                }
            }
            catch
            {
                result = Environment.UserName;
            }

            return result;
        }
    }
}
