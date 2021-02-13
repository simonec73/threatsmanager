using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Utilities;

namespace ThreatsManager
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LockFile : LockInfo
    {
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

        public async Task<bool> AcquireAsync()
        {
            bool result = false;

            if (IsOwned && !IsAcquired)
            {
                IsAcquired = true;
                await SaveAsync();

                result = true;
            }

            return result;
        }

        public async Task<bool> BookAsync()
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

                    await SaveAsync();
                }

                result = true;
            }

            return result;
        }

        public async Task ReleaseAsync()
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
                    await SaveAsync();
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
                    await SaveAsync(); 
                }
            }
        }

        public static async Task<LockFile> LoadAsync(string fileName)
        {
            LockFile result = null;

            if (!string.IsNullOrWhiteSpace(fileName) && File.Exists(fileName))
            {
                try
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

                    if (result != null) 
                        await result.MoveNextAsync();
                }
                catch (FileNotFoundException)
                {
                }
            }

            if (result == null && !string.IsNullOrWhiteSpace(fileName))
            {
                result = new LockFile(fileName);
                result.AutoLoad();
                await result.SaveAsync();
            }

            return result;
        }

        private async Task MoveNextAsync()
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
                await SaveAsync();
            }
        }

        private async Task SaveAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var fileStream = new FileStream(_fileName, FileMode.Create))
                    {
                        var serializer = new JsonSerializer {Formatting = Formatting.Indented};
                        using (var stringWriter = new StreamWriter(fileStream))
                        using (var writer = new JsonTextWriter(stringWriter))
                            serializer.Serialize(writer, this);
                    }
                }
                catch (IOException)
                {
                    // May happen if the file is in use somewhere else. It will be eventually saved.
                }
            });
        }
    }
}