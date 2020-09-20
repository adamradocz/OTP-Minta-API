using Microsoft.Extensions.Options;
using Moq;
using Otp.API.Models;
using Otp.API.Services;
using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Otp.API.Tests
{
    public class DokumentumokServiceTests
    {
        private const string DokumentumokPath = @"c:\Dokumentumok\";

        private readonly Mock<IOptions<DokumentumokConfiguration>> _options;
        private readonly MockFileSystem _fileSystem;

        public DokumentumokServiceTests()
        {
            DokumentumokConfiguration dokumentumokConfiguration = new DokumentumokConfiguration() { Path = DokumentumokPath };
            _options = new Mock<IOptions<DokumentumokConfiguration>>();
            _options.Setup(conf => conf.Value).Returns(dokumentumokConfiguration);

            _fileSystem = new MockFileSystem();
        }

        [Fact]
        public void DokumentumokService_IOptionsIsNull_ThrowsNullReferenceException()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() => new DokumentumokService(null, _fileSystem));
        }
        
        [Fact]
        public void DokumentumokService_IFileSystemIsNull_ThrowsNullReferenceException()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() => new DokumentumokService(_options.Object, null));
        }
        
        [Fact]
        public void DokumentumokService_DokumentumokConfigurationPathDoesntExist_ThrowsArgumentException()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => new DokumentumokService(_options.Object, _fileSystem));
        }
        
        [Fact]
        public void GetDokumentumok_EmptyDokumentumokFolder_ReturnsEmptyIEnumerable()
        {
            // Arrange
            _fileSystem.AddDirectory(DokumentumokPath);
            var service = new DokumentumokService(_options.Object, _fileSystem);

            // Act
            var fileSystemEntries = service.GetDokumentumok();

            // Assert
            Assert.Empty(fileSystemEntries);
        }
        
        [Theory]
        [InlineData("test.txt")]
        [InlineData(@"Sub-folder\sub-test.txt")]
        public void GetDokumentumok_TrimDokumentumokDirectory_ReturnRelativeEntriesPath(string fileName)
        {
            // Arrange
            _fileSystem.AddFile(Path.Combine(DokumentumokPath, fileName), new MockFileData("Lorem ipsum."));
            var service = new DokumentumokService(_options.Object, _fileSystem);

            // Act
            var fileSystemEntries = service.GetDokumentumok().ToArray();
            
            // Assert
            Assert.Equal(fileName, fileSystemEntries[0]);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(">test.txt")]
        [InlineData(@"sub:dir/test.txt")]
        public async Task GetDokumentum_InvalidFileName_ReturnsNull(string requiredDokumentumName)
        {
            // Arrange
            _fileSystem.AddDirectory(DokumentumokPath);
            var service = new DokumentumokService(_options.Object, _fileSystem);

            // Act
            var dokumentum = await service.GetDokumentum(requiredDokumentumName);

            // Assert
            Assert.Null(dokumentum);
        }

        [Fact]
        public async Task GetDokumentum_FileDoesntExist_ReturnsNull()
        {
            // Arrange
            _fileSystem.AddDirectory(DokumentumokPath);
            var service = new DokumentumokService(_options.Object, _fileSystem);
            var requiredDokumentumName = "non-existing.txt";

            // Act
            var dokumentum = await service.GetDokumentum(requiredDokumentumName);

            // Assert
            Assert.Null(dokumentum);
        }
        
        [Fact]
        public async Task GetDokumentum_FileSizeZero_ReturnsEmpty()
        {
            // Arrange
            var emptyFileName = "empty-file.txt";
            _fileSystem.AddFile(Path.Combine(DokumentumokPath, emptyFileName), new MockFileData(""));
            var service = new DokumentumokService(_options.Object, _fileSystem);

            // Act
            var dokumentum = await service.GetDokumentum(emptyFileName);

            // Assert
            Assert.Empty(dokumentum);
        }

        [Theory]
        [InlineData("sub-folder/test-file.txt")]
        [InlineData(@"sub-folder\test-file.txt")]
        [InlineData(@"sub-folder\\test-file.txt")]
        [InlineData(@"sub-folder/sub-sub-folder\test-file.txt")]
        [InlineData(@"sub-folder/sub-sub-folder\\test-file.txt")]
        [InlineData(@"sub-folder\sub-sub-folder/test-file.txt")]
        [InlineData(@"sub-folder\sub-sub-folder\\test-file.txt")]
        public async Task GetDokumentum_SubFolderFile_ReturnsNotEmpty(string filePath)
        {
            // Arrange
            _fileSystem.AddFile(Path.Combine(DokumentumokPath, filePath), new MockFileData("Lorem ipsum."));
            var service = new DokumentumokService(_options.Object, _fileSystem);

            // Act
            var dokumentum = await service.GetDokumentum(filePath);

            // Assert
            Assert.NotEmpty(dokumentum);
        }

        [Fact]
        public void GetFileSize_FileDoesntExist_ReturnsNull()
        {
            // Arrange
            _fileSystem.AddDirectory(DokumentumokPath);
            var service = new DokumentumokService(_options.Object, _fileSystem);
            string filName = "non-existing.txt";

            // Act
            var fileSize = service.GetFileSize(filName);

            // Assert
            Assert.Null(fileSize);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(">test.txt")]
        public async Task PostDokumentum_InvalidFileName_ReturnsFalse(string fileName)
        {
            // Arrange
            _fileSystem.AddDirectory(DokumentumokPath);
            var service = new DokumentumokService(_options.Object, _fileSystem);
            string base64File = "TG9yZW0gaXBzdW0u"; // Base64: "Lorem ipsum."

            // Act
            var response = await service.PostDokumentum(fileName, base64File);

            // Assert
            Assert.False(response.Item1);
        }

        [Fact]
        public async Task PostDokumentum_InvalidDirectoryName_ReturnsFalse()
        {
            // Arrange
            _fileSystem.AddDirectory(DokumentumokPath);
            var service = new DokumentumokService(_options.Object, _fileSystem);
            string relativeFilePath = "Invalid:Dir/test.txt";
            string base64File = "TG9yZW0gaXBzdW0u"; // Base64: "Lorem ipsum."

            // Act
            var response = await service.PostDokumentum(relativeFilePath, base64File);

            // Assert
            Assert.False(response.Item1);
        }

        [Fact]
        public async Task PostDokumentum_UploadToNonExistingSubDirectory_ReturnsFalse()
        {
            // Arrange
            _fileSystem.AddDirectory(DokumentumokPath);
            var service = new DokumentumokService(_options.Object, _fileSystem);
            string relativeFilePath = "NonExistingDir/test.txt";
            string base64File = "TG9yZW0gaXBzdW0u"; // Base64: "Lorem ipsum."

            // Act
            var response = await service.PostDokumentum(relativeFilePath, base64File);

            // Assert
            Assert.False(response.Item1);
        }

        [Theory]
        [InlineData(@"Dir\", "Dir/test.txt")]
        [InlineData(@"Dir\", @"Dir\test.txt")]
        [InlineData(@"Dir\", @"Dir\\test.txt")]
        [InlineData(@"Dir\Sub\", "Dir/Sub/test.txt")]
        [InlineData(@"Dir\Sub\", @"Dir\Sub\test.txt")]
        [InlineData(@"Dir\Sub\", @"Dir\\Sub\\test.txt")]
        [InlineData(@"Dir\Sub\", @"Dir\\Sub/test.txt")]
        [InlineData(@"Dir\Sub\", @"Dir\\Sub//test.txt")]
        public async Task PostDokumentum_UploadToExistingSubDirectory_ReturnsTrue(string directoryPath, string fileName)
        {
            // Arrange
            string absoluteDirectoryPath = Path.Combine(DokumentumokPath, directoryPath);
            _fileSystem.AddDirectory(absoluteDirectoryPath);
            var service = new DokumentumokService(_options.Object, _fileSystem);
            string base64File = "TG9yZW0gaXBzdW0u"; // Base64: "Lorem ipsum."

            // Act
            var response = await service.PostDokumentum(fileName, base64File);

            // Assert
            Assert.True(response.Item1);
        }

        [Fact]
        public async Task PostDokumentum_UploadZeroSizedFile_ReturnsTrue()
        {
            // Arrange
            _fileSystem.AddDirectory(DokumentumokPath);
            var service = new DokumentumokService(_options.Object, _fileSystem);
            string fileName = "test.txt";
            string base64File = "";

            // Act
            var response = await service.PostDokumentum(fileName, base64File);

            // Assert
            Assert.True(response.Item1);
        }

        [Fact]
        public async Task PostDokumentum_UploadBadCodedFile_ReturnsFalse()
        {
            // Arrange
            _fileSystem.AddDirectory(DokumentumokPath);
            var service = new DokumentumokService(_options.Object, _fileSystem);
            string fileName = "test.txt";
            string worngBase64File = "AAAAAAA";

            // Act
            var response = await service.PostDokumentum(fileName, worngBase64File);

            // Assert
            Assert.False(response.Item1);
        }
    }
}
