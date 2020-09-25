using System;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace FileProcessor
{
    public static class Encoder
    {
        /// <summary>
        /// Convert a file to Base64 string.
        /// </summary>
        /// <param name="filePath">Path of the file.</param>
        /// <returns>Base64 coded string.</returns>
        public static async Task<string> EncodeToBase64Async(IFileSystem fileSystem, string filePath)
        {
            var fileData = await fileSystem.File.ReadAllBytesAsync(filePath);
            return Convert.ToBase64String(fileData);
        }

        /// <summary>
        /// Create a file from a Base64 encoded string.
        /// </summary>
        /// <param name="path">File name.</param>
        /// <param name="base64String">Base64 encoded string.</param>
        public static async Task DecodeFromBase64Async(IFileSystem fileSystem, string path, string base64String)
        {
            await fileSystem.File.WriteAllBytesAsync(path, Convert.FromBase64String(base64String));
        }
    }
}
