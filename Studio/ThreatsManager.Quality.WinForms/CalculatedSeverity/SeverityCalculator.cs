﻿using System;
using System.Linq;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Quality.CalculatedSeverity
{
    public static class SeverityCalculator
    {
        public static ISeverity GetCalculatedSeverity(this IThreatEvent threatEvent, int configDelta)
        {
            ISeverity result = null;

            if (threatEvent != null)
            {
                var totalStrength = (threatEvent.Mitigations?
                    .Where(x => x.Status == MitigationStatus.Existing || x.Status == MitigationStatus.Implemented)
                    .Sum(x => x.StrengthId) ?? 0) - configDelta;
                var projected =
                    (int) Math.Max(Math.Ceiling(threatEvent.ThreatType.SeverityId * (1.0 - (Math.Min(totalStrength, 100.0) / 100.0))), 1.0);
                result = threatEvent.Model?.GetMappedSeverity(projected);
            }

            return result;
        }
    }
}
