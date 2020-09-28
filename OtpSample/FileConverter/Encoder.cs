using System;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace FileProcessor
{
    public static class Encoder
    {
        /// <summary>
        /// Convert a file to Base64 string.
        /// </summary>
        /// <param name="fileSystem">Filesystem abstraction.</param>
        /// <param name="filePath">Path of the file.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Base64 coded string.</returns>
        public static async Task<string> EncodeToBase64Async(IFileSystem fileSystem, string filePath, CancellationToken cancellationToken)
        {
            var fileData = await fileSystem.File.ReadAllBytesAsync(filePath, cancellationToken);
            return Convert.ToBase64String(fileData);
        }

        /// <summary>
        /// Create a file from a Base64 encoded string.
        /// </summary>
        /// <param name="fileSystem">Filesystem abstraction.</param>
        /// <param name="path">File name.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="base64String">Base64 encoded string.</param>
        public static async Task DecodeFromBase64Async(IFileSystem fileSystem, string path, string base64String, CancellationToken cancellationToken)
        {
            await fileSystem.File.WriteAllBytesAsync(path, Convert.FromBase64String(base64String), cancellationToken);
        }
    }
}
