using System;
using System.IO;
using System.Linq;
using System.Threading;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Threading;

namespace ThreatsManager
{
    public class DocumentLocker
    {
        private const string LockExt = ".tmlock";
        private static string _fileName;
        private static DateTime _lastWrite = DateTime.MinValue;
        private static bool _owned;
        private static bool _poll;

        public static event Action<string> OwnershipObtained;
        public static event Action<string, string, DateTime, int> OwnershipRequested;

        public static bool AcquireLock([Required] string fileName, bool book = true)
        {
            return AcquireLock(fileName, book, out var owner, 
                out var machine, out var timestamp, out var requests);
        }

        public static bool AcquireLock([Required] string fileName, bool book,
            out string owner, out string machine, out DateTime timestamp, out int requests)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("Threat Model file not found", fileName);
            var directory = Path.GetDirectoryName(fileName);
            if (string.IsNullOrWhiteSpace(directory))
                throw new DirectoryNotFoundException();

            bool result = false;
            owner = null;
            machine = null;
            timestamp = DateTime.MinValue;
            requests = 0;

            if (ReleaseLock())
            {
                LockFile lockFile = null;

                var fileNameWithoutExtensions = Path.GetFileNameWithoutExtension(fileName);
                _fileName = $"{Path.Combine(directory, fileNameWithoutExtensions)}{LockExt}";

                try
                {
                    lockFile = LockFile.Load(_fileName);
                }
                catch
                {
                    // Ignore errors.
                }

                result = _owned = lockFile?.IsOwned ?? false;

                if (_owned)
                {
                    lockFile?.Acquire();
                }
                else if (book)
                {
                    lockFile?.Book();
                }

                owner = lockFile?.User;
                machine = lockFile?.Machine;
                timestamp = lockFile?.Timestamp ?? DateTime.MinValue;
                requests = lockFile?.Requests?.Count() ?? 0;

                FileWatcher();
            }

            return result;
        }

        public static bool ReleaseLock(bool force = false)
        {
            var result = string.IsNullOrWhiteSpace(_fileName);

            if (!result)
            {
                _poll = false;

                if (force)
                {
                    try
                    {
                        var lockFile = LockFile.Load(_fileName);
                        lockFile?.Release();
                        result = lockFile != null;
                    }
                    catch
                    {
                    }

                    _lastWrite = DateTime.MinValue;
                    _fileName = null;
                    _owned = false;
                }
            }

            return result;
        }

        [Background]
        private static void FileWatcher()
        {
            _poll = true;

            do
            {
                var lastWrite = File.GetLastWriteTime(_fileName);
                if ((lastWrite.Year == 1601 || _lastWrite < lastWrite))
                {
                    var lockFile = LockFile.Load(_fileName);
                    if (_owned)
                    {
                        if ((lockFile?.IsOwned ?? false) && (lockFile.Requests?.Any() ?? false))
                        {
                            var first = lockFile.Requests.FirstOrDefault();
                            if (first != null)
                                OwnershipRequested?.Invoke(first.User, first.Machine, first.Timestamp,
                                    lockFile.Requests.Count());
                        }
                    }
                    else
                    {
                        _owned = lockFile.IsOwned;
                        if (_owned)
                        {
                            OwnershipObtained?.Invoke(Path.GetFileNameWithoutExtension(_fileName));
                        }
                    }
                }

                if (lastWrite.Year != 1601)
                    _lastWrite = lastWrite;

                Thread.Sleep(10000);
            } while (_poll);

            LockFile.Load(_fileName)?.Release();

            _lastWrite = DateTime.MinValue;
            _fileName = null;
            _owned = false;
        }
    }
}
