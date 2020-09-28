using Microsoft.Extensions.Options;
using Otp.API.Models;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Otp.API.Services
{
    public class DokumentumokService : IDokumentumokService
    {
        private readonly IOptions<DokumentumokConfiguration> _settings;
        private readonly IFileSystem _fileSystem;

        public DokumentumokService(IOptions<DokumentumokConfiguration> settings, IFileSystem fileSystem)
        {
            _settings = settings ?? throw new NullReferenceException();
            _fileSystem = fileSystem ?? throw new NullReferenceException();

            if (!_fileSystem.Directory.Exists(_settings.Value.Path))
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Returns a list of file system entries from the directory specified in the config file.
        /// </summary>
        /// <returns>List of file system entries.</returns>
        public IEnumerable<string> GetDokumentumok()
        {
            string dokumentumokPath = _settings.Value.Path;
            var fileSystemEntries = new List<string>();

            foreach (string fileSystemEntry in _fileSystem.Directory.EnumerateFileSystemEntries(dokumentumokPath, "*", System.IO.SearchOption.AllDirectories))
            {
                var relativePath = _fileSystem.Path.GetRelativePath(dokumentumokPath, fileSystemEntry);
                fileSystemEntries.Add(relativePath);
            }

            return fileSystemEntries;
        }

        /// <summary>
        /// If the the file exists return it as base64 encoded string.
        /// If the file doesn't exist return null.
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Base64 encoded string or null</returns>
        public async Task<string> GetDokumentum(string fileName, CancellationToken cancellationToken)
        {
            string dokumentumokPath = _settings.Value.Path;
            string filePath = _fileSystem.Path.Combine(dokumentumokPath, fileName);

            if (!_fileSystem.File.Exists(filePath))
            {
                return null;
            }

            return await FileProcessor.Encoder.EncodeToBase64Async(_fileSystem, filePath, cancellationToken);
        }

        /// <summary>
        /// Returns the file size.
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>File size or null if the file doesn't exist.</returns>
        public long? GetFileSize(string fileName)
        {
            string dokumentumokPath = _settings.Value.Path;
            string filePath = _fileSystem.Path.Combine(dokumentumokPath, fileName);

            if (!_fileSystem.File.Exists(filePath))
            {
                return null;
            }

            return _fileSystem.FileInfo.FromFileName(filePath).Length;
        }

        /// <summary>
        /// Decode the base64 encoded string and write to a file.
        /// </summary>
        /// <param name="relativeFilePath">Relative file path.</param>
        /// <param name="file">Base64 encoded string</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Tuple: (bool Success, string Message)</returns>
        public async Task<(bool, string)> PostDokumentum(string relativeFilePath, string file, CancellationToken cancellationToken)
        {
            // Filename check
            if (string.IsNullOrWhiteSpace(relativeFilePath))
            {
                return (false, "Fájlnév megadása kötelező.");
            }

            string fileName = _fileSystem.Path.GetFileName(relativeFilePath);
            string relativeDirectoryPath = _fileSystem.Path.GetDirectoryName(relativeFilePath);

            // Filename check
            if (fileName.IndexOfAny(_fileSystem.Path.GetInvalidFileNameChars()) != -1)
            {
                return (false, "Érvénytelen fájlnév.");
            }

            // Directory name check
            if (relativeDirectoryPath.IndexOfAny(_fileSystem.Path.GetInvalidPathChars()) != -1)
            {
                return (false, "Érvénytelen mappa név.");
            }

            // Directory check
            string dokumentumokPath = _settings.Value.Path;
            string absoluteDirectoryPath = _fileSystem.Path.Combine(dokumentumokPath, relativeDirectoryPath);
            if (!_fileSystem.Directory.Exists(absoluteDirectoryPath))
            {
                return (false, "Könyvtár nem létezik.");
            }
            
            string absoluteFilePath = _fileSystem.Path.Combine(absoluteDirectoryPath, fileName);

            // If the file is already exist, don't overwrite.
            if (_fileSystem.File.Exists(absoluteFilePath))
            {
                return (false, "Fájl már létezik.");
            }

            try
            {
                await FileProcessor.Encoder.DecodeFromBase64Async(_fileSystem, absoluteFilePath, file, cancellationToken);
            }
            catch (Exception exception)
            {
                return (false, exception.Message);
            }

            return (true, $"Sikeres feltöltés: {relativeFilePath}");
        }
    }
}
