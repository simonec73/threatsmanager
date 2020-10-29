using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace ThreatsManager.Utilities.Help
{
    /// <summary>
    /// Source: https://blog.sverrirs.com/2016/04/webrequest-caching-in-csharp.html
    /// </summary>
    public abstract class Manager
    {
        protected string GetDataUsingSocket(string url, CookieContainer cookieContainer = null )
        {
            // Optionally add a random element to the url if the correct macro is present 
            url = url.Replace("%RAND%", DateTime.Now.Ticks.ToString());

            // Structured uri to be able to extract components 
            var uri = new Uri(url);

            // Create the socket and connect it 
            var clientSocket = ConnectSocket(uri);
            if (clientSocket == null || !clientSocket.Connected)
                return null;

            try
            {
                return PerformGet(uri, clientSocket, cookieContainer)?.ToString();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                clientSocket.Close();
                clientSocket.Dispose();
            }
        }

        private Socket ConnectSocket(Uri uri)
        {
            // Detect the correct port based on the protocol scheme 
            int port = uri.Scheme
                .StartsWith("https", 
                    StringComparison.OrdinalIgnoreCase) ? 443 : 80;

            IPHostEntry hostEntry;
            Socket clientSocket = null;

            // Resolve the server name 
            try
            {
                hostEntry = Dns.GetHostEntry(uri.Host);
            }
            catch (Exception)
            {
                return null;
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Attempt to connect on each address returned from DNS. Break out once successfully connected 
            foreach (IPAddress address in hostEntry.AddressList)
            {
                try
                {
                    clientSocket = new Socket(address.AddressFamily, 
                        SocketType.Stream, 
                        ProtocolType.Tcp);
                    var remoteEndPoint = new IPEndPoint(address, port);
                    clientSocket.Connect(remoteEndPoint);
                    break;
                }
                catch (SocketException ex)
                {
                    return null;
                }
            }

            return clientSocket;
        }

        private StringBuilder PerformGet(Uri uri, Socket clientSocket, CookieContainer cookieContainer)
        {
            // Create the cookie string if there is one 
            string cookies = "";
            if (cookieContainer != null && cookieContainer.Count > 0)
            {
                cookies = "\r\nCookie: ";
                foreach (Cookie cookie in GetAllCookies(cookieContainer))
                {
                    cookies += cookie.Name + "=" + cookie.Value + "; ";
                }
            }

            // Format the HTTP GET request string 
            string getRequest = "GET " + uri.PathAndQuery + 
                                " HTTP/1.1\r\nHost: " + uri.Host + 
                                "\r\nConnection: Close" + cookies + 
                                "\r\n\r\n";

            var getBuffer = Encoding.ASCII.GetBytes(getRequest);

            // Send the GET request to the connected server 
            clientSocket.Send(getBuffer);

            // Create a buffer that is used to read the response 
            byte[] rData = new byte[1024];

            // Read the response and save the ASCII data in a string 
            int bytesRead = clientSocket.Receive(rData);

            var response = new StringBuilder();
            while (bytesRead != 0)
            {
                response.Append(Encoding.ASCII.GetChars(rData), 0, bytesRead);
                bytesRead = clientSocket.Receive(rData);
            }

            return response;
        }

        private static CookieCollection GetAllCookies(CookieContainer cookieJar, string scheme = "https")
        {
            var cookieCollection = new CookieCollection();

            Hashtable table = (Hashtable)cookieJar.GetType()
                .InvokeMember("m_domainTable",
                    BindingFlags.NonPublic | 
                    BindingFlags.GetField  | 
                    BindingFlags.Instance,
                    null, cookieJar, new object[]{});
            foreach (string rawKey in table.Keys)
            {
                // Skip dots in the beginning, the key value is the domain name for the cookies 
                var key = rawKey.TrimStart( '.' );

                // Invoke the private function to get the list of cookies 
                var list = (SortedList)table[rawKey].GetType()
                    .InvokeMember("m_list",
                        BindingFlags.NonPublic | 
                        BindingFlags.GetField  | 
                        BindingFlags.Instance,
                        null, table[key], new object[]{});

                foreach (var uri in list.Keys.Cast<string>()
                    .Select( listkey => new Uri(scheme + "://" + key + listkey)))
                {
                    cookieCollection.Add(cookieJar.GetCookies(uri));
                }
            }

            return cookieCollection;
        }
    }
}
