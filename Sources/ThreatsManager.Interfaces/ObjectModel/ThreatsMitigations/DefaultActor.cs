namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Enumeration of Threat Actors.
    /// </summary>
    public enum DefaultActor
    {
        /// <summary>
        /// Unknown or non-default Threat Actor.
        /// </summary>
        [EnumDescription("Unknown or non-default Threat Actor.")]
        Unknown,

        /// <summary>
        /// Malicious Administrator.
        /// </summary>
        [EnumLabel("Malicious Administrator")]
        [EnumDescription("Malicious Administrators are users with administrative rights on some resource. Those users are entitled to access the resource with the highest rights, therefore it may be difficult to prevent them to misuse the system they are required to manage. They may have different reasons to attack the solution: because they are disgruntled, more generically because they are corrupted, or even for simple curiosity. Different reasons may involve different types of risks, because for example if curiosity is the motivation, then the number of affected records would be limited. Typically, you have simply to ensure that they are trustworthy, to rely on Deterrent controls and in general to remove all major reasons for discontent.")]
        MaliciousAdministrator,

        /// <summary>
        /// Malicious privileged user.
        /// </summary>
        /// <remarks>It includes all privileged users which are not Administrators.</remarks>
        [EnumLabel("Malicious Privileged User")]
        [EnumDescription("Malicious users who are not Administrators, but have some additional privilege over  some resource or the solution. Those users are entitled to access the resource with some privilege, therefore it may be difficult to prevent them to misuse the system. They may have different reasons to attack the solution: because they are disgruntled, more generically because they are corrupted, or even for simple curiosity. Different reasons may involve different types of risks, because for example if curiosity is the motivation, then the number of affected records would be limited. Typically, you have simply to ensure that they are trustworthy, to rely on Deterrent controls and in general to remove all major reasons for discontent.")]
        MaliciousPrivilegedUser,

        /// <summary>
        /// Malicious unprivileged users.
        /// </summary>
        [EnumLabel("Malicious Unprivileged User")]
        [EnumDescription("Malicious users who have no specific privileges. They may have different reasons to attack the solution: because they are disgruntled, more generically because they are corrupted, or even for simple curiosity. Different reasons may involve different types of risks, because for example if curiosity is the motivation, then the number of affected records would be limited.")]
        MaliciousUnprivilegedUser,

        /// <summary>
        /// Script Kiddies.
        /// </summary>
        [EnumLabel("Script Kiddie")]
        [EnumDescription("Script Kiddies are hackers wannabe with limited technical knowledge. They typically use scripts and tools found on Internet, and identify weaknesses by simple scanning. They typically are moved by curiosity and by spirit of adventure, more than by greed: many times, their goal is to advertise their name and get some fame. Protection against known common attacks typically represents an effective approach against this type of attackers, because they are rarely determined adversaries.")]
        ScriptKiddie,

        /// <summary>
        /// Cyber Criminals.
        /// </summary>
        [EnumLabel("Cyber Criminal")]
        [EnumDescription("Cyber Criminals are adversaries moved by greed. They have variable degrees of technical proficiency, but typically they tend to stick with the most reliable common attacks. Though, they tend to be more experienced and determined than Script Kiddies, and therefore they should be considered dangerous adversaries. It is important to note that Cyber Criminals are moved by business interest, therefore raising the cost for them may represent an effective approach to discourage them from attacking. ")]
        CyberCriminal,

        /// <summary>
        /// Hacktivists.
        /// </summary>
        [EnumDescription("Hacktivists are adversaries moved by their moral: one common goal is to ensure that their point of view is known and that the victim organization is defamed. They have variable degrees of technical proficiency, but typically they tend to stick with the most reliable common attacks. They do not seek a financial gain, therefore raising the cost may be only partially effective to discourage them from attacking. They should be considered dangerous adversaries.")]
        Hacktivist,

        /// <summary>
        /// State sponsored attackers.
        /// </summary>
        [EnumLabel("State Sponsored Attacker")]
        [EnumDescription("State Sponsored attackers are the most dangerous adversaries. They may have great competencies and virtually unlimited resources: their arsenal may include zero days and custom tools. They are determined, and typically they are very hard to protect against. It is true, though, that they need to use those 'superpowers' very rarely, because organizations are mostly vulnerable to simple attacks.")]
        StateSponsored,
    }
}