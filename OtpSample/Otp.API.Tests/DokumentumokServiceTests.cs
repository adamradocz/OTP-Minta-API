using Microsoft.Extensions.Options;
using Moq;
using Otp.API.Models;
using Otp.API.Services;
using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Otp.API.Tests
{
    public class DokumentumokServiceTests
    {
        private const string DokumentumokPath = @"c:\Dokumentumok\";
        private readonly Mock<IOptions<DokumentumokConfiguration>> _options;

        public DokumentumokServiceTests()
        {
            DokumentumokConfiguration dokumentumokConfiguration = new DokumentumokConfiguration() { Path = DokumentumokPath };
            _options = new Mock<IOptions<DokumentumokConfiguration>>();
            _options.Setup(conf => conf.Value).Returns(dokumentumokConfiguration);
        }

        [Fact]
        public void DokumentumokService_IOptionsIsNull_ThrowsNullReferenceException()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Assert
            Assert.Throws<NullReferenceException>(() => new DokumentumokService(null, fileSystem));
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
            // Arrange
            var fileSystem = new MockFileSystem();

            // Assert
            Assert.Throws<ArgumentException>(() => new DokumentumokService(_options.Object, fileSystem));
        }
        
        [Fact]
        public void GetDokumentumok_EmptyDokumentumokFolder_ReturnsEmptyIEnumerable()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(DokumentumokPath);
            var service = new DokumentumokService(_options.Object, fileSystem);

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
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(Path.Combine(DokumentumokPath, fileName), new MockFileData("Lorem ipsum."));
            var service = new DokumentumokService(_options.Object, fileSystem);

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
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(DokumentumokPath);
            var service = new DokumentumokService(_options.Object, fileSystem);
            var cancellationToken = new CancellationToken();

            // Act
            var dokumentum = await service.GetDokumentum(requiredDokumentumName, cancellationToken);

            // Assert
            Assert.Null(dokumentum);
        }

        [Fact]
        public async Task GetDokumentum_FileDoesntExist_ReturnsNull()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(DokumentumokPath);
            var service = new DokumentumokService(_options.Object, fileSystem);
            var requiredDokumentumName = "non-existing.txt";
            var cancellationToken = new CancellationToken();

            // Act
            var dokumentum = await service.GetDokumentum(requiredDokumentumName, cancellationToken);

            // Assert
            Assert.Null(dokumentum);
        }
        
        [Fact]
        public async Task GetDokumentum_FileSizeZero_ReturnsEmpty()
        {
            // Arrange
            var emptyFileName = "empty-file.txt";
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(Path.Combine(DokumentumokPath, emptyFileName), new MockFileData(""));
            var service = new DokumentumokService(_options.Object, fileSystem);
            var cancellationToken = new CancellationToken();

            // Act
            var dokumentum = await service.GetDokumentum(emptyFileName, cancellationToken);

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
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(Path.Combine(DokumentumokPath, filePath), new MockFileData("Lorem ipsum."));
            var service = new DokumentumokService(_options.Object, fileSystem);
            var cancellationToken = new CancellationToken();

            // Act
            var dokumentum = await service.GetDokumentum(filePath, cancellationToken);

            // Assert
            Assert.NotEmpty(dokumentum);
        }

        [Fact]
        public void GetFileSize_FileDoesntExist_ReturnsNull()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(DokumentumokPath);
            var service = new DokumentumokService(_options.Object, fileSystem);
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
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(DokumentumokPath);
            var service = new DokumentumokService(_options.Object, fileSystem);
            
            var fileData = "Lorem ipsum.";
            string convertedFile = Convert.ToBase64String(Encoding.UTF8.GetBytes(fileData));
            var cancellationToken = new CancellationToken();

            // Act
            var response = await service.PostDokumentum(fileName, convertedFile, cancellationToken);

            // Assert
            Assert.False(response.Item1);
        }

        [Fact]
        public async Task PostDokumentum_InvalidDirectoryName_ReturnsFalse()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(DokumentumokPath);
            var service = new DokumentumokService(_options.Object, fileSystem);
            string relativeFilePath = "Invalid:Dir/test.txt";
            
            var fileData = "Lorem ipsum.";
            string convertedFile = Convert.ToBase64String(Encoding.UTF8.GetBytes(fileData));
            var cancellationToken = new CancellationToken();

            // Act
            var response = await service.PostDokumentum(relativeFilePath, convertedFile, cancellationToken);

            // Assert
            Assert.False(response.Item1);
        }

        [Fact]
        public async Task PostDokumentum_UploadToNonExistingSubDirectory_ReturnsFalse()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(DokumentumokPath);
            var service = new DokumentumokService(_options.Object, fileSystem);
            string relativeFilePath = "NonExistingDir/test.txt";

            var fileData = "Lorem ipsum.";
            string convertedFile = Convert.ToBase64String(Encoding.UTF8.GetBytes(fileData));
            var cancellationToken = new CancellationToken();

            // Act
            var response = await service.PostDokumentum(relativeFilePath, convertedFile, cancellationToken);

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
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(absoluteDirectoryPath);
            var service = new DokumentumokService(_options.Object, fileSystem);

            var fileData = "Lorem ipsum.";
            string convertedFile = Convert.ToBase64String(Encoding.UTF8.GetBytes(fileData));
            var cancellationToken = new CancellationToken();

            // Act
            var response = await service.PostDokumentum(fileName, convertedFile, cancellationToken);

            // Assert
            Assert.True(response.Item1);
        }

        [Fact]
        public async Task PostDokumentum_UploadZeroSizedFile_ReturnsTrue()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(DokumentumokPath);
            var service = new DokumentumokService(_options.Object, fileSystem);
            string fileName = "test.txt";
            string convertedFile = "";
            var cancellationToken = new CancellationToken();

            // Act
            var response = await service.PostDokumentum(fileName, convertedFile, cancellationToken);

            // Assert
            Assert.True(response.Item1);
        }

        [Fact]
        public async Task PostDokumentum_UploadBadCodedFile_ReturnsFalse()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(DokumentumokPath);
            var service = new DokumentumokService(_options.Object, fileSystem);
            string fileName = "test.txt";
            string wrongConvertedFile = "AAAAAAA";
            var cancellationToken = new CancellationToken();

            // Act
            var response = await service.PostDokumentum(fileName, wrongConvertedFile, cancellationToken);

            // Assert
            Assert.False(response.Item1);
        }
    }
}
