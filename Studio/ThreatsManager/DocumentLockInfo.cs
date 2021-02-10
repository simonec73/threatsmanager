using System;
using System.Diagnostics.Eventing.Reader;

namespace ThreatsManager
{
    public class DocumentLockInfo
    {
        public DocumentLockInfo(bool owned, string owner, string machine, DateTime timestamp, int pendingRequests)
        {
            Owned = owned;
            Owner = owner;
            Machine = machine;
            Timestamp = timestamp;
            PendingRequests = pendingRequests;
        }

        public bool Owned { get; }

        public string Owner { get; }

        public string Machine { get; }

        public DateTime Timestamp { get; }

        public int PendingRequests { get; }
    }
}