using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Engine.Config
{
    public class CertificateConfig : ConfigurationElement
    {
        public CertificateConfig()
        {

        }

        public CertificateConfig([NotNull] X509Certificate certificate) : this(new X509Certificate2(certificate))
        {
        }

        public CertificateConfig([NotNull] X509Certificate2 certificate) : this()
        {
            Thumbprint = certificate.Thumbprint;
            Subject = certificate.Subject;
            Issuer = certificate.Issuer;
            ExpirationDate = certificate.GetExpirationDateString();
        }

        [ConfigurationProperty("thumbprint", IsRequired = true, IsKey = true)]
        public string Thumbprint
        {
            get => (string)this["thumbprint"];
            set => this["thumbprint"] = value;
        }

        [ConfigurationProperty("subject", IsRequired = true, IsKey = false)]
        public string Subject
        {
            get => (string)this["subject"];
            set => this["subject"] = value;
        }

        [ConfigurationProperty("issuer", IsRequired = true, IsKey = false)]
        public string Issuer
        {
            get => (string)this["issuer"];
            set => this["issuer"] = value;
        }

        [ConfigurationProperty("expiration", IsRequired = true, IsKey = false)]
        public string ExpirationDate
        {
            get => (string) this["expiration"];
            set => this["expiration"] = value;
        }

        public override string ToString()
        {
            return Subject;
        }
    }
}
