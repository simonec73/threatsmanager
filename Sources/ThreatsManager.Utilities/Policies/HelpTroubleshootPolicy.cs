namespace ThreatsManager.Utilities.Policies
{
    public class HelpTroubleshootPolicy : Policy
    {
        protected override string PolicyName => "HelpTroubleshoot";

        public bool? HelpTroubleshoot => BoolValue;
    }
}
