using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Utilities;

namespace ThreatsManager
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LockFile : LockInfo
    {
        private static object _sync = new object();
        private string _fileName;

        private LockFile()
        {

        }

        private LockFile([Required] string fileName)
        {
            _fileName = fileName;
        }

        [JsonProperty("acquired")]
        public bool IsAcquired { get; private set; }

        [JsonProperty("requests")]
        private List<LockInfo> _requests;

        public IEnumerable<LockInfo> Requests => _requests;

        public bool IsOwned =>
            string.Compare(User, UserName.GetDisplayName(), StringComparison.InvariantCultureIgnoreCase) == 0 &&
            string.Compare(Machine, CurrentMachine, StringComparison.InvariantCultureIgnoreCase) == 0;

        public bool Acquire()
        {
            bool result = false;

            if (IsOwned && !IsAcquired)
            {
                IsAcquired = true;
                Save();

                result = true;
            }

            return result;
        }

        public bool Book()
        {
            bool result = false;

            if (!IsOwned)
            {
                var lockInfo = new LockInfo();
                lockInfo.AutoLoad();

                if (!(Requests?.Any(x =>
                    string.Compare(x.User, lockInfo.User, StringComparison.InvariantCultureIgnoreCase) == 0 &&
                    string.Compare(x.Machine, lockInfo.Machine, StringComparison.InvariantCultureIgnoreCase) ==
                    0) ?? false))
                {
                    // The request is new.
                    if (_requests == null)
                        _requests = new List<LockInfo>();
                    _requests.Add(lockInfo);

                    Save();
                }

                result = true;
            }

            return result;
        }

        public void Release()
        {
            if (IsOwned)
            {
                var first = _requests?.FirstOrDefault();
                if (first != null)
                {
                    _requests.Remove(first);
                    User = first.User;
                    Machine = first.Machine;
                    Timestamp = DateTime.Now;
                    IsAcquired = false;
                    Save();
                }
                else
                {
                    File.Delete(_fileName);
                }
            }
            else
            {
                var request = _requests?.FirstOrDefault(x =>
                    string.Compare(x.User, UserName.GetDisplayName(), StringComparison.InvariantCultureIgnoreCase) == 0 &&
                    string.Compare(x.Machine, CurrentMachine, StringComparison.InvariantCultureIgnoreCase) == 0);

                if (request != null)
                {
                    _requests.Remove(request);
                    Save(); 
                }
            }
        }

        public static LockFile Load(string fileName)
        {
            LockFile result = null;

            if (!string.IsNullOrWhiteSpace(fileName) && File.Exists(fileName))
            {
                try
                {
                    lock (_sync)
                    {
                        using (var fileStream = new FileStream(fileName, FileMode.Open))
                        {
                            var serializer = new JsonSerializer();
                            using (var stringReader = new StreamReader(fileStream))
                            using (var reader = new JsonTextReader(stringReader))
                            {
                                result = serializer.Deserialize<LockFile>(reader);
                                if (result != null)
                                    result._fileName = fileName;
                            }
                        }

                        result?.MoveNext();
                    }
                }
                catch (FileNotFoundException)
                {
                }
            }

            if (result == null && !string.IsNullOrWhiteSpace(fileName))
            {
                result = new LockFile(fileName);
                result.AutoLoad();
                result.Save();
            }

            return result;
        }

        private void MoveNext()
        {
            if (!IsOwned && !IsAcquired &&
                DateTime.Now - Timestamp > TimeSpan.FromMinutes(30.0))
            {
                var first = Requests?.FirstOrDefault();
                if (first != null)
                {
                    User = first.User;
                    Machine = first.Machine;
                    _requests.Remove(first);
                }
                else
                {
                    User = UserName.GetDisplayName();
                    Machine = CurrentMachine;
                }
                Timestamp = DateTime.Now;
                IsAcquired = false;
                Save();
            }
        }

        private void Save()
        {
            lock (_sync)
            {
                using (var fileStream = new FileStream(_fileName, FileMode.Create))
                {
                    var serializer = new JsonSerializer {Formatting = Formatting.Indented};
                    using (var stringWriter = new StreamWriter(fileStream))
                    using (var writer = new JsonTextWriter(stringWriter))
                        serializer.Serialize(writer, this);
                }
            }
        }
    }
}