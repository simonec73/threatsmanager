using System.Xml;

namespace ThreatsManager.MsTmt.Model
{
    public partial class ThreatModel
    {
        #region Public properties.
        public string Assumptions
        {
            get
            {
                XmlNode node = _document.SelectSingleNode(
                    "/*[local-name()='ThreatModel']/*[local-name()='MetaInformation']/*[local-name()='Assumptions']");
                return node?.InnerText;
            }
        }

        public string Contributors
        {
            get
            {
                XmlNode node = _document.SelectSingleNode(
                    "/*[local-name()='ThreatModel']/*[local-name()='MetaInformation']/*[local-name()='Contributors']");
                return node?.InnerText;
            }
        }

        public string ExternalDependencies
        {
            get
            {
                XmlNode node = _document.SelectSingleNode(
                    "/*[local-name()='ThreatModel']/*[local-name()='MetaInformation']/*[local-name()='ExternalDependencies']");
                return node?.InnerText;
            }
        }

        public string HighLevelSystemDescription
        {
            get
            {
                XmlNode node = _document.SelectSingleNode(
                    "/*[local-name()='ThreatModel']/*[local-name()='MetaInformation']/*[local-name()='HighLevelSystemDescription']");
                return node?.InnerText;
            }
        }

        public string Owner
        {
            get
            {
                XmlNode node = _document.SelectSingleNode(
                    "/*[local-name()='ThreatModel']/*[local-name()='MetaInformation']/*[local-name()='Owner']");
                return node?.InnerText;
            }
        }

        public string Reviewer
        {
            get
            {
                XmlNode node = _document.SelectSingleNode(
                    "/*[local-name()='ThreatModel']/*[local-name()='MetaInformation']/*[local-name()='Reviewer']");
                return node?.InnerText;
            }
        }

        public string ThreatModelName
        {
            get
            {
                XmlNode node = _document.SelectSingleNode(
                    "/*[local-name()='ThreatModel']/*[local-name()='MetaInformation']/*[local-name()='ThreatModelName']");
                return node?.InnerText;
            }
       }
        #endregion
    }
}
