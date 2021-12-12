using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private static CancellationTokenSource _cancellation;
        private static Task _task;

        public static event Action<string> OwnershipObtained;
        public static event Action<string, string, DateTime, int> OwnershipRequested;

        public static async Task<DocumentLockInfo> AcquireLockAsync([Required] string fileName, bool book = true)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("Threat Model file not found", fileName);
            var directory = Path.GetDirectoryName(fileName);
            if (string.IsNullOrWhiteSpace(directory))
                throw new DirectoryNotFoundException();

            DocumentLockInfo result = null;

            ReleaseLock();
            LockFile lockFile = null;

            var fileNameWithoutExtensions = Path.GetFileNameWithoutExtension(fileName);
            _fileName = $"{Path.Combine(directory, fileNameWithoutExtensions)}{LockExt}";

            try
            {
                lockFile = await LockFile.LoadAsync(_fileName);
            }
            catch
            {
                // Ignore errors.
            }

            _owned = lockFile?.IsOwned ?? false;

            if (lockFile != null)
            {
                if (_owned)
                {
                    await lockFile.AcquireAsync();
                }
                else if (book)
                {
                    await lockFile.BookAsync();
                }

                result = new DocumentLockInfo(_owned, lockFile.User, lockFile.Machine,
                    lockFile.Timestamp, lockFile.Requests?.Count() ?? 0);

                _cancellation = new CancellationTokenSource();
                _task = FileWatcherAsync(_cancellation.Token);
            }

            return result;
        }

        public static void ReleaseLock()
        {
            if (!string.IsNullOrWhiteSpace(_fileName))
            {
                _cancellation?.Cancel();
                _task?.Wait(2000);
            }
        }

        [Background(IsLongRunning = true)]
        private static async Task FileWatcherAsync(CancellationToken cancellation)
        {
            LockFile lockFile;

            do
            {
                if (cancellation.IsCancellationRequested)
                {
                    await CancelAsync(cancellation);
                    await Task.Delay(1000);
                }

                if (!string.IsNullOrWhiteSpace(_fileName))
                {
                    var lastWrite = File.GetLastWriteTime(_fileName);
                    if ((lastWrite.Year == 1601 || _lastWrite < lastWrite))
                    {
                        lockFile = await LockFile.LoadAsync(_fileName);
                        if (lockFile != null)
                        {
                            if (_owned)
                            {
                                if (lockFile.IsOwned && (lockFile.Requests?.Any() ?? false))
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
                    }

                    if (lastWrite.Year != 1601)
                        _lastWrite = lastWrite;
                }

                if (cancellation.IsCancellationRequested)
                {
                    await CancelAsync(cancellation);
                    await Task.Delay(1000);
                }
                else
                {
                    await Task.Delay(3000);
                }
            } while (true);
        }

        private static async Task CancelAsync(CancellationToken cancellation)
        {
            var lockFile = await LockFile.LoadAsync(_fileName);
            if (lockFile != null)
                await lockFile.ReleaseAsync();

            _lastWrite = DateTime.MinValue;
            _fileName = null;
            _owned = false;

            cancellation.ThrowIfCancellationRequested();
        }
    }
}
