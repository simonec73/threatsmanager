using System;
using System.Runtime.InteropServices;
using System.Security;

namespace ThreatsManager.PackageManagers.Packaging
{
    public static class SecureStringUtils
    {
        // Credits: Eric Lloyd (https://stackoverflow.com/questions/18392538/securestring-to-byte-c-sharp).
        public static T Process<T>(this SecureString src, Func<byte[], T> func)
        {
            IntPtr bstr = IntPtr.Zero;
            byte[] workArray = null;
            GCHandle? handle = null; // Hats off to Tobias Bauer
            try
            {
                /*** PLAINTEXT EXPOSURE BEGINS HERE ***/
                bstr = Marshal.SecureStringToBSTR(src);
                unsafe
                {
                    byte* bstrBytes = (byte*)bstr;
                    workArray = new byte[src.Length * 2];
                    handle = GCHandle.Alloc(workArray, GCHandleType.Pinned); // Hats off to Tobias Bauer
                    for (int i = 0; i < workArray.Length; i++)
                        workArray[i] = *bstrBytes++;
                }

                return func(workArray);
            }
            finally
            {
                if (workArray != null)
                    for (int i = 0; i < workArray.Length; i++)
                        workArray[i] = 0;
                handle?.Free();
                if (bstr != IntPtr.Zero)
                    Marshal.ZeroFreeBSTR(bstr);
                /*** PLAINTEXT EXPOSURE ENDS HERE ***/
            }
        }
    }
}
