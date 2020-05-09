using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Packaging
{
    public class Package
    {
        #region Private member variables.
        private string _path;
        private EncryptionDetails _encryptionDetails;
        private readonly Dictionary<Uri, Stream> _streams = new Dictionary<Uri, Stream>();
        #endregion

        #region Private constants.
        private const int Iterations = 10000;
        private const string EncryptedFile = "encrypted.json";
        private readonly byte[] _encryptedPrefix = new byte[] {(byte)'E', (byte)'N', (byte)'C', 0};
        #endregion

        #region Constructors.
        private Package()
        {
        }

        public Package([Required] string fullpath)
        {
            if (!File.Exists(fullpath))
                throw new FileNotFoundException();

            _path = fullpath;
        }
        #endregion

        #region Public static member functions.
        public static Package Create([Required] string fullpath)
        {
#pragma warning disable S108 // Nested blocks of code should not be left empty
            using (var package = System.IO.Packaging.Package.Open(fullpath, FileMode.Create, FileAccess.ReadWrite))
            {
            }
#pragma warning restore S108 // Nested blocks of code should not be left empty

            return new Package(fullpath);
        }

        public static Package Create([Required] string fullpath, [Required] string algorithm, [Required] string hashAlgo, [NotNull] byte[] salt)
        {
            var result = Create(fullpath);

            using (var package = System.IO.Packaging.Package.Open(fullpath, FileMode.Create, FileAccess.ReadWrite))
            {
                result._encryptionDetails = new EncryptionDetails()
                {
                    Algorithm = algorithm,
                    HashAlgorithm = hashAlgo,
                    Salt = salt
                };

                var part = package.CreatePart(GetUri(EncryptedFile), "application/json", CompressionOption.Maximum);

                using (var output = part.GetStream(FileMode.Create, FileAccess.Write))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    using (var writer = new StreamWriter(output, Encoding.UTF8))
                    {
                        serializer.Serialize(writer, result._encryptionDetails);
                    }
                }
            }

            return result;
        }

        public static bool IsEncrypted([Required] string fullPath)
        {
            bool result = false;

            using (var package = System.IO.Packaging.Package.Open(fullPath, FileMode.Open, FileAccess.Read))
            {
                result = IsEncrypted(package);
            }

            return result;
        }
        #endregion

        #region Public member functions.
        public byte[] Read([Required] string name, SecureString passphrase = null)
        {
            byte[] result = null;

            using (var package = System.IO.Packaging.Package.Open(_path, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    var uri = GetUri(name);
                    if (package.PartExists(uri))
                    {
                        PackagePart part = package.GetPart(GetUri(name));

                        var stream = part.GetStream();
                        var binary = Read(stream);

                        if (passphrase != null && IsEncrypted(package))
                        {
                            if (_encryptionDetails == null)
                                LoadEncryptionDetails(package);

                            result = Decrypt(binary, passphrase);
                        }
                        else
                        {
                            result = binary;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return result;
        }

        public void Add([Required] string name, [NotNull] byte[] input, SecureString passphrase = null)
        {
            if (passphrase != null && _encryptionDetails == null)
                throw new InvalidOperationException(Properties.Resources.EncryptionNotWellDefined);

            MemoryStream stream = new MemoryStream();
            if (passphrase != null)
            {
                var encrypted = Encrypt(input, passphrase);
                stream.Write(encrypted, 0, encrypted.Length);
            }
            else
            {
                stream.Write(input, 0, input.Length);
            }
            stream.Seek(0, SeekOrigin.Begin);
            _streams.Add(GetUri(name), stream);
        }

        public void Save()
        {
            if (_streams.Count > 0)
            {
                using (var package = System.IO.Packaging.Package.Open(_path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    foreach (var item in _streams)
                    {
                        PackagePart filePart = null;

                        Uri uri = item.Key;

                        if (package.PartExists(uri))
                        {
                            package.DeletePart(uri);
                            package.Flush();
                        }

                        filePart = package.CreatePart(uri, System.Net.Mime.MediaTypeNames.Application.Octet, CompressionOption.Maximum);

                        using (Stream output = filePart?.GetStream(FileMode.Create, FileAccess.Write))
                        {
                            CopyStream(item.Value, output);
                        }
                    }
                }

                _streams.Clear();
            }
        }
        #endregion

        #region Funzioni membro private: funzioni ausiliarie generiche.
        /// <summary>
        /// Ottiene l'uri di una risorsa a partire dal suo nome.
        /// </summary>
        /// <param name="name">Nome della risorsa.</param>
        /// <returns>Uri della risorsa.</returns>
        /// <remarks>Si noti che l'uri deve necessariamente iniziare con "/" e deve essere di tipo relativo.</remarks>
        private static Uri GetUri(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("name");

            Uri result = null;

            if (name.StartsWith("/", StringComparison.Ordinal))
                result = new Uri(name, UriKind.Relative);
            else
                result = new Uri(string.Concat("/", name), UriKind.Relative);

            return result;
        }

        private static byte[] Read(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Copia del contenuto di uno stream presso un altro stream.
        /// </summary>
        /// <param name="input">Stream in ingresso.</param>
        /// <param name="output">Stream in uscita.</param>
        private static void CopyStream(Stream input, Stream output)
        {
            const int bufSize = 0x1000;
            byte[] buf = new byte[bufSize];
            int bytesRead = 0;
            while ((bytesRead = input.Read(buf, 0, bufSize)) > 0)
                output.Write(buf, 0, bytesRead);
            if (output.CanSeek && output.Position > 0)
                output.Seek(0, SeekOrigin.Begin);
        }

        private SymmetricAlgorithm GetAlgorithm(SecureString passphrase)
        {
            if (_encryptionDetails == null)
                throw new InvalidOperationException(Properties.Resources.EncryptionNotWellDefined);

            var result = SymmetricAlgorithm.Create(_encryptionDetails.Algorithm);
#pragma warning disable SCS0011 // CBC mode is weak
            result.Mode = CipherMode.CBC;
#pragma warning restore SCS0011 // CBC mode is weak
            result.Padding = PaddingMode.PKCS7;

            IntPtr unmanagedBytes = Marshal.SecureStringToGlobalAllocUnicode(passphrase);
            byte[] bValue = new byte[passphrase.Length * 2];
            try
            {
                Marshal.Copy(unmanagedBytes, bValue, 0, passphrase.Length * 2);
            }
            finally
            {
                // This will completely remove the data from memory
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedBytes);
            }

            var derive = new Rfc2898DeriveBytes(bValue, _encryptionDetails.Salt, Iterations);
            result.Key = derive.GetBytes(result.KeySize / 8);

            return result;
        }

        private void LoadEncryptionDetails(System.IO.Packaging.Package package)
        {
            var part = package.GetPart(GetUri(EncryptedFile));
            var stream = part.GetStream();
            JsonSerializer serializer = new JsonSerializer();
#pragma warning disable SG0018 // Path traversal
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                _encryptionDetails = (EncryptionDetails) serializer.Deserialize(reader, typeof(EncryptionDetails));
            }
#pragma warning restore SG0018 // Path traversal
        }

        private static bool IsEncrypted([NotNull] System.IO.Packaging.Package package)
        {
            bool result = false;

            try
            {
                package.GetPart(GetUri(EncryptedFile));
                result = true;
            }
#pragma warning disable S2486 // Generic exceptions should not be ignored
            catch
            {
            }
#pragma warning restore S2486 // Generic exceptions should not be ignored

            return result;
        }

        private byte[] Convert(int number)
        {
            byte[] result = new byte[4];

            result[0] = (byte)(number >> 24);
            result[1] = (byte)(number >> 16);
            result[2] = (byte)(number >> 8);
            result[3] = (byte)number;

            return result;
        }

        private int Convert(byte[] number)
        {
            return (((int) number[0]) << 24) + (((int)number[1]) << 16) + (((int)number[2]) << 8) + ((int) number[3]);
        }

        private bool Equal([NotNull] byte[] source, [NotNull] byte[] target)
        {
            bool result = true;
            if (source.Length == target.Length)
            {
                for (int i = 0; i < source.Length; i++)
                {
                    if (source[i] != target[i])
                    {
                        result = false;
                        break;
                    }
                }
            }
            else
            {
                result = false;
            }

            return result;
        }

        private byte[] Encrypt([NotNull] byte[] input, [NotNull] SecureString passphrase)
        {
            IntPtr unmanagedBytes = Marshal.SecureStringToGlobalAllocUnicode(passphrase);
            byte[] bValue = new byte[passphrase.Length * 2];
            try
            {
                Marshal.Copy(unmanagedBytes, bValue, 0, passphrase.Length * 2);
            }
            finally
            {
                // This will completely remove the data from memory
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedBytes);
            }

            using (var algorithm = GetAlgorithm(passphrase))
            {
                algorithm.GenerateIV();
                var iv = algorithm.IV;

                byte[] encrypted;

                using (var encryptor = algorithm.CreateEncryptor(algorithm.Key, iv))
                using (var cipherStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(cipherStream, encryptor, CryptoStreamMode.Write))
                    using (var writer = new BinaryWriter(cryptoStream))
                    {
                        writer.Write(input);
                        cryptoStream.FlushFinalBlock();
                        encrypted = cipherStream.ToArray();
                    }
                }

                // The result is obtained by concatenating the IV and the ciphertext.
                var result = new byte[_encryptedPrefix.Length + sizeof(int) + iv.Length + sizeof(int) + encrypted.Length];
                Array.Copy(_encryptedPrefix, 0, result, 0, _encryptedPrefix.Length);
                Array.Copy(Convert(iv.Length), 0, result, _encryptedPrefix.Length, sizeof(int));
                Array.Copy(iv, 0, result, _encryptedPrefix.Length + sizeof(int), iv.Length);
                Array.Copy(Convert(encrypted.Length), 0, result, _encryptedPrefix.Length + sizeof(int) + iv.Length, sizeof(int));
                Array.Copy(encrypted, 0, result, _encryptedPrefix.Length + sizeof(int) + iv.Length + sizeof(int), encrypted.Length);

                return result;
            }
        }

        private int GetInt([NotNull] byte[] input, int start = 0)
        {
            byte[] intArray = new byte[sizeof(int)];
            Array.Copy(input, start, intArray, 0, sizeof(int));
            return Convert(intArray);
        }

        private byte[] Decrypt([NotNull] byte[] input, [NotNull] SecureString passphrase)
        {
            byte[] result = null;
            int pos = 0;

            var header = new byte[_encryptedPrefix.Length];
            Array.Copy(input, pos, header, 0, _encryptedPrefix.Length);
            pos += _encryptedPrefix.Length;
            if (Equal(header, _encryptedPrefix))
            {
                int ivLen = GetInt(input, _encryptedPrefix.Length);
                pos += sizeof(int);
                var iv = new byte[ivLen];
                Array.Copy(input, pos, iv, 0, ivLen);
                pos += ivLen;

                int encryptedLen = GetInt(input, pos);
                pos += sizeof(int);
                var encrypted = new byte[encryptedLen];
                Array.Copy(input, pos, encrypted, 0, encryptedLen);

                using (var algorithm = GetAlgorithm(passphrase))
                {
                    algorithm.IV = iv;

                    using (var stream = new MemoryStream())
                    {
                        using (var cryptoStream =
                            new CryptoStream(stream, algorithm.CreateDecryptor(), CryptoStreamMode.Write))
                        using (var writer = new BinaryWriter(cryptoStream))
                        {
                            writer.Write(encrypted);
                        }

                        result = stream.ToArray();
                    }
                }
            }

            return result;
        }
        #endregion
    }
}