using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThreatsManager.Engine.Config;
using ThreatsManager.Utilities;

namespace ThreatsManager.Engine.Policies
{
    public class CertificatesPolicy : Policy
    {
        protected override string PolicyName => "Certificates";

        public IEnumerable<CertificateConfig> Certificates
        {
            get
            {
                IEnumerable<CertificateConfig> result = null;

                var rows = StringArrayValue;
                if (rows?.Any() ?? false)
                {
                    List<CertificateConfig> certificates = null;
                    Regex regex = new Regex("/<certificate thumbprint=\"(?'thumbprint'[^\"]+)\" subject=\"(?'subject'[^\"]+)\" issuer=\"(?'issuer'[^\"]+)\" expiration=\"(?'expiration'[^\"]+)\"/gs");
                    foreach (var row in rows)
                    {
                        var match = regex.Match(row);
                        if (match.Success)
                        {
                            if (certificates == null)
                                certificates = new List<CertificateConfig>();

                            var cert = new CertificateConfig()
                            {
                                Thumbprint = match.Groups["thumbprint"]?.Value,
                                Subject = match.Groups["subject"]?.Value,
                                Issuer = match.Groups["issuer"]?.Value,
                                ExpirationDate = match.Groups["expiration"]?.Value
                            };
                            certificates.Add(cert);
                        }
                    }

                    result = certificates?.ToArray();
                }

                return result;
            }
        }
    }
}
